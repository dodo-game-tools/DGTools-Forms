using UnityEngine;
using UnityEngine.UI;

namespace DGTools.Forms
{
    public class StringField : FormField<string, StringFieldAttribute>
    {
        [SerializeField] protected InputField inputField;

        protected override void OnConfigure()
        {
            if (!string.IsNullOrEmpty(attribute.placeHolder) && inputField.placeholder != null)
                inputField.placeholder.GetComponent<Text>().text = attribute.placeHolder;

            inputField.contentType = attribute.contentType;
            inputField.text = value;
            inputField.onValueChanged.AddListener(OnValueChanged);
        }

        protected virtual void OnValueChanged(string text)
        {
            value = text;

            inputField.text = value;
        }
    }
}
