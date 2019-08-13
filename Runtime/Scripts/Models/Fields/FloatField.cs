using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace DGTools.Forms {
    public class FloatField : FormField<float, FloatFieldAttribute>
    {
        [SerializeField] protected InputField inputField;

        [Header("Optional Links")]
        [SerializeField] Slider slider;
        
        protected override void OnConfigure()
        {
            inputField.contentType = InputField.ContentType.DecimalNumber;
            inputField.text = value.ToString("n" + attribute.decimals, CultureInfo.InvariantCulture);
            inputField.onValueChanged.AddListener(OnValueChanged);

            if (slider != null) {
                if (attribute.useSlider)
                {
                    slider.minValue = attribute.minValue;
                    slider.maxValue = attribute.maxValue;

                    slider.onValueChanged.AddListener(OnSliderValueChanged);

                    slider.gameObject.SetActive(true);

                }
                else {
                    slider.gameObject.SetActive(false);
                }
            }
        }

        protected virtual void OnSliderValueChanged(float sliderValue) {
            
            value = sliderValue;
            inputField.text = value.ToString("n" + attribute.decimals, CultureInfo.InvariantCulture);
        }

        protected virtual void OnValueChanged(string text)
        {
            float last_value = value;

            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    value = 0;
                }
                else
                {
                    value = float.Parse(text, CultureInfo.InvariantCulture);
                }
                    
            }
            catch
            {
                value = last_value;
            }

            inputField.text = value.ToString("n"+attribute.decimals, CultureInfo.InvariantCulture);
        }
    }
}

