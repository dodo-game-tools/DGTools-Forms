using UnityEngine;
using UnityEngine.UI;

namespace DGTools.Forms
{
    public class StringField : FormField<string, StringFieldAttribute>
    {
        [SerializeField] InputField inputField;

        protected override void OnConfigure()
        {
            if (!string.IsNullOrEmpty(attribute.placeHolder) && inputField.placeholder != null)
                inputField.placeholder.GetComponent<Text>().text = attribute.placeHolder;

            inputField.text = value;
            inputField.onValueChanged.AddListener(OnValueChanged);
        }

        void OnValueChanged(string text)
        {
            string last_value = value;

            value = text;

            if (attribute.runtimeCheck)
            {
                CheckValue();
            }

            inputField.text = value;
        }
    }
}
