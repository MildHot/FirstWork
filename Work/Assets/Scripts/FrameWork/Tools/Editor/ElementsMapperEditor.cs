using UnityEngine;
using UnityEditor.AnimatedValues;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ElementsMapper), true)]
    public class ElementsMapperEditor : Editor
    {
        private SerializedProperty m_panels;
        private SerializedProperty m_elems;
        private Color defaultColor = Color.black; 

        void OnEnable()
        {
            m_panels = serializedObject.FindProperty("Panels");
            m_elems = serializedObject.FindProperty("Elements");
        }

        public override void OnInspectorGUI()
        {
            defaultColor = EditorStyles.label.normal.textColor;

            serializedObject.Update();

            // panels
            EditorGUILayout.LabelField("Panels");
            EditorGUI.indentLevel++;
            for (int i = 0; i < m_panels.arraySize; i++)
            {
                SerializedProperty item = m_panels.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal();
                bool hasObject = item.objectReferenceValue != null;
                ColorLabelField("Panel", hasObject ? defaultColor : Color.red, GUILayout.Width(70f));
                EditorGUILayout.PropertyField(item, GUIContent.none);
                if (GUILayout.Button("Delete", GUILayout.MaxWidth(60)))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", "You will delete this panel, are you sure?", "yes", "no"))
                    {
                        m_panels.DeleteArrayElementAtIndex(i);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add", GUILayout.MaxWidth(200)))
            {
                m_panels.InsertArrayElementAtIndex(m_panels.arraySize);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();


            // elements
            EditorGUILayout.LabelField("Elements");
            EditorGUI.indentLevel++;
            for (int i = 0; i < m_elems.arraySize; i++)
            {
                SerializedProperty item = m_elems.GetArrayElementAtIndex(i);
                SerializedProperty name = item.FindPropertyRelative("Name");
                SerializedProperty gameObject = item.FindPropertyRelative("GameObject");

                EditorGUILayout.BeginHorizontal();
                bool hasObject = gameObject.objectReferenceValue != null;
                bool diffName = true;
                if (hasObject)
                {
                    string nodeName = (gameObject.objectReferenceValue as GameObject).name;
                    if (name.stringValue.Equals("")) name.stringValue = nodeName;
                    diffName = !name.stringValue.Equals(nodeName);
                }
                ColorLabelField("Element", hasObject ? defaultColor : Color.red, GUILayout.Width(70f));
                EditorGUILayout.PropertyField(gameObject, GUIContent.none); 
                EditorGUILayout.LabelField(diffName ? " *Name" : "  Name", GUILayout.Width(62f));
                name.stringValue = ColorTextField(name.stringValue, diffName ? Color.white : defaultColor, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Delete", GUILayout.MaxWidth(60)))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", "You will delete this element, are you sure?", "yes", "no"))
                    {
                        m_elems.DeleteArrayElementAtIndex(i);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add", GUILayout.MaxWidth(200)))
            {
                m_elems.InsertArrayElementAtIndex(m_elems.arraySize);
                SerializedProperty newitem = m_elems.GetArrayElementAtIndex(m_elems.arraySize - 1);
                newitem.FindPropertyRelative("Name").stringValue = "";
                newitem.FindPropertyRelative("GameObject").objectReferenceValue = null;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }

        void ColorLabelField(string label, Color c, GUILayoutOption op)
        {
            var colorStyle = new GUIStyle(EditorStyles.label);
            colorStyle.normal.textColor = c;
            EditorGUILayout.LabelField(label, colorStyle, op);
        }

        string ColorTextField(string text, Color c, GUILayoutOption op)
        {
            var colorStyle = new GUIStyle(EditorStyles.textField);
            colorStyle.normal.textColor = c;
            colorStyle.focused.textColor = c;
            return EditorGUILayout.TextField(text, colorStyle, op);
        }
    }
}