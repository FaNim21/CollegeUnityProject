
namespace Main.UI
{
    public interface IWindowControl
    {
        bool IsActive { get; }

        void ToggleWindow();
    }
}
