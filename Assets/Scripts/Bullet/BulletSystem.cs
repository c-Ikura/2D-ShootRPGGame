using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace ShootGame
{
    public interface IBulletSystem : ISystem
    {
        BulletInfo CurBullet { get; }
        Queue<BulletInfo> GunInfos { get; }
        void PickBullet(string name, int bulletCountOutGun);
        void ShiftBullet();
    }

    public class BulletSystem : AbstractSystem, IBulletSystem
    {
        public BulletInfo CurBullet => throw new System.NotImplementedException();

        public Queue<BulletInfo> GunInfos => throw new System.NotImplementedException();

        public void PickBullet(string name, int bulletCountOutGun)
        {
            throw new System.NotImplementedException();
        }

        public void ShiftBullet()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInit()
        {

        }
    }
}
