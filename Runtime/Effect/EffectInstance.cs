using System;
using Dave6.StatSystem.Stat;
using UnityEngine;
using UnityUtils.Timer;

namespace Dave6.StatSystem.Effect
{
    /// <summary>
    /// 얼마를 적용할지 계산, 어디에 적용할지 정함
    /// </summary>
    public class EffectInstance : IDisposable
    {
        public EffectDefinition definition { get; private set; }
        public BaseStat targetStat { get; private set; }
        public EffectPreset effectPresets { get; private set; }

        BaseContribution m_Contribution; // nullable
        float m_TotalValue;

        Timer m_TimeRemaining;
        float dotDelay = 0.1f;
        float currentTime = 0;

        public bool disposed { get; private set; }

        /// <summary>
        /// 1)Effect 정의  2)대상 스텟  3)빠른참조¿
        /// </summary>
        public EffectInstance(EffectDefinition definition, BaseStat target, EffectPreset sources)
        {
            this.definition = definition;
            targetStat = target;
            effectPresets = sources;
            InitializeEffectValue();
            if (definition.duration != -1)
            {
                m_TimeRemaining = new Countdown(definition.duration);
                m_TimeRemaining.OnTimerStop += Dispose;
                m_TimeRemaining.Start();
            }
        }

        public void InitializeEffectValue()
        {
            float totalWeight = 0;

            // total값에 각 sourceStat.finalValue * weight 더하기
            foreach (var pair in effectPresets.sources)
            {
                totalWeight += pair.stat.finalValue * pair.weight;
            }
            // 최종 base값 반환
            m_TotalValue = totalWeight + definition.flatValue;
        }

        public void OnUpdate()
        {
            if (EffectApplyMode.Periodic != definition.applyMode) return;
            currentTime += Time.deltaTime;

            if (currentTime >= dotDelay)
            {
                ApplyInstant();
                currentTime = 0;
            }
        }

        public virtual void ApplyInstant()
        {
            if (targetStat is IEffectApplicable applicable)
            {
                switch (definition.instant.operationType)
                {
                    case EffectOperationType.Current:
                    applicable.ApplyCurrentValue(definition, m_TotalValue);
                    break;
                    case EffectOperationType.CurrentPercent:
                    applicable.ApplyCurrentPercent(definition, m_TotalValue);
                    break;
                    case EffectOperationType.MaxPercent:
                    applicable.ApplyMaxPercent(definition, m_TotalValue);
                    break;
                }
            }
        }
        public virtual void ApplySustained()
        {
            m_Contribution = new BaseContribution(definition.sustained.valueType, m_TotalValue);
            targetStat.AddBaseContribution(m_Contribution);
        }

        public void Dispose()
        {
            disposed = true;
        }

        public void Cleanup()
        {
            if (m_Contribution != null)
            {
                targetStat.RemoveBaseContribution(m_Contribution);
                m_Contribution = null;
            }
        }
    }
}
