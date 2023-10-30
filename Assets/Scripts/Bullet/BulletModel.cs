using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace ShootGame
{
    public interface IBulletModel : IModel
    {
        /// <summary>
        /// 获取子弹信息
        /// </summary> <summary>
        /// 
        /// </summary>
        BulletInfo GetBulletInfo(string name);
    }
    public class BulletModel : AbstractModel, IBulletModel
    {
        public Dictionary<string, BulletInfo> bulletInfoDic = new Dictionary<string, BulletInfo>
        {
            {"pistolBullet", new PistolBulle("pistolBullet", 0.1f, 10, 10)},
            {"laserBullet", new LaserBulle("laserBullet", 0.2f, 4, 20)}
        };

        public BulletInfo GetBulletInfo(string name)
        {
            bulletInfoDic.TryGetValue(name, out var bulletInfo);
            if (bulletInfo != null)
            {
                return bulletInfo;
            }

            return null;
        }

        protected override void OnInit()
        {
        }

    }
}

