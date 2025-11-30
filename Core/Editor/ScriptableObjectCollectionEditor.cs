using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Editor
{
    public class ScriptableObjectCollectionEditor<T> : VisualElement where T: ScriptableObject
    {
        protected ScriptableObject m_Target;
        protected List<T> m_Items;
        List<T> m_FilteredListView;
        InspectorElement m_Inspector;
        ListView m_ListView;
        ToolbarSearchField m_ToolbarSearchField;
        Button m_CreateButton;
        PropertyField m_NameField;

        public ScriptableObjectCollectionEditor()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.dave6.statsystem/Core/Editor/ScriptableObjectCollectionEditor.uxml");
            visualTree.CloneTree(this);

            m_Inspector = this.Q<Inspector>();
            m_ListView = this.Q<ListView>();
            m_ToolbarSearchField = this.Q<ToolbarSearchField>();
            m_CreateButton = this.Q<Button>("create-button");
            m_NameField = this.Q<PropertyField>("name-field");
        }

        public void Initialize(ScriptableObject target, List<T> items)
        {
            Debug.Log("초기화 됨");
            m_Target = target;
            m_Items = items;
            InitializeInternal();
        }
        void InitializeInternal()
        {
            Func<VisualElement> makeItem = () => new Label();
            m_ListView.makeItem = makeItem;
            m_ListView.selectionChanged += objects =>
            {
                T item = objects.First() as T;
                Select(item);
            };
            Action<VisualElement, int> bindItem = (element, i) =>
            {
                Label label = element as Label;
                label.AddManipulator(new ContextualMenuManipulator(evt =>
                {
                    evt.menu.AppendAction("Duplicate", action =>
                    {
                        Duplicate(m_FilteredListView[i]);
                    });
                    evt.menu.AppendAction("Remove", action =>
                    {
                        Remove(m_FilteredListView[i]);
                    });
                }));
                SerializedObject serializedObject = new SerializedObject(m_FilteredListView[i]);
                SerializedProperty serializedProperty = serializedObject.FindProperty("m_Name");
                label.BindProperty(serializedProperty);
            };
            m_ListView.bindItem = bindItem;
            m_ListView.itemsSource = m_FilteredListView = m_Items;

            m_CreateButton.clicked += Create;
            
            m_ToolbarSearchField.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                m_ListView.itemsSource = m_FilteredListView = m_Items
                    .Where(item => item.name.StartsWith(evt.newValue, StringComparison.OrdinalIgnoreCase)).ToList();
                m_ListView.Rebuild();
            });
        }

        void Create()
        {
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(T).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToArray();

            if (types.Length > 1)
            {
                GenericMenu menu = new GenericMenu();
                foreach (Type type in types)
                {
                    menu.AddItem(new GUIContent(ObjectNames.NicifyVariableName(type.Name)), false, delegate
                    {
                        CreateItem(type);
                    });
                }
                menu.ShowAsContext();
            }
            else
            {
                CreateItem(types[0]);
            }
        }
        void CreateItem(Type type)
        {
            T item = (T)ScriptableObject.CreateInstance(type);
            item.name = "New Item";
            item.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(item, m_Target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            m_Items.Add(item);
            m_ListView.Rebuild();
            Select(item);
            m_ListView.SetSelection(m_Items.Count - 1);
            EditorUtility.SetDirty(m_Target);
        }
        void Select(T item)
        {
            SerializedObject serializedObject = new SerializedObject(item);
            m_Inspector.Bind(serializedObject);
            m_NameField.Bind(serializedObject);
        }
        
        void Duplicate(T item)
        {
            T duplicate = ScriptableObject.Instantiate(item);
            duplicate.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(duplicate, m_Target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            m_Items.Add(duplicate);
            m_ListView.Rebuild();
            Select(duplicate);
            m_ListView.SetSelection(m_Items.Count - 1);
            EditorUtility.SetDirty(m_Target);
        }
        void Remove(T item)
        {
            string path = AssetDatabase.GetAssetPath(item);

            if (EditorUtility.DisplayDialog("Remove Item", "Are you sure you want to remove " + item.name + "?", "Yes", "No"))
            {
                if (string.IsNullOrEmpty(path))
                {
                    // Sub-Asset이면 실제 삭제
                    ScriptableObject.DestroyImmediate(item, true);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                // 리스트/뷰에서 제거
                m_Items.Remove(item);
                m_ListView.Rebuild();
                EditorUtility.SetDirty(m_Target);
            }
        }
    }
}