using System;

namespace DGTools.Forms {
    public class FloatFieldAttribute : FormFieldAttribute
    {
        public float minValue = float.MinValue;
        public float maxValue = float.MaxValue;
        public bool positive = false;
        public bool useSlider = true;
        public int decimals = 2;

        public override Type formFieldType => typeof(FloatField);

        protected override bool CheckConstraint(object value, out string error)
        {
            float castValue = (float)value;

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

