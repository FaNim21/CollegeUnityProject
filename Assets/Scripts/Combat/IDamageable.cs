using UnityEngine;

namespace Main.Combat
{
    public interface IDamageable
    {
        Vector3 Position { get; }
        float Size { get; }
        bool Died {get;set;}

        void TakeDamage(int damage);
    }
}
