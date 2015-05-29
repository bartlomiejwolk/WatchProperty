// Copyright (c) 2015 Bartlomiej Wolk (bartlomiejwolk@gmail.com)
// 
// This file is part of the WatchProperty extension for Unity. Licensed under
// the MIT license. See LICENSE file in the project root folder.

using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace WatchPropertyEx {

    [CustomEditor(typeof (WatchProperty))]
    public class WatchPropertyEditor : Editor {

        #region FIELDS

        private WatchProperty Script { get; set; }

        #endregion

        #region PROPERTIES

        private PropertyInfo[] sourceProperties;
        private PropertyInfo[] targetProperties;

        #endregion PROPERTIES

        #region SERIALIZED PROPERTIES

        private SerializedProperty description;
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

            DrawVersionLabel();
            DrawDescriptionTextArea();

            EditorGUILayout.Space();

            DrawSourceCoField();
            HandleDrawSourcePropertyDropdown();

            EditorGUILayout.BeginHorizontal();

            DrawTriggerDropdown();
            DrawConditionValueField();

            EditorGUILayout.EndHorizontal();

            DrawActionDropdown();
            HandleActionOption();

            serializedObject.ApplyModifiedProperties();
        }
        private void OnEnable() {
            Script = (WatchProperty) target;

            description = serializedObject.FindProperty("description");
            sourceCo = serializedObject.FindProperty("sourceCo");
            targetCo = serializedObject.FindProperty("targetCo");
            trigger = serializedObject.FindProperty("trigger");
            action = serializedObject.FindProperty("action");
            conditionValue = serializedObject.FindProperty("conditionValue");
        }

        #endregion UNITY MESSAGES

        #region INSPECTOR CONTROLS
        private void HandleDrawSourcePropertyDropdown() {
            if (!Script.SourceCo) return;

            // Get all properties from source game object.
            sourceProperties = Script.SourceCo.GetType().GetProperties();

            // Initialize array.
            var sourcePropNames = new string[sourceProperties.Length];

            // Fill array with property names.
            for (var i = 0; i < sourceProperties.Length; i++) {
                sourcePropNames[i] = sourceProperties[i].Name;
            }

            // Display dropdown component property list.
            Script.SourcePropIndex = EditorGUILayout.Popup(
                "Source Property",
                Script.SourcePropIndex,
                sourcePropNames);

            // Save selected property name.
            Script.SourcePropName = sourcePropNames[Script.SourcePropIndex];

        }

        private void DrawTriggerDropdown() {
            EditorGUIUtility.labelWidth = 50;

            EditorGUILayout.PropertyField(trigger);
        }

        private void DrawConditionValueField() {
            EditorGUIUtility.labelWidth = 50;
            EditorGUIUtility.fieldWidth = 20;

            EditorGUILayout.PropertyField(
                conditionValue,
                new GUIContent(
                    "Value",
                    ""));

            EditorGUIUtility.labelWidth = 0;
        }

        // todo extract
        private void HandleActionOption() {
            // Action dropdown
            switch (action.enumValueIndex) {
                // Action "Enable".
                case (int)Action.Enable:
                    // Display fields for target object.
                    EditorGUILayout.PropertyField(targetCo);
                    break;
                // Action "Disable".
                case (int)Action.Disable:
                    break;
                // Action "Set".
                case (int)Action.Set:
                    // Display field for target game object.
                    EditorGUILayout.PropertyField(targetCo);

                    // TODO Create method with args. Component properties by
                    // name.
                    string[] targetPropNames;
                    // Find component properties.
                    if (Script.TargetCo) {
                        targetProperties = Script.TargetCo.GetType()
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
                        Script.TargetPropIndex = EditorGUILayout.Popup(
                            "Target Property",
                            Script.TargetPropIndex,
                            targetPropNames);

                        Script.TargetPropName =
                            targetPropNames[Script.TargetPropIndex];
                    }
                    break;
            }
        }

        private void DrawActionDropdown() {
            EditorGUILayout.PropertyField(action);
        }

        private void DrawSourceCoField() {
            EditorGUILayout.PropertyField(sourceCo);
        }


        private void DrawVersionLabel() {
            EditorGUILayout.LabelField(
                string.Format(
                    "{0} ({1})",
                    WatchProperty.Version,
                    WatchProperty.Extension));
        }

        private void DrawDescriptionTextArea() {
            description.stringValue = EditorGUILayout.TextArea(
                description.stringValue);
        }

        #endregion INSPECTOR

        #region METHODS

        [MenuItem("Component/WatchProperty")]
        private static void AddEntryToComponentMenu() {
            if (Selection.activeGameObject != null) {
                Selection.activeGameObject.AddComponent(typeof(WatchProperty));
            }
        }

        #endregion METHODS
    }

}