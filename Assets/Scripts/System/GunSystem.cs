using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;

namespace ShootGame
{
    public interface IGunSystem : ISystem
    {
        public GunInfo curGun { get; set; }
        void ShiftGun();
        GunInfo GetGunInfo(string gunName);
        void UpGunDate();
        
    }
    public class GunSystem : AbstractSystem, IGunSystem
    {
        public GunInfo curGun { get ; set ; }

        public Queue<GunInfo> guns = new Queue<GunInfo>();

        public Dictionary<string, GunInfo> gunDic = new Dictionary<string, GunInfo>()
        {
            {"HandGun",new GunInfo()
            {
                bulletCount = new BindableProperty<int>(10),
                name = new BindableProperty<string>("HandGun")
            } },

            {"LaserGun",new GunInfo()
            {
                bulletCount = new BindableProperty<int>(10),
                name = new BindableProperty<string>("LaserGun")
            } },                             
        };
        protected override void OnInit()
        {
            UpGunDate();
            ShiftGun(); 
        }
        public void ShiftGun()
        {
            if(guns.Count > 0)
            {                
                curGun = guns.Dequeue();                
                guns.Enqueue(curGun);

            }
        }
        public void UpGunDate()//把有子弹的添加到枪械库里
        {
            foreach (var gun in gunDic)
            {
                if (gun.Value.bulletCount.Value > 0)
                {
                    if (guns.Contains(gun.Value))
                    {
                        //如果枪械库里有就不添加
                    }
                    else
                    {
                        //没有 要添加
                        guns.Enqueue(gun.Value);
                        Debug.Log("添加了 " + gun.Key + "子弹数量 " + gun.Value.bulletCount.Value);
                    }
                }
            }
        }

        public GunInfo GetGunInfo(string gunName)
        {
            if(gunDic.TryGetValue(gunName,out var gunInfo))
            {
                return gunInfo;
            }
            
            return null;
        }
    }
}

