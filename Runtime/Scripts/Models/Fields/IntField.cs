using UnityEngine;
using UnityEngine.UI;

namespace DGTools.Forms
{
    public class IntField : FormField<int, IntFieldAttribute>
    {
        [SerializeField] InputField inputField;

        [Header("Optional Links")]
        [SerializeField] Button addButton;
        [SerializeField] Button removeButton;

        protected override void OnConfigure()
        {
            inputField.text = value.ToString();
            inputField.onValueChanged.AddListener(OnValueChanged);

            if (addButton != null)
                addButton.onClick.AddListener(OnAddButtonClick);

            if (removeButton != null)
                removeButton.onClick.AddListener(OnRemoveButtonClick);
        }

        void OnValueChanged(string text)
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

            if (attribute.runtimeCheck)
            {
                CheckValue();
            }

            inputField.text = value.ToString();
        }

        void OnAddButtonClick() {
            if (value + 1 < attribute.maxValue) {
                value++;
            }
            inputField.text = value.ToString();
        }

        void OnRemoveButtonClick()
        {
            if (value - 1 > attribute.minValue && (!attribute.positive || value > 0))
            {
                value--;
            }
            inputField.text = value.ToString();
        }
    }
}
