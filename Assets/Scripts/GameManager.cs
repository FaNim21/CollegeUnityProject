using Main.Visual;
using UnityEngine;
using Main.Player;
using Main.Combat;
using Main.Datas;
using System.Collections.Generic;
using Main.UI.Equipment;

namespace Main
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public List<ItemData> items = new();

        public MapManager mapManager;

        public PlayerController playerController;
        public Popup popup;
        public Projectile projectile;
        public InventorySlot inventorySlot;


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
