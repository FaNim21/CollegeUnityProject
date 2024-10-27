using Main.Combat;
using UnityEngine;

public abstract class Structure : MonoBehaviour, IDamageable
{
    [SerializeField, ReadOnly] private float health;
    [SerializeField, ReadOnly] private bool died;

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || died) return;

        health -= damage;

        if (health < 0)
        {
            died = true;
            OnDeath();
        }
    }

    protected void UpdateBar(Transform bar, float proportion)
    {
        Vector3 barScale = bar.localScale;
        barScale.x = proportion;
        bar.localScale = barScale;
    }

    protected virtual void OnDeath() { }

    public abstract void OpenGUI();
    public abstract void OnCollect();
}
