using UnityEngine;

namespace Main.Datas
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Datas/Entity")]
    public class EntityData : ScriptableObject
    {
        public int maxHealth;
        public int damage;
        public float speed;
        public float attackTime;
    }
}
