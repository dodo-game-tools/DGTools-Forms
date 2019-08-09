namespace DGTools.Forms {
    /// <summary>
    /// An item should implement that interface to be used in a <see cref="Form"/>
    /// </summary>
    public interface IFormBindable
    {

    }

    /// <summary>
    /// The same as <see cref="IFormBindable"/>, but has an OnUpdate() methods that is called when the item is updated by the <see cref="Form"/>
    /// </summary>
    public interface IFormBindableWithCallback : IFormBindable
    {
        void OnUpdate();
    }
}

