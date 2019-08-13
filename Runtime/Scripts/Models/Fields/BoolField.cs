using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace DGTools.Forms
{
    public class BoolField : FormField<bool, BoolFieldAttribute>
    {
        [SerializeField] protected Toggle toggle;

        protected override void OnConfigure()
        {
            toggle.isOn = value;
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        protected virtual void OnValueChanged(bool toggleValue) {
            value = toggleValue;
        }
    }
}
