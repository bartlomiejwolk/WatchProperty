// Copyright (c) 2015 Bartłomiej Wołk (bartlomiejwolk@gmail.com)
//  
// This file is part of the WatchProperty extension for Unity.
// Licensed under the MIT license. See LICENSE file in the project root folder.

using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace WatchProperty {

    [CustomEditor(typeof (WatchProperty))]
    public class WatchPropertyEditor : Editor {

        /* Serialized properties */
        private SerializedProperty _sourceCo;
        private SerializedProperty _targetCo;
        private SerializedProperty _trigger;
        private SerializedProperty _action;
        private SerializedProperty _conditionValue;

        /* Component properties */
        private PropertyInfo[] _sourceProperties;
        private PropertyInfo[] _targetProperties;

        private void OnEnable() {
            _sourceCo = serializedObject.FindProperty("_sourceCo");
            _targetCo = serializedObject.FindProperty("_targetCo");
            _trigger = serializedObject.FindProperty("_trigger");
            _action = serializedObject.FindProperty("_action");
            _conditionValue = serializedObject.FindProperty("_conditionValue");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            WatchProperty script = (WatchProperty) target;

            // Display fields for game object.
            EditorGUILayout.PropertyField(_sourceCo);

            // Component properties by name.
            string[] sourcePropNames;
            // Find component properties in a selected component.
            if (script.SourceCo) {
                // Get all properties from source game object.
                _sourceProperties = script.SourceCo.GetType().GetProperties();
                // Initialize array.
                sourcePropNames = new string[_sourceProperties.Length];
                // Fill array with property names.
                for (int i = 0; i < _sourceProperties.Length; i++) {
                    sourcePropNames[i] = _sourceProperties[i].Name;
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
            EditorGUILayout.PropertyField(_trigger);
            EditorGUIUtility.labelWidth = 100;
            // Display textbox to enter value required by the trigger option.
            EditorGUILayout.PropertyField(_conditionValue);
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.EndHorizontal();

            // Display action dropdown field.
            EditorGUILayout.PropertyField(_action);

            // Action dropdown
            switch (_action.enumValueIndex) {
                // Action "Enable".
                case (int) Action.Enable:
                    // Display fields for target object.
                    EditorGUILayout.PropertyField(_targetCo);
                    break;
                // Action "Disable".
                case (int) Action.Disable:
                    break;
                // Action "Set".
                case (int) Action.Set:
                    // Display field for target game object.
                    EditorGUILayout.PropertyField(_targetCo);

                    // TODO Create method with args.
                    // Component properties by name.
                    string[] targetPropNames;
                    // Find component properties.
                    if (script.TargetCo) {
                        _targetProperties = script.TargetCo.GetType()
                            .GetProperties(
                                BindingFlags.Public |
                                BindingFlags.Instance);
                        //BindingFlags.NonPublic);
                        // Initialize array.
                        targetPropNames = new string[_targetProperties.Length];
                        for (int i = 0; i < _targetProperties.Length; i++) {
                            targetPropNames[i] = _targetProperties[i].Name;
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

    }

}
