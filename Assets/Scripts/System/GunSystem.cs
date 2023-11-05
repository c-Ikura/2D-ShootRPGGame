using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEditor;
using UnityEngine;

namespace ShootGame
{
    public interface IGunSystem : ISystem
    {
        public GunInfo curGun { get; set; }
        void ShiftGun();
        void RemoveGun();
        void UpGunDate();
        GunInfo GetGunInfo(string gunName);               
    }
    public class GunSystem : AbstractSystem, IGunSystem
    {
        public GunInfo curGun { get ; set ; }

        public List<GunInfo> gunList = new List<GunInfo>();

        public int gunIndex = 0;

        public Dictionary<string, GunInfo> gunDic = new Dictionary<string, GunInfo>()
        {
            {"HandGun",new GunInfo()
            {
                bulletCount = new BindableProperty<int>(10),
                name = new BindableProperty<string>("HandGun"),
                gunState = new BindableProperty<GunState>(GunState.Idel)
                
            } },

            {"LaserGun",new GunInfo()
            {
                bulletCount = new BindableProperty<int>(10),
                name = new BindableProperty<string>("LaserGun"),
                gunState = new BindableProperty<GunState>(GunState.Idel)
    }       },                             
        };
        protected override void OnInit()
        {
            UpGunDate();
            ShiftGun(); 
        }

        /// <summary>
        /// 切枪(枪械库)
        /// </summary>
        public void ShiftGun()
        {
            if(gunList.Count > 0)
            {
                gunIndex++;
                if(gunIndex> gunList.Count-1)
                {
                    gunIndex = 0;
                }
                else if(gunIndex < 0)
                {
                    gunIndex = gunList.Count-1;
                }

                curGun = gunList[gunIndex];
            }
        }
        /// <summary>
        /// 子弹用完时调用
        /// </summary>
        public void RemoveGun()
        {
            curGun = gunList[0];
            gunList.RemoveAt(gunIndex);                
        }

        /// <summary>
        /// 把有子弹的枪更新到枪械库
        /// </summary>
        public void UpGunDate()//把有子弹的添加到枪械库里
        {
            foreach (var gun in gunDic)
            {
                if (gun.Value.bulletCount.Value > 0)
                {
                    if (gunList.Contains(gun.Value))
                    {
                        //如果枪械库里有就不添加
                    }
                    else
                    {
                        //没有 要添加
                        gunList.Add(gun.Value);
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

