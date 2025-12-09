using System.Collections.Generic;
using UnityEngine;

namespace Dave6.StatSystem
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
        float currentValue { get; }
        void ApplyEffect(EffectDefinition effect, float value);
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
