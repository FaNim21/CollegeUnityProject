using UnityEngine;

namespace Main.Datas
{
    [CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory/Item")]
    public class ItemData : ScriptableObject
    {
        public int ID;
        public new string name;
        [TextArea(4, 4)] public string description;
        public Sprite icon;
        public int maxStackSize;
    }
}