using System;

namespace DGTools.Forms
{
    /// <summary>
    /// The base class for form fields attributes
    /// It makes the link between the <see cref="FormField"/> and the targeted field and provides some constraints
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class FormFieldAttribute : Attribute
    {
        /// <summary>
        /// The text of the label for the field (field name by default)
        /// </summary>
        public string label = null;

        /// <summary>
        /// If true, the field will be checked every time it's changed in <see cref="Form"/>
        /// </summary>
        public bool runtimeCheck = false;

        /// <summary>
        /// True if this field can't be null
        /// </summary>
        public bool required = false;

        /// <summary>
        /// The field will be visible only in the given <see cref="FormMode"/> (all modes are selected by default)
        /// You can set multiple <see cref="FormMode"/> with flags
        /// </summary>
        public FormMode onlyVisibleIn = FormMode.all;

        /// <summary>
        /// The type of the linked <see cref="FormField"/>
        /// </summary>
        public abstract Type formFieldType { get; }

        /// <summary>
        /// Check all the constraints for a given value
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="error">Returns the first error, null if there is no error</param>
        /// <returns>True if all the constraints are satisfied</returns>
        public bool RunCheckConstraint(object value, out string error)
        {
            if (required && value == null)
            {
                error = "A value is required";
                return false;
            }

            return CheckConstraint(value, out error);
        }

        protected abstract bool CheckConstraint(object value, out string error);
    }
}
