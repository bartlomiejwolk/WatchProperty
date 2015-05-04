using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace WatchProperty {

    /// Observe component property and act when value is changed.
    public class WatchProperty : MonoBehaviour {

        /// Triggers on the source component that trigger actions on
        /// the target component.
        public enum Triggers {

            Equal,
            EqualOrLess,
            LessThan,
            MoreThan

        }

        /// Actions that can be done to the target component.
        public enum Actions {

            Enable,
            Disable,
            Set

        }

        /// Source component.
        ///
        /// Component which property will be used to update
        /// target game object.
        [SerializeField]
        private Component _sourceCo;

        public Component SourceCo {
            get { return _sourceCo; }
            set { _sourceCo = value; }
        }

        /// Target component.
        ///
        /// Component which property will be updated by
        /// target game object's property.
        [SerializeField]
        private Component _targetCo;

        public Component TargetCo {
            get { return _targetCo; }
            set { _targetCo = value; }
        }

        /// Name of the selected source game object property.
        [SerializeField]
        private string _sourcePropName;

        public string SourcePropName {
            get { return _sourcePropName; }
            set { _sourcePropName = value; }
        }

        /// Name of the selected target game object selected property.
        // TODO This shouldn't be serialized.
        [SerializeField]
        private string _targetPropName;

        public string TargetPropName {
            get { return _targetPropName; }
            set { _targetPropName = value; }
        }

        /// Index of the source property in the property array.
        ///
        /// Property array contains names of all properties
        /// found in the source game object.
        [SerializeField]
        private int _sourcePropIndex = 0;

        public int SourcePropIndex {
            get { return _sourcePropIndex; }
            set { _sourcePropIndex = value; }
        }

        /// Index of the target property in the property array.
        ///
        /// Property array contains names of all properties
        /// found in the target game object.
        [SerializeField]
        private int _targetPropIndex = 0;

        public int TargetPropIndex {
            get { return _targetPropIndex; }
            set { _targetPropIndex = value; }
        }

        /// Metadata of the selected source property.
        private PropertyInfo _sourcePropInfo;

        /// Trigger that causes some change in the target component.
        [SerializeField]
        private Triggers _trigger;

        /// Action to be performed on the target.
        [SerializeField]
        private Actions _action;

        /// Value of the property that acts as a trigger.
        [SerializeField]
        private float _conditionValue;

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
                case Triggers.Equal:
                    HandleEqual(sourceValue, sourceType);
                    break;
                case Triggers.EqualOrLess:
                    HandleEqualOrLess(sourceValue, sourceType);
                    break;
                case Triggers.LessThan:
                    HandleLessThan(sourceValue, sourceType);
                    break;
                case Triggers.MoreThan:
                    HandleMoreThan(sourceValue, sourceType);
                    break;
            }
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
                    if (floatValue == (float) _conditionValue) {
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
                    if (floatValue <= (float) _conditionValue) {
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
                    if (floatValue < (float) _conditionValue) {
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
                    if (floatValue > (float) _conditionValue) {
                        HandleEnableAction();
                    }
                    break;
            }
        }

        private void HandleEnableAction() {
            _targetCo.gameObject.SetActive(true);
        }

    }

}