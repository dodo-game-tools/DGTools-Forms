using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace DGTools.Forms
{
    public class BoolField : FormField<bool, BoolFieldAttribute>
    {
        [SerializeField] Toggle toggle;       

        protected override bool CastObjectToValue(object objectToCast)
        {
            return Convert.ToBoolean(value);
        }

        protected override void OnConfigure()
        {
            toggle.isOn = value;
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        void OnValueChanged(bool toggleValue) {
            value = toggleValue;
        }
    }
}
