using Core;
using UnityEngine;

namespace StatSystem
{
    [CreateAssetMenu(fileName = "StatDefinition", menuName = "DaveAssets/StatSystem/StatDefinition")]
    public class StatDefinition : ScriptableObject
    {
        [SerializeField] int m_BaseValue;
        [SerializeField] int m_Cap = -1;
        [SerializeField] NodeGraph m_Fomula;
        public int baseValue => m_BaseValue;
        public int cap => m_Cap;
        public NodeGraph formula => m_Fomula;
    }
}