namespace WPF_GUI.ViewModel
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
        Edit,

        /// <summary>
        /// Represent revert state. It is used by VivewModel when user reverts changes
        /// </summary>
        Revert
    }
}