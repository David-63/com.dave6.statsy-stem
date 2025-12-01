using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
    [CreateAssetMenu(fileName = "StatDataBase", menuName = "DaveAssets/StatSystem/StatDatabase")]
    public class StatDatabase : ScriptableObject
    {
        public List<StatDefinition> stats = new List<StatDefinition>();
        public List<StatDefinition> primaryStats = new List<StatDefinition>();
        public List<StatDefinition> attributes = new List<StatDefinition>();
    }
}