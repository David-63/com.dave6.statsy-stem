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
            IStatController entity = target as IStatController;
            var stat = entity.statHandler.GetHealthStat();
            
            entity.statHandler.CreateEffectInstance(effectDefinition, stat);

            Destroy(gameObject);
        }
    }
}