using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
    [CreateAssetMenu(fileName = "StatDataBase", menuName = "DaveAssets/StatSystem/StatDatabase")]
    public class StatDatabase : ScriptableObject
    {
        public List<StatDefinition> stats;
        public List<StatDefinition> primaryStats;
        public List<StatDefinition> attributes;
    }
}