using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame.Gun
{
    public class RayGun : Gun
    {
        public bool isShoot;
        public bool isPower;
        protected override void Start()
        {
            curBullet = bulletModel.GetBulletInfo("laserBullet");
            base.Start();
            isCombinable = true;
        }
        protected override void Update()
        {
            base.Update();

            gunAnimator.SetBool("isPower", isPower);
        }
        protected override void Recoil(Vector2 recoilDir, Rigidbody2D shooterRb)
        {
            shooterRb.AddForce(recoilDir * 0.5f, ForceMode2D.Impulse);//ºó×øÁ¦
        }

        public override void Shoot(Vector2 shootPos, Rigidbody2D shooterRb)
        {
            var dir = (shootPos - (Vector2)transform.position).normalized;
            Debug.DrawRay(transform.position, dir * 1000, Color.green);

            isPower = true;

            if (isPower)
            {
                if (canShoot)
                {
                    hitInfo = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, shootLayer);
                    (curBullet as IBullet).BulletShoot(shootPos);
                    Recoil(-dir, shooterRb);
                    Debug.DrawLine(transform.position, hitInfo.point, Color.red, 1000);

                    canShoot = false;
                }
            }
        }
    }
}

