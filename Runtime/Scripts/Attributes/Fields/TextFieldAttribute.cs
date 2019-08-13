using System;

namespace DGTools.Forms
{
	public class TextFieldAttribute : StringFieldAttribute
	{
        public int fieldHeight = 100;

        public override Type formFieldType => typeof(TextField);
    }
}
