using QFramework;
using ShootGame;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GunContriller : ViewController
{
    public IGun curGun;
    public IGunSystem gunSystem;
    public Dictionary<string,IGun> gunDic = new Dictionary<string,IGun>();
    public Action<int> onShiftGun;

    private void Start()
    {
        var guns = GetComponentsInChildren<IGun>();

        foreach (var gun in guns)
        {
            if (!gunDic.ContainsKey(gun.gunName))
            {               
                gunDic.Add(gun.gunName, gun);//Ìí¼Óµ½×Öµä
            }
            
        }
        
        gunSystem = this.GetSystem<IGunSystem>();

       curGun = gunDic[gunSystem.curGun.name.Value];
    }


    private void OnEnable()
    {
        onShiftGun += ShiftGun;
    }
    private void OnDisable()
    {
        onShiftGun -= ShiftGun;
    }

    public void ShiftGun(int enableIndex)
    {
        gunSystem.ShiftGun();
        curGun = gunDic[gunSystem.curGun.name.Value];
    }
}
