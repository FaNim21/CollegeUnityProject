
namespace Main.UI
{
    public interface IWindowControl
    {
        bool IsActive { get; }

        void OpenWindow();
        void ToggleWindow()
        {
            if (!IsActive)
                OpenWindow();
            else 
                CloseWindow();
        }
        void CloseWindow();
    }
}
