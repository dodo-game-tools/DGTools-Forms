using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

namespace DGTools.Forms
{
    /// <summary>
    /// The base class for any form field, but no form fields should inherit from this, use <see cref="FormField{Tvalue, Tattribute}"/> instead
    /// </summary>
    public abstract class FormField : MonoBehaviour
    {
        #region Public Variables
        [Header("UI Relations")]
        [SerializeField] protected Text label;
        [Tooltip("Put here a text that will show and display the error when constraint are not satisfied")]
        [SerializeField] protected Text errorText;
        #endregion

        #region Private Variables
        protected FieldInfo fieldInfo;
        protected FormFieldAttribute attribute;
        #endregion

        #region Properties
        /// <summary>
        /// The <see cref="Type"/> of the value of the <see cref="FormField"/>
        /// </summary>
        public abstract Type fieldType { get; }

        /// <summary>
        /// The <see cref="Type"/> of the linked <see cref="FormFieldAttribute"/>
        /// </summary>
        public abstract Type attributeType { get; }

        /// <summary>
        /// The uncasted value of the <see cref="FormField"/>
        /// </summary>
        public object value { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// This methods is used by the <see cref="Form"/> to configure the <see cref="FormField"/>
        /// </summary>
        /// <param name="fieldInfo">The <see cref="FieldInfo"/> of the field</param>
        /// <param name="attribute">The linked <see cref="FormFieldAttribute"/>, should have the same type thant the one defined in <see cref="FormField.attributeType"/></param>
        /// <param name="value">The field value of the item</param>
        public virtual void Configure(FieldInfo fieldInfo, FormFieldAttribute attribute, object value)
        {
#if UNITY_EDITOR
            name = string.Format("{0}_{1}_field", fieldInfo.Name, fieldInfo.FieldType.Name);
#endif
            this.fieldInfo = fieldInfo;
            this.attribute = attribute;

            if (label != null)
                label.text = (attribute.label != null ? attribute.label : fieldInfo.Name) + (attribute.required ? " *" : "");
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Call this to check the current value and display errors if any
        /// </summary>
        /// <returns>True if all constraints are satisfied</returns>
        public abstract bool CheckValue();

        /// <summary>
        /// This methods is called by the <see cref="Form"/> to bind the field value to the <see cref="IFormBindable"/> item
        /// </summary>
        /// <param name="item"></param>
        public abstract void Bind(object item);
        #endregion
    }

    /// <summary>
    /// The base class for any form field
    /// </summary>
    /// <typeparam name="Tvalue">The type of the field value</typeparam>
    /// <typeparam name="Tattribute">The type of the linked attribute</typeparam>
    public abstract class FormField<Tvalue, Tattribute> : FormField where Tattribute : FormFieldAttribute
    {
        #region Properties
        /// <summary>
        /// The casted value from <see cref="FormField"/>
        /// </summary>
        public new Tvalue value { get; protected set; }

        /// <summary>
        /// The linked <see cref="FormFieldAttribute"/>
        /// </summary>
        protected new Tattribute attribute => (Tattribute)base.attribute;

        /// <summary>
        /// The <see cref="Type"/> of the value of the <see cref="FormField"/>
        /// </summary>
        public override Type fieldType => typeof(Tvalue);

        /// <summary>
        /// The <see cref="Type"/> of the linked <see cref="FormFieldAttribute"/>
        /// </summary>
        public override Type attributeType => typeof(Tattribute);
        #endregion

        #region Public Methods
        /// <summary>
        /// This methods is used by the <see cref="Form"/> to configure the <see cref="FormField"/>
        /// </summary>
        /// <param name="fieldInfo">The <see cref="FieldInfo"/> of the field</param>
        /// <param name="attribute">The linked <see cref="FormFieldAttribute"/>, should have the same type thant the one defined in <see cref="FormField.attributeType"/></param>
        /// <param name="value">The field value of the item</param>
        public override void Configure(FieldInfo fieldInfo, FormFieldAttribute attribute, object value)
        {
            base.Configure(fieldInfo, attribute, value);

            this.value = CastObjectToValue(value);
            OnConfigure();
        }

        /// <summary>
        /// Call this to check the current value and display errors if any
        /// </summary>
        /// <returns>True if all constraints are satisfied</returns>
        public override bool CheckValue()
        {
            string error = null;
            bool result = attribute.RunCheckConstraint(value, out error);

            errorText.gameObject.SetActive(error != null);
            errorText.text = error;

            return result;
        }

        /// <summary>
        /// This methods is called by the <see cref="Form"/> to bind the field value to the <see cref="IFormBindable"/> item
        /// </summary>
        /// <param name="item"></param>
        public override void Bind(object item)
        {
            fieldInfo.SetValue(item, value);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Override this if you need to do a specific cast
        /// </summary>
        /// <param name="objectToCast">The object to cast</param>
        /// <returns>The casted value</returns>
        protected virtual Tvalue CastObjectToValue(object objectToCast) {
            return (Tvalue)objectToCast;
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Implement this in children to execute specific configuration
        /// </summary>
        protected abstract void OnConfigure();
        #endregion
    }
}


