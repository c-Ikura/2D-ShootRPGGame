using QFramework;
using UnityEngine;

namespace ShootGame
{
    public class Bullet : ViewController
    {
        public Rigidbody2D bulletRb;
        public IGameObjectPool bulletPool;
        private void Awake()
        {
            bulletRb = GetComponent<Rigidbody2D>();
            bulletPool = this.GetSystem<IGameObjectPool>();
        }

        private void Update()
        {
            bulletRb.velocity = transform.up * 20;
        }

        private void OnEnable()
        {

        }
        private void OnDisable()
        {
            bulletRb.velocity = Vector2.zero;
        }
       
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag != "Player")
            {
                bulletPool.Release("Bullet_Single", this.gameObject);
                var boomEffect = bulletPool.Get("Bullet_BoomEffect");
                boomEffect.transform.position = transform.position;

            }
        }

    }
}

