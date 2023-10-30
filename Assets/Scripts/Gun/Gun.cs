using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace ShootGame.Gun
{
    public interface IGun
    {
        /// <summary>
        /// 射击
        /// </summary> <summary>
        /// 
        /// </summary>
        void Shoot(Vector2 shootPos, Rigidbody2D shooterRb);
    }

    public abstract class Gun : ViewController, IGun
    {
        protected RaycastHit2D hitInfo;
        protected LayerMask shootLayer;
        protected Animator gunAnimator;
        protected IBulletModel bulletModel;
        public BulletInfo curBullet;
        public bool isCombinable;//是否可连发
        public bool canShoot;

        public float shootForce;//后坐力
        [SerializeField] private float curShootIntervalTime;//当前射击时间间隔       
        protected virtual void Awake()
        {
            gunAnimator = GetComponent<Animator>();
            bulletModel = this.GetModel<IBulletModel>();
        }
        protected virtual void Start()
        {
            canShoot = true;

            curShootIntervalTime = curBullet.BulletPerSecond;
            shootLayer = 1 << 6;
        }
        protected virtual void Update()
        {
            if (canShoot)
            {

            }
            else
            {
                curShootIntervalTime -= Time.deltaTime;
                if (curShootIntervalTime < 0)
                {
                    canShoot = true;
                    curShootIntervalTime = curBullet.BulletPerSecond;
                }
            }
        }

        protected virtual void Recoil(Vector2 recoilDir, Rigidbody2D shooterRb)
        {
            shooterRb.AddForce(recoilDir * shootForce, ForceMode2D.Impulse);//后坐力
        }

        public abstract void Shoot(Vector2 shootPos, Rigidbody2D shooterRb);

    }

}
