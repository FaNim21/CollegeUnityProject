using Main.Visual;
using UnityEngine;

namespace Main
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public PlayerController playerController;
        public Popup popup;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            Popup.InitializePooling();
        }

        public Transform GetPlayer()
        {
            return playerController.transform;
        }
    }
}
