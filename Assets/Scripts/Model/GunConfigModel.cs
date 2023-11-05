using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public class GunConfigInfo
    {
        public GunConfigInfo() { }
        public GunConfigInfo(string name,float AttackRate,float Damge,int MaxBullet,string Description)
        {
            this.Name = name;
            this.AttackRate = AttackRate;
            this.Damge = Damge;
            this.MaxBullet = MaxBullet;
            this.Description = Description;
        }

        public string Name;
        public string Description;

        public float AttackRate;
        public float Damge;

        public int MaxBullet;
        
    }

    public interface IGunConfigModel : IModel
    {
        GunConfigInfo GetGunConfig(string gunName);
    }
    public class GunConfigModel : AbstractModel, IGunConfigModel
    {
        public Dictionary<string, GunConfigInfo> gunConfigsDic = new Dictionary<string, GunConfigInfo>()
        {
            {"HandGun",new GunConfigInfo("HandGun",0.5f,8,100,"平平无奇的手枪子弹")},
            {"LaserGun",new GunConfigInfo("LaserGun",0.2f,3,300,"平平无奇的激光枪子弹")}
        };
        public GunConfigInfo GetGunConfig(string gunName)
        {
            if(gunConfigsDic.TryGetValue(gunName, out GunConfigInfo gunConfig))
            {
                return gunConfig;
            }

            return null;
        }

        protected override void OnInit()
        {
           
        }
    }
}

