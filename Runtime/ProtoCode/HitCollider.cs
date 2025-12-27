using Dave6.StatSystem;
using Dave6.StatSystem.Effect;
using Dave6.StatSystem.Interaction;
using UnityEngine;

namespace ProtoCode
{
    public class HitCollider : MonoBehaviour, IStatInvoker
    {
        [SerializeField] EffectDefinition m_EffectDefinition;
        public EffectDefinition effectDefinition => m_EffectDefinition;

        // 원래는 플레이어가 생성하면서 초기화 함수로 본인을 넣어줘야함
        [SerializeField] IStatController m_Actor;
        public IStatController actor => m_Actor;

        /// <summary>
        /// 생성될 때, 스텟을 받도록 할수도 있음
        /// </summary>
        public void Initialize(IStatController actorEntity)
        {
            m_Actor = actorEntity;
        }

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

            //m_Actor.statHandler.ApplyInstantEffect(effectDefinition, stat);
            m_Actor.statHandler.CreateEffectInstance(effectDefinition, stat);
            Debug.Log($"target Helth: {stat.currentValue}/{stat.finalValue}");

            if (stat.currentValue <= 0)
            {
                Destroy(target.gameObject);
            }
        }
    }
}