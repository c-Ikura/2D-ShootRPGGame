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
        /// ��ǹ(ǹе��)
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
        /// �ӵ�����ʱ����
        /// </summary>
        public void RemoveGun()
        {
            curGun = gunList[0];
            gunList.RemoveAt(gunIndex);                
        }

        /// <summary>
        /// �����ӵ���ǹ���µ�ǹе��
        /// </summary>
        public void UpGunDate()//�����ӵ�����ӵ�ǹе����
        {
            foreach (var gun in gunDic)
            {
                if (gun.Value.bulletCount.Value > 0)
                {
                    if (gunList.Contains(gun.Value))
                    {
                        //���ǹе�����оͲ����
                    }
                    else
                    {
                        //û�� Ҫ���
                        gunList.Add(gun.Value);
                        Debug.Log("����� " + gun.Key + "�ӵ����� " + gun.Value.bulletCount.Value);
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

