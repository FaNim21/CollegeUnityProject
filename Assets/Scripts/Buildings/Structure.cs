using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    private float health;

    public void TakeDamage()
    {

    }

    public abstract void OnCollect();
}
