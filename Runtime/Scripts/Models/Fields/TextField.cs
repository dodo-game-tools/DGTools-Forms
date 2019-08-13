using System;
using UnityEngine;
using UnityEngine.UI;

namespace DGTools.Forms
{
	public class TextField : StringField
	{
        [SerializeField] LayoutElement inputLayout;

        public override Type attributeType => typeof(TextFieldAttribute);

        protected override void OnConfigure()
        {
            Debug.Log(((TextFieldAttribute)attribute).fieldHeight);
            inputField.textComponent.horizontalOverflow = HorizontalWrapMode.Wrap;
            inputField.lineType = InputField.LineType.MultiLineNewline;
            inputLayout.minHeight = ((TextFieldAttribute)attribute).fieldHeight;

            base.OnConfigure();
            
            //RectTransform fieldTransform = inputField.GetComponent<RectTransform>();
            //fieldTransform.sizeDelta = new Vector2(fieldTransform.sizeDelta.x, ((TextFieldAttribute)attribute).fieldHeight);
        }
    }
}
