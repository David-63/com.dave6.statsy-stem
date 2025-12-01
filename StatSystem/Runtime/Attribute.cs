using UnityEngine;

namespace StatSystem
{
    public class Attribute : Stat
    {
        protected int m_CurrentValue;
        public int currentValue { get => m_CurrentValue; private set => m_CurrentValue = value; }
        public event System.Action CurrentValueChanged;
        public event System.Action<StatModifier> AppliedModifier;
        
        public Attribute(StatDefinition definition) : base(definition)
        {
            currentValue = baseValue;
        }

        public virtual void ApplyModifier(StatModifier modifier)
        {
            int nextValue = currentValue;

            switch (modifier.Type)
            {
                case ModifierOperationType.Additive:
                nextValue += modifier.Magnitude;
                break;

                case ModifierOperationType.Multiplicative:
                nextValue *= modifier.Magnitude;
                break;
                case ModifierOperationType.Override:
                nextValue = modifier.Magnitude;
                break;
            }
            nextValue = Mathf.Clamp(nextValue, 0, value);

            if (currentValue != nextValue)
            {
                currentValue = nextValue;
                CurrentValueChanged?.Invoke();
                AppliedModifier?.Invoke(modifier);
            }
        }
    }
}