using UnityEditor;
# if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
# endif

namespace Main.UI
{
    public class PauseMenu : MonoBehaviour, IWindowControl
    {
        public GameObject background;
        [SerializeField] private CanvasHandle _canvasHandle;

        public bool IsActive => background.activeSelf;


        private void Awake()
        {
            _canvasHandle.AddWindowToControl(this);
            _canvasHandle.AddWindowToEscapeControl(this);
        }

        public void OpenWindow()
        {
            background.SetActive(true);
            _canvasHandle.isCanvasEnabled = true;
            Time.timeScale = 0;
        }

        public void CloseWindow()
        {
            background.SetActive(false);
            Time.timeScale = 1;
            _canvasHandle.isCanvasEnabled = false;
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            Application.Quit();

# if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
# endif
        }
    }
}
