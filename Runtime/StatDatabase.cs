using System.Collections.Generic;
using UnityEngine;

namespace Dave6.StatSystem
{
    /*
        스텟 리스트는 계층적으로 존재해야함
        
    */
    [CreateAssetMenu(fileName = "StatDatabase", menuName = "DaveAssets/StatSystem/StatDatabase")]
    public class StatDatabase : ScriptableObject
    {
        [SerializeField] List<StatDefinition> m_Attributes = new();
        [SerializeField] List<StatDefinition> m_SecondaryStat = new();
        [SerializeField] List<StatDefinition> m_ResourceStat = new();
        public List<StatDefinition> attributes => m_Attributes;
        public List<StatDefinition> secondaryStat => m_SecondaryStat;
        public List<StatDefinition> resourceStat => m_ResourceStat;
    }
}
