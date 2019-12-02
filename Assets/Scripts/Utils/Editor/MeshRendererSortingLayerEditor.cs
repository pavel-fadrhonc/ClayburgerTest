using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Utils.Editor
{
    [CustomEditor(typeof(MeshRendererSortingLayer))]
    public class MeshRendererSortingLayerEditor : UnityEditor.Editor
    {
        MeshRendererSortingLayer script;

        void Awake()
        {
            script = (MeshRendererSortingLayer) target;
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            //Sprite sorting
            GUILayout.Space(10);
            //Get sorting layers
            int[] layerIDs = GetSortingLayerUniqueIDs();
            string[] layerNames = GetSortingLayerNames();
            //Get selected sorting layer
            int selected = -1;
            for (int i = 0; i < layerIDs.Length; i++)
            {
                if (layerIDs[i] == script.sortingLayer)
                {
                    selected = i;
                }
            }

            //Select Default layer if no other is selected
            if (selected == -1)
            {
                for (int i = 0; i < layerIDs.Length; i++)
                {
                    if (layerIDs[i] == 0)
                    {
                        selected = i;
                    }
                }
            }

            //Sorting layer dropdown
            EditorGUI.BeginChangeCheck();
            GUIContent[] dropdown = new GUIContent[layerNames.Length + 2];
            for (int i = 0; i < layerNames.Length; i++)
            {
                dropdown[i] = new GUIContent(layerNames[i]);
            }

            dropdown[layerNames.Length] = new GUIContent();
            dropdown[layerNames.Length + 1] = new GUIContent("Add Sorting Layer...");
            selected = EditorGUILayout.Popup(new GUIContent("Sorting Layer", "Name of the Renderer's sorting layer"),
                selected, dropdown);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(script, "Change sorting layer");
                if (selected == layerNames.Length + 1)
                {
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags and Layers");
                }
                else
                {
                    script.sortingLayer = layerIDs[selected];
                }

                EditorUtility.SetDirty(script);
            }

            //Order in layer field
            EditorGUI.BeginChangeCheck();
            int order = EditorGUILayout.IntField(
                new GUIContent("Order in Layer", "Renderer's order within a sorting layer"), script.orderInLayer);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(script, "Change order in layer");
                script.orderInLayer = order;
                EditorUtility.SetDirty(script);
            }
        }

        //Get the sorting layer IDs
        public int[] GetSortingLayerUniqueIDs()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs",
                BindingFlags.Static | BindingFlags.NonPublic);
            return (int[]) sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
        }

        //Get the sorting layer names
        public string[] GetSortingLayerNames()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty =
                internalEditorUtilityType.GetProperty("sortingLayerNames",
                    BindingFlags.Static | BindingFlags.NonPublic);
            return (string[]) sortingLayersProperty.GetValue(null, new object[0]);
        }
    }
}