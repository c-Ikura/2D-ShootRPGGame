using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace ShootGame.Gun
{
    public class BulletGun : Gun
    {
        protected override void Start()
        {
            curBullet = bulletModel.GetBulletInfo("pistolBullet");

            base.Start();

            isCombinable = false;
        }
        protected override void Recoil(Vector2 recoilDir, Rigidbody2D shooterRb)
        {
            shooterRb.AddForce(recoilDir * 0.5f, ForceMode2D.Impulse);//������
        }

        public override void Shoot(Vector2 shootPos, Rigidbody2D shooterRb)
        {
            var dir = (shootPos - (Vector2)transform.position).normalized;


            if (canShoot)
            {
                gunAnimator.SetTrigger("Shoot");

                hitInfo = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, shootLayer);
                (curBullet as IBullet).BulletShoot(shootPos);
                Recoil(-dir, shooterRb);
                Debug.DrawLine(transform.position, hitInfo.point, Color.red, 1000);

                canShoot = false;
            }

        }
    }
}

