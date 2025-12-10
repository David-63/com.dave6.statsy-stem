using System.Collections.Generic;

namespace Dave6.StatSystem.Stat
{
    public class SecondaryStat : BaseStat, IDerived
    {
        List<SourcePair> m_SourceStats;        
        public List<SourcePair> sources => m_SourceStats;
        public SecondaryStat(StatDefinition definition) : base(definition) { }

        public void SetupSources(List<SourcePair> sourceStats) => m_SourceStats = sourceStats;

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

    }
}
