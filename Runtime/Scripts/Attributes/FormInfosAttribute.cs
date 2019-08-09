using System;

namespace DGTools.Forms
{
    /// <summary>
    /// This attribute provides infos to the <see cref="Form"/> when it builds on current class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FormInfosAttribute : Attribute
	{
        public string title;
        public string description;
    }
}
