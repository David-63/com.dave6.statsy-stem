using System.Collections.Generic;
using Dave6.StatSystem.Effect;
using Dave6.StatSystem.Stat;

namespace Dave6.StatSystem.Stat
{
    public interface IDerived
    {
        List<SourcePair> sources { get; }

        /// <summary>
        /// Definition 에 담긴 formulaStat의 타입과 일치하는 인스턴스 스텟 리스트를 연결
        /// </summary>
        void SetupSources(List<SourcePair> sources);
    }

    public interface IEffectApplicable
    {
        void ApplyCurrentValue(EffectDefinition effect, float value);
        void ApplyCurrentPercent(EffectDefinition effect, float value);
        void ApplyMaxPercent(EffectDefinition effect, float value);
    }

    public struct SourcePair
    {
        public readonly BaseStat stat;
        public readonly float weight;

        public SourcePair(BaseStat stat, float weight)
        {
            this.stat = stat;
            this.weight = weight;
        }
    }

    /// <summary>
    /// 참조할 스텟의 key와 weight 정보가 담겨있음
    /// </summary>
    public class EffectPreset
    {
        readonly List<SourcePair> m_Sources;
        public IReadOnlyList<SourcePair> sources => m_Sources;

        public EffectPreset(IEnumerable<SourcePair> sources)
        {
            m_Sources = new List<SourcePair>(sources);
        }
    }

}
