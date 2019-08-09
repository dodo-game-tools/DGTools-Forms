using System;
using System.Linq;

namespace DGTools.Forms {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class StringFieldAttribute : FormFieldAttribute
    {
        public string placeHolder = null;
        public int maxLength = int.MaxValue;
        public int minLength = 0;
        public bool excludeDigits = false;
        public bool excludeSpecialChars = false;

        public override Type formFieldType => typeof(StringField);

        protected override bool CheckConstraint(object value, out string error)
        {
            string castValue = string.IsNullOrEmpty((string)value) ? "" : value.ToString();

            if (castValue.Length > maxLength) {
                error = string.Format("Value should have less than {0} characters", maxLength);
                return false;
            }

            if (castValue.Length < minLength)
            {
                error = string.Format("Value should have more than {0} characters", minLength);
                return false;
            }

            if (excludeDigits && castValue.Any(char.IsDigit)) {
                error = "Value shouldn't contain digits";
                return false;
            }

            if (excludeSpecialChars && castValue.Any(ch => ! char.IsLetterOrDigit( ch )))
            {
                error = "Value shouldn't contain special chars";
                return false;
            }

            error = null;
            return true;
        }
    }
}
