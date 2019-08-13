# DGTools Form

A simple form generator for Unity

## Forms?

Yep! Every web frameworks provide easy form generators, but it still really boring and redundant to manage forms in Unity.
This tool gives you a simple form generator which generates fields from an item and binds form results to it.

## Installing

To install this package you'll need [DGTools-Core](https://github.com/Poulpinou/DGTools-Core), click [here](https://github.com/Poulpinou/DGTools-Core) to know how, then open "DGTools/Package Importer" window in the Unity Editor and click on "Install" next to "com.dgtools.forms".

## How to use?

### 1 - Implement the IFormBindable interface

You can build a form on every item, MonoBehaviour or not! To do this you just have to implement the IFormBindable interface

```
using DGTools.Forms

public class MyItem : IFormBindable {

}
```

### 2 - Add FormField attributes

To know which fields the form should use and how, you should add some FormField attributes your fields.

The attribute should match your field type. You can add constraints to your field from the attribute.

```
[FormInfos(title = "My wonderful form", description = "Waow... Like I said!")]     //Gives a title and a description to your form (optional)
public class MyItem : IFormBindable {

    [IntField] public int myInt;                                          // Creates an int field in the form

    [FloatField(maxValue = 42.0f, minValue = -20)] public int myFloat;    // Creates a float field with value constraints

    [StringField(
        label = "User Name",											  // Adds a custom label to the field
        placeHolder = "Poulpinou",                                        // Adds a placeholder
        excludeDigits = true,                                             // Not allows digits in result
        required = true,                                                  // Forces the user to put a value in the field
		runtimeCheck = true                                               // Checks the constraint every time the user changes the value
    )]
    string myString;

    [BoolField(
        onlyVisibleIn = FormMode.custom1 | FormMode.create                // This field will only be visible when the form is in create or in custom1 mode
    )]
    bool myBool;
}
```

### 3 - Create a form in your scene

You can create it from raw or create it from the **Create/UI/DGTools/BaseForm** Menu.

You will need a FormTheme, you can create it from your assets (right click + **"DGTools/Forms/Theme"**) or use the one provided by default.

The FormTheme makes the link beetween the FormFieldAttributes and your FormField prefabs

### 4 - Put the item in your Form

How can it be easier?

```
public class TestForm : MonoBehaviour
{
    [SerializeField] Form form;

    void Start()
    {
        MyItem item = new MyItem();

        form.SetItem(item);									// Just set the item to the form and it will automatically generate the form in your scene
		form.onValidate.AddListener(OnFormValidate);        // Add a listener if you want to do something when form validates
    }

	void OnFormValidate(Form form) {
        MyItem item = (MyItem)form.item;
        Debug.Log(test.myString);                           // You'll get the item with the values inputed in the form!
    }
}
```

There is other options you can use 

```
form.SetItem(item, FormMode.custom1);           // This will filter fields and only display fields with onlyVisibleIn == FormMode.custom1  or FormMode.all (default)	

form.CreateItem<MyItem>();                      // This will create a new MyItem item and bind it to the form (only if you have an empty constructor in MyItem)
```

### 5 - Customize your fields

#### Option 1 : Cutomize only graphics

You can create your own fields prefabs, you just have to validate these constraints :

**1)** The root of the prefab should have a script that inherit from **FormField** attached to it

**2)** At least all the parameters that are not in the **"Optional Links"** section should be provided


Now, to use this prefab you have to add this to a **FormTheme** (you can create a new one from the create menu : right click in the assets + **"DGTools/Forms/Theme"**)

If your field inherits from basic fields, you can put it in the **"Base Fields"** section (ex : StringField goes in String Field... Yep, that was tricky...)

Custom fields are for... Custom fields! (= next section) For the example, by default TextField is in custom fields because it inherits from a base field (StringField), but it is **NOT** a base field (TextField) 

#### Option 2 : Create custom fields

If you need more complex fields for your game, you'll have to create your own fields. How to do that? Follow those steps : 

**1) Create your custom attribute**. The attribute, like I said earlier, contains the constraints, their checks and will be use to call the field from your item. You can follow this template : 

```
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MyCustomFieldAttribute : FormFieldAttribute
    {
	/*
	* Put here some constraint fields + their default value
	*
	* Ex from int field : 
	* public int minValue = int.MinValue;
	*
	*/

	// Override this and put the type of the FormField in it
        public override Type formFieldType => typeof(MyCustomField);

	// This method will be used to check the constraints 
        protected override bool CheckConstraint(object value, out string error)
        {
	    /*
	    * Ex from int field :
            * int castValue = (int)value;                                               // Cast the object to your value type if needed
			*
            * if (castValue < minValue)                                                 // Compare value with your contraint field
            * {
            *     error = string.Format("Value should be greater than {0}", minValue);  // Returns the error
            *     return false;															// Returns false
            * }
	    *
	    */

	    // If there is no error, return this
            error = null;
            return true;
        }
    }
```
	
	
		
**2) Create your custom field**. Like the attribute, you'll have to override FormField or a child of it. Follow this template : 

```
    public class MyCustomField : FormField<{my custom type}, CustomFieldAttribute>
    {
	// Add some relations with the UI elements, you'll often wanna use and InputField, use it like this : 
        [SerializeField] InputField inputField;

	// You will need to override this if it doesn't inherit directly from FormField
	public override Type attributeType => typeof(CustomFieldAttribute);

	// This method will be called when the form field is created (you can see this as an Awake())
        protected override void OnConfigure()
        {
	    // Do what you need to do to build your field, ex : 
            inputField.text = value.ToString();                      // The value will fetch the provided type ({my custom type} here)
            inputField.onValueChanged.AddListener(OnValueChanged);   // You will need a way to change the value of your field, events are a good way
        }

	// Here an example of check but you can create your own 
        void OnValueChanged(string text)
        {
	    // You need to assign the value field, taht it the one taht will be used to bind the item
            value = ({my custom type})text

            inputField.text = value.ToString();
        }

	// Sometimes, your provided {my custom type} will not be able to automatically cast from object, you'll have an error that you can fix like this : 
	protected override {my custom type} CastObjectToValue(object objectToCast)
        {
	    // Cast your item here
            return castedItem;
        }
    }
```



**3) Add your new field to the FormTheme**. Create and add your custom field prefab in the "Custom Fields" list (see the previous section) and that's it! 
You can now use your custom field like this : 

```
	public class MyItem : IFormBindable {

		[MyCustomField(minvalue = 42)] {my custom type} myField;

	}
```


See [Documentation API](https://poulpinou.github.io/DGTools-Forms/annotated.html) for more details.

## Authors

* **Donovan Persent ([Poulpinou](https://github.com/Poulpinou))**

## Licenses
See [Licence](https://github.com/Poulpinou/DGTools-Core/LICENCE.md)
