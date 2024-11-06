using UnityEngine;

namespace Main.Datas
{
    public enum ItemType
    {
        None,
        Material,
        Fuel,
        Structure,
    }

    [CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory/Item")]
    public class ItemData : ScriptableObject
    {
        public int ID;
        public string codeName;
        public new string name;
        public ItemType type;
        [TextArea(2, 4)] public string description;
        public Sprite icon;
        public int maxStackSize;

        [Header("Fuel")]
        public float fuelPower;
    }
}