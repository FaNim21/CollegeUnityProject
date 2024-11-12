using UnityEngine;

namespace Main.Combat
{
    public class Projectile : MonoBehaviour
    {
        public Rigidbody2D rb;

        public int damage;
        public float speed;

        private Vector2 velocity;

        public void Setup(int layerMask, Vector2 velocity, float speed, int damage, float lifeTime = 1f)
        {
            gameObject.layer = layerMask;
            gameObject.transform.GetChild(0).gameObject.layer = layerMask;

            this.velocity = velocity;
            this.speed = speed;
            this.damage = damage;

            Destroy(gameObject, lifeTime);
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + speed * Time.deltaTime * velocity);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.parent.TryGetComponent<IDamageable>(out var entity))
            {
                entity.TakeDamage(damage);
                //Object pooling is useless in this games
                Destroy(gameObject);
            }
        }
    }
}
