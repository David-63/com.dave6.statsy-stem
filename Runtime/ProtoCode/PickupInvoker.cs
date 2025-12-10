using Dave6.StatSystem;
using Dave6.StatSystem.Effect;
using Dave6.StatSystem.Interaction;
using UnityEngine;

namespace ProtoCode
{
    public class PickupInvoker : MonoBehaviour, IStatInvoker
    {
        [SerializeField] EffectDefinition m_EffectDefinition;
        public EffectDefinition effectDefinition => m_EffectDefinition;

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IStatReceiver>(out var entity))
            {
                entity.Accept(this);
            }
        }

        public void Invoke<T>(T target) where T : Component, IStatReceiver
        {
            IEntity entity = target as IEntity;
            var stat = entity.statHandler.GetHealthStat();
            Debug.Log($"Before Effect -> Health: {stat.currentValue}/{stat.finalValue}");


            entity.statHandler.ApplyEffect(effectDefinition, stat);
            Debug.Log($"After Effect -> Health: {stat.currentValue}/{stat.finalValue}");

            Destroy(gameObject);
        }
    }
}