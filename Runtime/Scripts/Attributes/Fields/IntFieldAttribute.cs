using System;

namespace DGTools.Forms
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class IntFieldAttribute : FormFieldAttribute
    {
        public int minValue = int.MinValue;
        public int maxValue = int.MaxValue;
        public bool positive = false;

        public override Type formFieldType => typeof(IntField);

        protected override bool CheckConstraint(object value, out string error)
        {
            int castValue = (int)value;

            if (castValue < minValue)
            {
                error = string.Format("Value should be greater than {0}", minValue);
                return false;
            }

            if (castValue > maxValue)
            {
                error = string.Format("Value should be lower than {0}", maxValue);
                return false;
            }

            if (positive && castValue < 0)
            {
                error = "Value should be positive";
                return false;
            }

            error = null;
            return true;
        }
    }
}