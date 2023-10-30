using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public interface IBullet
    {
        void BulletShoot(Vector2 pos);
    }

    /// <summary>
    /// 子弹信息类
    /// </summary>


    public class BulletInfo
    {
        public BulletInfo(string name, float bulletPerSecond, float bulletDamage, float attackRange)
        {
            Name = name;
            BulletPerSecond = bulletPerSecond;
            BulletDamage = bulletDamage;
            AttackRange = attackRange;
        }
        public string Name { get; set; }
        public float BulletPerSecond { get; set; }//子弹射速
        public float BulletDamage { get; set; }//子弹伤害
        public float AttackRange { get; set; }//射程范围
    }


    public class PistolBulle : BulletInfo, IBullet
    {
        public PistolBulle(string name, float bulletPerSecond, float bulletDamage, float attackRange) : base(name, bulletPerSecond, bulletDamage, attackRange)
        {

        }

        public void BulletShoot(Vector2 pos)
        {
            Debug.Log("射出手枪子弹" + pos);
        }
    }

    public class LaserBulle : BulletInfo, IBullet
    {
        public LaserBulle(string name, float bulletPerSecond, float bulletDamage, float attackRange) : base(name, bulletPerSecond, bulletDamage, attackRange)
        {

        }

        public void BulletShoot(Vector2 pos)
        {
            Debug.Log("射出激光子弹" + pos);
        }
    }
}

