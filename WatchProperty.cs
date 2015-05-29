// Copyright (c) 2015 Bartłomiej Wołk (bartlomiejwolk@gmail.com)
// 
// This file is part of the WatchProperty extension for Unity. Licensed under
// the MIT license. See LICENSE file in the project root folder.

using System;
using System.Reflection;
using UnityEngine;

namespace WatchPropertyEx {
    
    /// Observe component property and act when value is changed.
    // todo move docs to properties
    public class WatchProperty : MonoBehaviour {

        #region CONSTANTS

        public const string Version = "v0.1.0";
        public const string Extension = "WatchProperty";

        #endregion
        
        #region FIELDS

#pragma warning disable 0414
        /// <summary>
        ///     Allows identify component in the scene file when reading it with
        ///     text editor.
        /// </summary>
        [SerializeField]
        private string componentName = "WatchProperty";
#pragma warning restore0414

        #endregion

        #region INSPECTOR FIELDS

        [SerializeField]
        private string description = "Description";
 
        /// Action to be performed on the target.
        [SerializeField]
        private Action action;

        /// Value of the property that acts as a trigger.
        [SerializeField]
        private float conditionValue;

        /// Source component.
        /// 
        /// Component which property will be used to update target game object.
        [SerializeField]
        private Component sourceCo;

        /// Index of the source property in the property array.
        /// 
        /// Property array contains names of all properties found in the source
        /// game object.
        [SerializeField]
        private int sourcePropIndex;

        /// Metadata of the selected source property.
        private PropertyInfo sourcePropInfo;

        /// Name of the selected source game object property.
        [SerializeField]
        private string sourcePropName;

        /// Target component.
        /// 
        /// Component which property will be updated by target game object's
        /// property.
        [SerializeField]
        private Component targetCo;

        /// Index of the target property in the property array.
        /// 
        /// Property array contains names of all properties found in the target
        /// game object.
        [SerializeField]
        private int targetPropIndex;

        /// Name of the selected target game object selected property.
        // TODO This shouldn't be serialized.
        [SerializeField]
        private string targetPropName;

        /// Trigger that causes some change in the target component.
        [SerializeField]
        private Trigger trigger;

        #endregion FIELDS

        #region PROPERTIES

        public Component SourceCo {
            get { return sourceCo; }
            set { sourceCo = value; }
        }

        public int SourcePropIndex {
            get { return sourcePropIndex; }
            set { sourcePropIndex = value; }
        }

        public string SourcePropName {
            get { return sourcePropName; }
            set { sourcePropName = value; }
        }

        public Component TargetCo {
            get { return targetCo; }
            set { targetCo = value; }
        }

        public int TargetPropIndex {
            get { return targetPropIndex; }
            set { targetPropIndex = value; }
        }

        public string TargetPropName {
            get { return targetPropName; }
            set { targetPropName = value; }
        }

        /// Value of the property that acts as a trigger.
        public float ConditionValue {
            get { return conditionValue; }
            set { conditionValue = value; }
        }

        /// Action to be performed on the target.
        public Action Action {
            get { return action; }
            set { action = value; }
        }

        public string Description {
            get { return description; }
            set { description = value; }
        }

        #endregion PROPERTIES

        #region UNITY MESSAGES

        private void Awake() {
            // Initialize class fields.
            sourcePropInfo = sourceCo.GetType().GetProperty(sourcePropName);
        }

        // TODO Add option to update in FixedUpdate().
        private void Update() {
            if (sourcePropInfo == null) return;

            // Value of the source property.
            var sourceType = sourcePropInfo.PropertyType;
            // Type of the source property value.
            var sourceValue = sourcePropInfo.GetValue(sourceCo, null);

            // Handle trigger option.
            switch (trigger) {
                case Trigger.Equal:
                    HandleEqual(sourceValue, sourceType);
                    break;

                case Trigger.EqualOrLess:
                    HandleEqualOrLess(sourceValue, sourceType);
                    break;

                case Trigger.LessThan:
                    HandleLessThan(sourceValue, sourceType);
                    break;

                case Trigger.MoreThan:
                    HandleMoreThan(sourceValue, sourceType);
                    break;
            }
        }

        #endregion UNITY MESSAGES

        #region METHODS

        private void HandleEnableAction() {
            targetCo.gameObject.SetActive(true);
        }

        /// Handle 'Equal' option.
        private void HandleEqual(object sourceValue, Type sourceType) {
            // Check source type and then compare with the _conditionValue.
            switch (sourceType.ToString()) {
                case "System.Int32":
                    // Keep source value as integer.
                    var intValue = (int) sourceValue;
                    if (intValue == (int) ConditionValue) {
                        HandleEnableAction();
                    }
                    break;

                case "System.Single":
                    var floatValue = (float) sourceValue;
                    if (floatValue == ConditionValue) {
                        HandleEnableAction();
                    }
                    break;
            }
        }

        /// Handle 'EqualOrLess' option.
        private void HandleEqualOrLess(object sourceValue, Type sourceType) {
            // Check source type and then compare with the _conditionValue.
            switch (sourceType.ToString()) {
                case "System.Int32":
                    // Keep source value as integer.
                    int intValue;
                    intValue = (int) sourceValue;
                    if (intValue <= (int) ConditionValue) {
                        HandleEnableAction();
                    }
                    break;

                case "System.Single":
                    float floatValue;
                    floatValue = (float) sourceValue;
                    if (floatValue <= ConditionValue) {
                        HandleEnableAction();
                    }
                    break;
            }
        }

        /// Handle 'LessThan' option.
        private void HandleLessThan(object sourceValue, Type sourceType) {
            // Check source type and then compare with the _conditionValue.
            switch (sourceType.ToString()) {
                case "System.Int32":
                    // Keep source value as integer.
                    int intValue;
                    intValue = (int) sourceValue;
                    if (intValue < (int) ConditionValue) {
                        HandleEnableAction();
                    }
                    break;

                case "System.Single":
                    float floatValue;
                    floatValue = (float) sourceValue;
                    if (floatValue < ConditionValue) {
                        HandleEnableAction();
                    }
                    break;
            }
        }

        /// Handle 'MoreThan' option.
        private void HandleMoreThan(object sourceValue, Type sourceType) {
            // Check source type and then compare with the _conditionValue.
            switch (sourceType.ToString()) {
                case "System.Int32":
                    // Keep source value as integer.
                    int intValue;
                    intValue = (int) sourceValue;
                    if (intValue > (int) ConditionValue) {
                        HandleEnableAction();
                    }
                    break;

                case "System.Single":
                    float floatValue;
                    floatValue = (float) sourceValue;
                    if (floatValue > ConditionValue) {
                        HandleEnableAction();
                    }
                    break;
            }
        }

        #endregion METHODS
    }

}