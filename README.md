# DGTools Form

A simple form generator for Unity

## A Forms?

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
    float myBool;
}
```

### 3 - Create a form in your scene

You can create it from raw or create it from the **Create/UI/DGTools/Form** Menu.
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

Todo : write this doc



See [Documentation API](https://poulpinou.github.io/DGTools-Forms/annotated.html) for more details.

## Authors

* **Donovan Persent ([Poulpinou](https://github.com/Poulpinou))**

## Licenses
See [Licence](https://github.com/Poulpinou/DGTools-Core/LICENCE.md)
