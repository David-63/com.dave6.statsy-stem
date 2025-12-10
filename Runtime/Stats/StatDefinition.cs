using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dave6.StatSystem.Stat
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]    
    public struct StatFormulaSource
    {
        public StatDefinition key;  // 어떤 스탯에 의존?
        public float value;         // 얼마나 영향을 주는지
    }
    [CreateAssetMenu(fileName = "StatDefinition", menuName = "DaveAssets/StatSystem/Definition")]
    public class StatDefinition : ScriptableObject
    {
        [SerializeField] int m_InitialValue;
        public int initialValue => m_InitialValue;

        // 이건 그냥 타입과 강도만 반영
        [SerializeField] List<StatFormulaSource> m_FormulaStats;
        public List<StatFormulaSource> formulaStats => m_FormulaStats;
    }
}
