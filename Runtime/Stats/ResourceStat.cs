using System;
using System.Collections.Generic;
using Dave6.StatSystem.Effect;
using UnityEngine;

namespace Dave6.StatSystem.Stat
{
    public class ResourceStat : BaseStat, IDerived, IEffectApplicable
    {
        bool _initialFinish = false;
        float m_PreviousFinalValue;
        float m_CurrentValue;
        public float currentValue => Mathf.Clamp(m_CurrentValue, 0f, finalValue);

        

        List<SourcePair> m_SourceStats;        
        public List<SourcePair> sources => m_SourceStats;

        public ResourceStat(StatDefinition definition) : base(definition) { }
        public void SetupSources(List<SourcePair> sourceStats) => m_SourceStats = sourceStats;

        public override void Initialize()
        {
            CalculateValue();
            m_PreviousFinalValue = m_FinalValue;
            m_CurrentValue = m_FinalValue;
            _initialFinish = true;
        }

        protected override int CalculateBaseInternal()
        {
            int totalWeight = 0;

            // total값에 각 sourceStat.finalValue * weight 더하기
            foreach (var pair in m_SourceStats)
            {
                totalWeight += (int)(pair.stat.finalValue * pair.weight);
            }
            // 최종 base값 반환
            return baseValue + totalWeight;
        }

        protected override void AfterValueCalculated()
        {
            if (!_initialFinish) return;

            // 최대치가 증가한 경우에만 값 보정
            if (m_PreviousFinalValue < finalValue)
            {
                float ratio = currentValue / m_PreviousFinalValue;
                m_CurrentValue = finalValue * ratio;
            }
            m_PreviousFinalValue = finalValue;
        }


        public void ApplyEffect(EffectDefinition effect, float value)
        {
            switch (effect.operationType)
            {
                case EffectOperationType.Addition:
                    // 현재값 += value
                    m_CurrentValue += value;
                    break;

                case EffectOperationType.Subtraction:
                    // 현재값 -= value
                    m_CurrentValue -= value;
                    break;

                case EffectOperationType.PercentCurrentIncrease:
                    // 현재값 *= (1 + 퍼센트)
                    m_CurrentValue *= (1f + value);
                    break;

                case EffectOperationType.PercentCurrentDecrease:
                    // 현재값 *= (1 - 퍼센트)
                    m_CurrentValue *= Mathf.Max(0f, 1f - value);
                    break;

                case EffectOperationType.PercentMaxIncrease:
                    // 최대값 증가 → 현재값도 비례 증가시키려면 ratio 조정 필요
                    {
                        float oldMax = finalValue;
                        float newMax = oldMax * (1f + value);

                        // 최대치 영향 주는 것이므로 finalValue 재계산이 필요한 구조면
                        // 여기선 임시로 current를 비례 증가시키는 형태 사용
                        float ratio = m_CurrentValue / oldMax;
                        m_CurrentValue = newMax * ratio;
                    }
                    break;

                case EffectOperationType.PercentMaxDecrease:
                    {
                        float oldMax = finalValue;
                        float newMax = oldMax * Mathf.Max(0f, 1f - value);

                        float ratio = m_CurrentValue / oldMax;
                        m_CurrentValue = newMax * ratio;
                    }
                    break;
            }

        }
    }
}
