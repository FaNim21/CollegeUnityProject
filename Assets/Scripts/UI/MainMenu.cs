using UnityEditor;
# if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
# endif

namespace Main.UI
{
    public class MainMenu : MonoBehaviour
    {

        public void Play()
        {
            SceneManager.LoadScene(1);
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
