using QFramework;
using ShootGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCommand : AbstractCommand
{
    public readonly static ShootCommand Single = new ShootCommand();   
    protected override void OnExecute()
    {
        //��ȡ����
        var gunSystem = this.GetSystem<IGunSystem>();
        var curGun = gunSystem.curGun;
        var gunConfig = this.GetModel<IGunConfigModel>().GetGunConfig(curGun.name);
        
        //�޸�����
        if(curGun.name != "HandGun")
        {
            curGun.bulletCount.Value--;
        }        
        curGun.gunState.Value = GunState.Shooting;

        //������
        var timeSystem = this.GetSystem<ITimeSystem>();
        timeSystem.AddTimer(gunConfig.AttackRate, false, () =>
        {
            curGun.gunState.Value = GunState.Idel;
            if (curGun.bulletCount <= 0)
            {
                gunSystem.RemoveGun();
            }
        });
    }
}
