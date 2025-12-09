using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dave6.StatSystem
{
    public abstract class BaseStat
    {
        public StatDefinition definition { get; private set; }
        int m_BaseValue;
        public int baseValue => m_BaseValue;

        /// <summary>
        /// UI, 애니메이션, 사운드, 효과, 기타 시스템에 이벤트 전달
        /// </summary>
        public event Action onValueChanged;

        // lazy 계산은 시스템이 자리잡고 나서 고려할 사항        
        // bool m_IsDirty = true;

        protected int m_FinalValue;
        public int finalValue => m_FinalValue;

        protected List<StatModifier> m_Modifiers = new();

        public BaseStat(StatDefinition definition)
        {
            this.definition = definition;
            m_BaseValue = definition.initialValue;
            m_FinalValue = definition.initialValue;
        }

        public virtual void Initialize()
        {
            CalculateValue();
        }

        /// <summary>
        /// 1) base 값이 변하거나, 2) modifier 요소가 변경되면 계산
        /// </summary>
        public void CalculateValue()
        {
            // ======================================================================
            // # Base 값 구하기 #
            int calcBase = CalculateBaseInternal();

            // ======================================================================
            // # Modifier 계산 #
            CalculateModifiers(calcBase);

            // ======================================================================
            // # current 비율 적용 #
            AfterValueCalculated();
            
        }

        /// <summary>
        /// abstract로 할지 고민됬었는데 그냥 필요한쪽이 구현하는거로 결정
        /// </summary>
        /// <returns></returns>
        protected virtual int CalculateBaseInternal()
        {
            return baseValue;
        }

        void CalculateModifiers(int calcBase)
        {
            float flat = 0;
            float percent = 0f;             // 합연산
            float finalMultiplier = 1f;     // 곱연산

            foreach (var m in m_Modifiers)
            {
                (flat, percent, finalMultiplier) = m.operationType switch
                {
                    ModifierOperationType.Flat              => (flat + m.magnitude, percent, finalMultiplier),
                    ModifierOperationType.Percent           => (flat, percent + m.magnitude * 0.01f, finalMultiplier),
                    ModifierOperationType.finalMultiplier   => (flat, percent, finalMultiplier * (1f + m.magnitude * 0.01f)),
                    _ => throw new NotSupportedException()
                };
            }

            // 플렛 적용
            int result = Mathf.FloorToInt(calcBase + flat);

            // 퍼센트 적용
            result = Mathf.FloorToInt(result * (1f + percent));

            // 최종 배율 적용
            m_FinalValue = Mathf.FloorToInt(result * finalMultiplier);
        }

        /// <summary>
        /// CurrentValue 계산이 필요한 경우에 구현
        /// </summary>
        protected virtual void AfterValueCalculated() { }


        // `modifier 추가 제거를 어떤식으로 구현되는가` 이건 과제임

        public void AddModifier(StatModifier modifier)
        {
            m_Modifiers.Add(modifier);
            modifier.onExpired += HandleModifierExpired;

            CalculateValue();
            onValueChanged?.Invoke();
        }

        public void RemoveModifierFromSource(object source)
        {
            if (source == null) return;

            int prevCount = m_Modifiers.Count;

            m_Modifiers.RemoveAll(m => m.source == source);
            
            if (m_Modifiers.Count != prevCount)
            {
                CalculateValue();
                onValueChanged?.Invoke();
            }
        }
        public void RemoveAllModifiers()
        {
            if (m_Modifiers.Count > 0)
            {
                m_Modifiers.Clear();

                CalculateValue();
                onValueChanged?.Invoke();
            }
        }
        void HandleModifierExpired(StatModifier modifier)
        {
            modifier.onExpired -= HandleModifierExpired;
            m_Modifiers.Remove(modifier);

            CalculateValue();
            onValueChanged?.Invoke();

        }
    }
}
