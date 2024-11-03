namespace Main.Combat
{
    public interface IDamageable
    {
        bool Died {get;set;}

        void TakeDamage(int damage);
    }
}
