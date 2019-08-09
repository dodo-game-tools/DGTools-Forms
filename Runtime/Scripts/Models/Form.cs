using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DGTools.Forms {

    public enum FormMode {
        create = 1,
        edit = 1 << 2,
        custom1 = 1 << 3,
        custom2 = 1 << 4,
        custom3 = 1 << 5,
        all = ~0
    }

    /// <summary>
    /// The main form class, set an <see cref="IFormBindable"/> item to it, and it will generate a form from item's <see cref="FormFieldAttribute"/>
    /// </summary>
    public class Form : MonoBehaviour
    {
        #region Public Variables
        [Header("Display Settings")]
        [SerializeField] FormTheme theme;

        [Header("UIRelations")]
        [SerializeField] Button validateButton;
        [SerializeField] Button cancelButton;

        [Header("Texts Settings (optional)")]
        [Tooltip("{0} => FormInfosAttribute title value (if it has one), {1} => Item Type Name, {2} => current FormMode. Keep this field empty to use your Text text")]
        [SerializeField] string titleFormat = "{0} ({2})";
        [SerializeField] Text title;
        [Tooltip("{0} => FormInfosAttribute title value (if it has one), {1} => Item Type Name, {2} => current FormMode. Keep this field empty to use your Text text")]
        [SerializeField] string descriptionFormat = "A simple form for {1}";
        [SerializeField] Text description;

        [Header("Optional Settings")]
        [Tooltip("The item that will contains the fields GameObjects")]
        [SerializeField] Transform fieldsContainer;
        [SerializeField] [TypeConstraint(typeof(IFormBindable))] GameObject _item;
        [SerializeField] Button resetButton;
        #endregion

        #region Events
        [Serializable] public class FormEvent : UnityEvent<Form> { }
        /// <summary>
        /// Is called when the form validates
        /// </summary>
        [Header("Events")]
        public FormEvent onValidate = new FormEvent();
        /// <summary>
        /// Is called when the form is canceled
        /// </summary>
        public FormEvent onCancel = new FormEvent();
        #endregion

        #region Properties
        /// <summary>
        /// The item bound by the form
        /// </summary>
        public IFormBindable item { get; private set; }

        /// <summary>
        /// The current <see cref="FormMode"/>
        /// </summary>
        public FormMode mode { get; private set; }

        /// <summary>
        /// All the instantiated fields in <see cref="Form.fieldsContainer"/>
        /// </summary>
        public FormField[] fields => fieldsContainer.GetComponentsInChildren<FormField>();

        /// <summary>
        /// The infos provided by the <see cref="FormInfosAttribute"/> of the <see cref="Form.item"/>. Null if item doesn't use this attribute
        /// </summary>
        public FormInfosAttribute formInfos { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a new item of type <typeparamref name="Titem"/> and bind it to the form (The form enters in <see cref="FormMode.create"/>
        /// </summary>
        /// <typeparam name="Titem">The type of the item you want to create from the form</typeparam>
        public void CreateItem<Titem>() where Titem : IFormBindable, new() {
            SetItem(new Titem(), FormMode.create);
        }

        /// <summary>
        /// Use this to bind an item to the form. The form will automatically build fields
        /// </summary>
        /// <param name="item">The item to bind</param>
        /// <param name="mode">The active <see cref="FormMode"/></param>
        public void SetItem(IFormBindable item, FormMode mode = FormMode.edit)
        {
            this.item = item;
            this.mode = mode;

            ReloadForm();

        }

        /// <summary>
        /// Reloads the form
        /// </summary>
        public void ReloadForm() {
            ClearForm();
            BuildForm();
        }

        /// <summary>
        /// Clears the form
        /// </summary>
        public void ClearForm() {
            foreach (FormField field in fields)
            {
                Destroy(field.gameObject);
            }
        }
        #endregion

        #region Private Methods
        void BuildForm() {
#if UNITY_EDITOR
            name = string.Format("Form({0})", item.GetType().Name);
#endif

            formInfos = item.GetType().GetCustomAttribute<FormInfosAttribute>();

            if (title != null && !string.IsNullOrEmpty(titleFormat)) {
                title.text = string.Format(titleFormat, formInfos != null ? formInfos.title : "", item.GetType().Name, mode);
            }

            if (description != null && !string.IsNullOrEmpty(descriptionFormat))
            {
                description.text = string.Format(descriptionFormat, formInfos != null ? formInfos.description : "", item.GetType().Name, mode);
            }

            foreach (FieldInfo fieldInfo in item.GetType().GetRuntimeFields()) {
                FormFieldAttribute attribute = fieldInfo.GetCustomAttribute<FormFieldAttribute>();
                if (attribute != null) {
                    if (!attribute.onlyVisibleIn.HasFlag(mode)) continue;
                    FormField field = Instantiate(theme.GetFieldFromAttribute(attribute), fieldsContainer);
                    field.Configure(fieldInfo, attribute, fieldInfo.GetValue(item));
                }
            }
        }

        bool ValidateForm() {
            bool isValid = true;
            foreach (FormField field in fields) {
                if (!field.CheckValue())
                    isValid = false;
            }
            return isValid;
        }

        void BindItem() {
            foreach (FormField field in fields)
            {
                field.Bind(item);
            }

            if (item is IFormBindableWithCallback) {
                ((IFormBindableWithCallback)item).OnUpdate();
            }
        }
        #endregion

        #region Event Methods
        void OnValidateClick() {
            if (ValidateForm()) {
                BindItem();
                onValidate.Invoke(this);
            }
        }

        void OnCancelClick() {
            onCancel.Invoke(this);
        }

        void OnResetClick() {
            ReloadForm();
        }
        #endregion

        #region Runtime Methods
        private void Awake()
        {
            if (_item != null) {
                SetItem(_item.GetComponent<IFormBindable>());
            }

            validateButton.onClick.AddListener(OnValidateClick);
            cancelButton.onClick.AddListener(OnCancelClick);
            if (resetButton != null) resetButton.onClick.AddListener(OnResetClick);
        }
        #endregion


#if UNITY_EDITOR
        #region Editor Methods
        [MenuItem("GameObject/UI/DGTools/Form", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            Form form = Instantiate(Resources.Load<Form>("Prefabs/default_Form"));
            form.name = "Form";
            GameObjectUtility.SetParentAndAlign(form.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(form.gameObject, "Create " + form.name);
            Selection.activeObject = form;
        }

        private void OnValidate()
        {
            if (fieldsContainer == null)
            {
                fieldsContainer = transform;
            }
        }
        #endregion
#endif
    }
}

