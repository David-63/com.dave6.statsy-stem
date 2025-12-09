using System.Collections.Generic;
using Dave6.StatSystem;
using Unity.Collections;
using UnityEngine;

namespace ProtoCode
{
    public class StatController : MonoBehaviour, IEntity
    {
        [SerializeField] StatDatabase m_StatDatabase;
        public StatDatabase statDatabase => m_StatDatabase;

        StatHandler m_StatHandler;
        public StatHandler statHandler => m_StatHandler;


        [SerializeField] EffectDefinition testEffect;


        void Awake()
        {
            Debug.Log($"스텟 초기화 진행");
            InitializeStat();
        }

        public void InitializeStat()
        {
            m_StatHandler = new StatHandler(m_StatDatabase);
            m_StatHandler.InitializeStat();

            // 스텟 테스트
            foreach (var stat in m_StatHandler.stats)
            {
                Debug.Log($"{stat.Key}");
            }

            // BaseStat targetStat = m_StatHandler.GetStat("A_Strength");
            // Debug.Log($"{targetStat} BaseValue: {targetStat.baseValue}");
            // Debug.Log($"{targetStat} FinalValue: {targetStat.finalValue}");

            // var derivedStat = m_StatHandler.GetStat("S_Attack") as SecondaryStat;
            // Debug.Log($"{derivedStat} BaseValue: {derivedStat.baseValue}");
            // Debug.Log($"{derivedStat} FinalValue: {derivedStat.finalValue}");
            // derivedStat.AddModifier(new StatModifier(this, ModifierOperationType.Flat, 5));
            // Debug.Log($"{derivedStat} FinalValue: {derivedStat.finalValue}");

            // // 스텟에 직접 때려박는 방식
            // {
            //     var healthStat = m_StatHandler.GetStat("R_Health") as ResourceStat;
            //     Debug.Log($"Before Effect -> Health: {healthStat.currentValue}/{healthStat.finalValue}");

            //     List<SourcePair> sources = new();
            //     foreach (var tuple in testEffect.sourceStats)
            //     {
            //         var sourceStat = m_StatHandler.GetStat(tuple.key.name);
            //         if (sourceStat != null)
            //         {
            //             sources.Add(new SourcePair(sourceStat, tuple.value));
            //             Debug.Log($"Effect Source: {sourceStat.definition.name}, Weight: {tuple.value}");
            //         }
            //     }

            //     var hitEffect = new EffectInstance(testEffect, healthStat, sources);
            //     hitEffect.Apply();
            //     Debug.Log($"After Effect -> Health: {healthStat.currentValue}/{healthStat.finalValue}");
            // }

            {
                var healthStat = m_StatHandler.GetStat("R_Health") as ResourceStat;
                Debug.Log($"Before Effect -> Health: {healthStat.currentValue}/{healthStat.finalValue}");
                m_StatHandler.ApplyEffect(testEffect, healthStat);
                Debug.Log($"After Effect -> Health: {healthStat.currentValue}/{healthStat.finalValue}");
            }

        }
    }
}