using Main.Visual;
using UnityEngine;
using Main.Player;
using Main.Combat;

namespace Main
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public MapManager mapManager;

        public PlayerController playerController;
        public Popup popup;
        public Projectile projectile;


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
