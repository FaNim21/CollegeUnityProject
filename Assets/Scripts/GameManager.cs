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

        public ItemData GetItemData(string codeName)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var current = items[i];

                if (current.codeName.Equals(codeName, System.StringComparison.OrdinalIgnoreCase))
                {
                    return current;
                }
            }
            return null;
        }
    }
}
