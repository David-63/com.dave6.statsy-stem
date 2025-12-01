using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatSystem.Editor
{
    public class StatDatabaseEditorWindow : EditorWindow
    {
        [SerializeField]
        VisualTreeAsset m_VisualTreeAsset = default;
        StatDatabase m_Database;
        StatCollectionEditor m_Current;

        public static void ShowWindow()
        {
            StatDatabaseEditorWindow window = GetWindow<StatDatabaseEditorWindow>();
            window.titleContent = new GUIContent("StatDatabase");
            window.minSize = new Vector2(800, 600);
        }
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is StatDatabase)
            {
                ShowWindow();
                return true;
            }
            return false;
        }

        void OnSelectionChange()
        {
            // project 창에서 StatDatabase 선택 시 에디터 창에 반영
            m_Database = Selection.activeObject as StatDatabase;
        }


        public void CreateGUI()
        {
            OnSelectionChange();

            VisualElement root = rootVisualElement;

            VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
            root.Add(labelFromUXML);

            StatCollectionEditor stats = root.Q<StatCollectionEditor>("stats");
            stats.Initialize(m_Database, m_Database.stats);
            Button statsTab = root.Q<Button>("stats-tab");
            statsTab.clicked += () =>
            {
                m_Current.style.display = DisplayStyle.None;
                stats.style.display = DisplayStyle.Flex;
                m_Current = stats;
            };

            StatCollectionEditor primaryStats = root.Q<StatCollectionEditor>("primary-stats");
            primaryStats.Initialize(m_Database, m_Database.primaryStats);
            Button primaryStatsTab = root.Q<Button>("primary-stats-tab");
            primaryStatsTab.clicked += () =>
            {
                m_Current.style.display = DisplayStyle.None;
                primaryStats.style.display = DisplayStyle.Flex;
                m_Current = primaryStats;
            };

            StatCollectionEditor attributes = root.Q<StatCollectionEditor>("attributes");
            attributes.Initialize(m_Database, m_Database.attributes);
            Button attributesTab = root.Q<Button>("attributes-tab");
            attributesTab.clicked += () =>
            {
                m_Current.style.display = DisplayStyle.None;
                attributes.style.display = DisplayStyle.Flex;
                m_Current = attributes;
            };
            m_Current = stats;
        }
    }
}