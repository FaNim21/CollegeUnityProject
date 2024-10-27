using Main.Misc;
using UnityEngine;

namespace Main.Combat
{
    public class Entity : MonoBehaviour, IDamageable
    {
        public void TakeDamage(int damage)
        {
            Utils.Log($"{gameObject.name} otrzymal damage {damage}");
        }
    }
}
