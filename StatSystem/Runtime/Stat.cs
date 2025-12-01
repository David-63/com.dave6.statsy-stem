using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StatSystem
{
    public class Stat
    {
        protected StatDefinition m_Definition;
        public StatDefinition definition => m_Definition;
        protected int m_Value;                                                          // <- 이 값으로 테스트함
        protected List<StatModifier> m_Modifiers = new();


        public int value { get => m_Value; private set => m_Value = value; }            // <- 이 값으로 테스트함
        public virtual int baseValue { get => m_Definition.baseValue; protected set {} }
        public event System.Action valueChanged;                 // 별 일 없으면 UnityAction으로 변경해도 될듯

        public Stat(StatDefinition definition)
        {
            m_Definition = definition;
        }

        public void Initialize()
        {
            CalculateValue();
        }

        public void AddModifier(StatModifier modifier)
        {
            m_Modifiers.Add(modifier);
            CalculateValue();
        }
        public void RemoveModifierFromSource(Object source)
        {
            m_Modifiers = m_Modifiers.Where(m => m.Source.GetInstanceID() != source.GetInstanceID()).ToList();
            CalculateValue();
        }

        internal void CalculateValue()
        {
            int nextValue = baseValue;

            if (m_Definition.formula != null && m_Definition.formula.rootNode != null)
            {
                nextValue += Mathf.RoundToInt(m_Definition.formula.rootNode.value);
            }

            m_Modifiers.Sort((x, y) => x.Type.CompareTo(y.Type));

            // base에 각 모디파이어 요소 적용
            foreach (var modifier in m_Modifiers)
            {
                if (modifier.Type == ModifierOperationType.Additive)
                {
                    nextValue += modifier.Magnitude;
                }
                else if (modifier.Type == ModifierOperationType.Multiplicative)
                {
                    nextValue *= modifier.Magnitude;
                }
            }

            // 최대 값 제한
            if (m_Definition.cap >= 0)
            {
                nextValue = Mathf.Min(nextValue, m_Definition.cap);
            }

            // 계산 내용 반영
            if (value != nextValue)
            {
                value = nextValue;
                valueChanged?.Invoke();
            }
        }
    }
}