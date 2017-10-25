namespace WPF_GUI
{
    public enum ProgrammState
    {
        /// <summary>
        /// Represent view state. User can only view images
        /// </summary>
        View,

        /// <summary>
        /// Represent edit state. User can edit single image, but cannot view other images
        /// </summary>
        Edit
    }
}