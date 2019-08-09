using System;

namespace DGTools.Forms
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BoolFieldAttribute : FormFieldAttribute
    {
        public override Type formFieldType => typeof(BoolField);

        protected override bool CheckConstraint(object value, out string error)
        {
            bool castValue = (bool)value;

            error = null;
            return true;
        }
    }
}
