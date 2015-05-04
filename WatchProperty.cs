// Copyright (c) 2015 Bartłomiej Wołk (bartlomiejwolk@gmail.com)
// 
// This file is part of the WatchProperty extension for Unity. Licensed under
// the MIT license. See LICENSE file in the project root folder.

using System;
using System.Reflection;
using UnityEngine;

namespace WatchProperty {

    /// Observe component property and act when value is changed.
    public class WatchProperty : MonoBehaviour {
        #region FIELDS

        /// Action to be performed on the target.
        [SerializeField]
        private Action _action;

        /// Value of the property that acts as a trigger.
        [SerializeField]
        private float _conditionValue;

        /// Source component.
        /// 
        /// Component which property will be used to update target game object.
        [SerializeField]
        private Component _sourceCo;

        /// Index of the source property in the property array.
        /// 
        /// Property array contains names of all properties found in the source
        /// game object.
        [SerializeField]
        private int _sourcePropIndex;

        /// Metadata of the selected source property.
        private PropertyInfo _sourcePropInfo;

        /// Name of the selected source game object property.
        [SerializeField]
        private string _sourcePropName;

        /// Target component.
        /// 
        /// Component which property will be updated by target game object's
        /// property.
        [SerializeField]
        private Component _targetCo;

        /// Index of the target property in the property array.
        /// 
        /// Property array contains names of all properties found in the target
        /// game object.
        [SerializeField]
        private int _targetPropIndex;

        /// Name of the selected target game object selected property.
        // TODO This shouldn't be serialized.
        [SerializeField]
        private string _targetPropName;

        /// Trigger that causes some change in the target component.
        [SerializeField]
        private Trigger _trigger;

        #endregion FIELDS

        #region PROPERTIES

        public Component SourceCo {
            get { return _sourceCo; }
            set { _sourceCo = value; }
        }

        public int SourcePropIndex {
            get { return _sourcePropIndex; }
            set { _sourcePropIndex = value; }
        }

        public string SourcePropName {
            get { return _sourcePropName; }
            set { _sourcePropName = value; }
        }

        public Component TargetCo {
            get { return _targetCo; }
            set { _targetCo = value; }
        }

        public int TargetPropIndex {
            get { return _targetPropIndex; }
            set { _targetPropIndex = value; }
        }

        public string TargetPropName {
            get { return _targetPropName; }
            set { _targetPropName = value; }
        }

        #endregion PROPERTIES

        #region UNITY MESSAGES

        private void Awake() {
            // Initialize class fields.
            _sourcePropInfo = _sourceCo.GetType().GetProperty(_sourcePropName);
        }

        // TODO Add option to update in FixedUpdate().
        private void Update() {
            if (_sourcePropInfo == null) return;

            // Value of the source property.
            var sourceType = _sourcePropInfo.PropertyType;
            // Type of the source property value.
            var sourceValue = _sourcePropInfo.GetValue(_sourceCo, null);

            // Handle trigger option.
            switch (_trigger) {
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
            _targetCo.gameObject.SetActive(true);
        }

        /// Handle 'Equal' option.
        private void HandleEqual(object sourceValue, Type sourceType) {
            // Check source type and then compare with the _conditionValue.
            switch (sourceType.ToString()) {
                case "System.Int32":
                    // Keep source value as integer.
                    var intValue = (int) sourceValue;
                    if (intValue == (int) _conditionValue) {
                        HandleEnableAction();
                    }
                    break;

                case "System.Single":
                    var floatValue = (float) sourceValue;
                    if (floatValue == _conditionValue) {
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
                    if (intValue <= (int) _conditionValue) {
                        HandleEnableAction();
                    }
                    break;

                case "System.Single":
                    float floatValue;
                    floatValue = (float) sourceValue;
                    if (floatValue <= _conditionValue) {
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
                    if (intValue < (int) _conditionValue) {
                        HandleEnableAction();
                    }
                    break;

                case "System.Single":
                    float floatValue;
                    floatValue = (float) sourceValue;
                    if (floatValue < _conditionValue) {
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
                    if (intValue > (int) _conditionValue) {
                        HandleEnableAction();
                    }
                    break;

                case "System.Single":
                    float floatValue;
                    floatValue = (float) sourceValue;
                    if (floatValue > _conditionValue) {
                        HandleEnableAction();
                    }
                    break;
            }
        }

        #endregion METHODS
    }

}