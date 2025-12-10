using Dave6.StatSystem.Effect;
using UnityEngine;

namespace Dave6.StatSystem.Interaction
{
    /// <summary>
    /// Projectile, HitCollider, Pickup
    /// Definition-target-Actor(선택)
    /// </summary>
    public interface IStatInvoker
    {
        EffectDefinition effectDefinition { get; }

        void Invoke<T>(T target) where T : Component, IStatReceiver;
    }
}
