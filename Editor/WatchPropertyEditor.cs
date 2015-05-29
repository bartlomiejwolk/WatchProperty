// Copyright (c) 2015 Bartłomiej Wołk (bartlomiejwolk@gmail.com)
// 
// This file is part of the WatchProperty extension for Unity. Licensed under
// the MIT license. See LICENSE file in the project root folder.

using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace WatchProperty {

    [CustomEditor(typeof (WatchProperty))]
    public class WatchPropertyEditor : Editor {
        #region PROPERTIES

        private PropertyInfo[] sourceProperties;
        private PropertyInfo[] targetProperties;

        #endregion PROPERTIES

        #region SERIALIZED PROPERTIES

        private SerializedProperty action;
        private SerializedProperty conditionValue;
        private SerializedProperty sourceCo;
        private SerializedProperty targetCo;
        private SerializedProperty trigger;

        #endregion SERIALIZED PROPERTIES

        #region UNITY MESSAGES

        // todo extract methods
        public override void OnInspectorGUI() {
            serializedObject.Update();
            var script = (WatchProperty) target;

            // Display fields for game object.
            EditorGUILayout.PropertyField(sourceCo);

            // Component properties by name.
            string[] sourcePropNames;
            // Find component properties in a selected component.
            if (script.SourceCo) {
                // Get all properties from source game object.
                sourceProperties = script.SourceCo.GetType().GetProperties();
                // Initialize array.
                sourcePropNames = new string[sourceProperties.Length];
                // Fill array with property names.
                for (var i = 0; i < sourceProperties.Length; i++) {
                    sourcePropNames[i] = sourceProperties[i].Name;
                }
                // Display dropdown component property list.
                script.SourcePropIndex = EditorGUILayout.Popup(
                    "Source Property",
                    script.SourcePropIndex,
                    sourcePropNames);

                // Save selected property name.
                script.SourcePropName = sourcePropNames[script.SourcePropIndex];
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 50;
            // Display trigger dropdown field.
            EditorGUILayout.PropertyField(trigger);
            EditorGUIUtility.labelWidth = 100;
            // Display textbox to enter value required by the trigger option.
            EditorGUILayout.PropertyField(conditionValue);
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.EndHorizontal();

            // Display action dropdown field.
            EditorGUILayout.PropertyField(action);

            // Action dropdown
            switch (action.enumValueIndex) {
                // Action "Enable".
                case (int) Action.Enable:
                    // Display fields for target object.
                    EditorGUILayout.PropertyField(targetCo);
                    break;
                // Action "Disable".
                case (int) Action.Disable:
                    break;
                // Action "Set".
                case (int) Action.Set:
                    // Display field for target game object.
                    EditorGUILayout.PropertyField(targetCo);

                    // TODO Create method with args. Component properties by
                    // name.
                    string[] targetPropNames;
                    // Find component properties.
                    if (script.TargetCo) {
                        targetProperties = script.TargetCo.GetType()
                            .GetProperties(
                                BindingFlags.Public |
                                BindingFlags.Instance);
                        //BindingFlags.NonPublic);
                        // Initialize array.
                        targetPropNames = new string[targetProperties.Length];
                        for (var i = 0; i < targetProperties.Length; i++) {
                            targetPropNames[i] = targetProperties[i].Name;
                        }
                        // Display dropdown component property list.
                        script.TargetPropIndex = EditorGUILayout.Popup(
                            "Target Property",
                            script.TargetPropIndex,
                            targetPropNames);

                        script.TargetPropName =
                            targetPropNames[script.TargetPropIndex];
                    }
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            // Save changes
            if (GUI.changed) {
                EditorUtility.SetDirty(script);
            }
        }

        private void OnEnable() {
            sourceCo = serializedObject.FindProperty("sourceCo");
            targetCo = serializedObject.FindProperty("targetCo");
            trigger = serializedObject.FindProperty("trigger");
            action = serializedObject.FindProperty("action");
            conditionValue = serializedObject.FindProperty("conditionValue");
        }

        #endregion UNITY MESSAGES
    }

}