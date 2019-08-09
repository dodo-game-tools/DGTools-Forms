using System.Collections.Generic;
using UnityEngine;
using System;

namespace DGTools.Forms
{
    /// <summary>
    /// Use this to tell to the form which <see cref="FormField"/> it should use for a given <see cref="FormFieldAttribute"/>
    /// </summary>
    [CreateAssetMenu(menuName = "DGTools/Forms/Theme")]
    public class FormTheme : ScriptableObject
    {
        [Header("Default Fields")]
        [SerializeField] IntField intField;
        [SerializeField] StringField stringField;
        [SerializeField] FloatField floatField;
        [SerializeField] BoolField boolField;

        [Header("Custom Fields")]
        [SerializeField] List<FormField> customFields;

        /// <summary>
        /// Returns the linked <see cref="FormField"/> from a given <see cref="FormFieldAttribute"/>
        /// </summary>
        /// <param name="attribute">The linked <see cref="FormFieldAttribute"/></param>
        public FormField GetFieldFromAttribute(FormFieldAttribute attribute)
        {
            if (attribute.formFieldType == typeof(IntField)) return intField;
            else if(attribute.formFieldType == typeof(StringField)) return stringField;
            else if(attribute.formFieldType == typeof(FloatField)) return floatField;
            else if(attribute.formFieldType == typeof(BoolField)) return boolField;

            else
                foreach (FormField field in customFields)
                    if (field.GetType() == attribute.formFieldType) return field;


            throw new Exception(string.Format("No {0} field (from attribute {1}) found in {2} Theme", attribute.formFieldType, attribute.GetType(), name));
        }
    }
}
