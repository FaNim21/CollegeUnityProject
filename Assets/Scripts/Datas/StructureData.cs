using UnityEngine;

namespace Main.Datas
{
    [CreateAssetMenu(fileName = "StructureData", menuName = "Datas/Structure")]
    public class StructureData : ScriptableObject
    {
        public int maxHealth;
        //could done data type for all structures, but there is only two in game

    }
}
