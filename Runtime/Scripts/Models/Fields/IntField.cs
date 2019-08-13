using UnityEngine;
using UnityEngine.UI;

namespace DGTools.Forms
{
    public class IntField : FormField<int, IntFieldAttribute>
    {
        [SerializeField] protected InputField inputField;

        [Header("Optional Links")]
        [SerializeField] protected Button addButton;
        [SerializeField] protected Button removeButton;

        protected override void OnConfigure()
        {
            inputField.contentType = InputField.ContentType.IntegerNumber;

            inputField.text = value.ToString();
            inputField.onValueChanged.AddListener(OnValueChanged);

            if (addButton != null)
                addButton.onClick.AddListener(OnAddButtonClick);

            if (removeButton != null)
                removeButton.onClick.AddListener(OnRemoveButtonClick);
        }

        protected virtual void OnValueChanged(string text)
        {
            int last_value = value;

            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    value = 0;
                }
                else
                    value = int.Parse(text);
            }
            catch
            {
                value = last_value;
            }

            inputField.text = value.ToString();
        }

        protected virtual void OnAddButtonClick() {
            if (value + 1 < attribute.maxValue) {
                value++;
            }
            inputField.text = value.ToString();
        }

        protected virtual void OnRemoveButtonClick()
        {
            if (value - 1 > attribute.minValue && (!attribute.positive || value > 0))
            {
                value--;
            }
            inputField.text = value.ToString();
        }
    }
}
