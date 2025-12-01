using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("StatSystem.Tests")]
namespace StatSystem
{
    public class PrimaryStat : Stat
    {
        int m_BaseValue;
        public override int baseValue { get => m_BaseValue; protected set => m_BaseValue = value; }

        public PrimaryStat(StatDefinition definition) : base(definition)
        {
            baseValue = definition.baseValue;
        }

        internal void Add(int amount)
        {
            baseValue += amount;
            CalculateValue();
        }
        internal void Substract(int amount)
        {
            baseValue -= amount;
            CalculateValue();
        }
    }
}