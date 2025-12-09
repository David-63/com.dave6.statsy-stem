using System.Collections.Generic;

namespace Dave6.StatSystem
{
    public class EffectInstance
    {
        public EffectDefinition definition { get; private set; }
        public ResourceStat targetStat { get; private set; }

        List<SourcePair> m_Sources;
        public List<SourcePair> sources => m_Sources;
        float m_TotalValue;

        /// <summary>
        /// 1)Effect 정의  2)대상 스텟
        /// </summary>
        public EffectInstance(EffectDefinition definition, ResourceStat target, List<SourcePair> sources)
        {
            this.definition = definition;
            targetStat = target;
            m_Sources = sources;
            InitializeEffectValue();
        }

        public void InitializeEffectValue()
        {
            float totalWeight = 0;

            // total값에 각 sourceStat.finalValue * weight 더하기
            foreach (var pair in m_Sources)
            {
                totalWeight += pair.stat.finalValue * pair.weight;
            }
            // 최종 base값 반환
            m_TotalValue = totalWeight;
        }

        public virtual void Apply()
        {
            targetStat.ApplyEffect(definition, m_TotalValue);
        }
    }
}
