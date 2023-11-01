using QFramework;
using ShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCommand : AbstractCommand
{
    private string gunName;
    public ShootCommand(string gunName)
    {
        this.gunName = gunName;
    }
    protected override void OnExecute()
    {
        var gunSystem = this.GetSystem<IGunSystem>();
        var gunInfo = gunSystem.GetGunInfo(gunName);
        gunInfo.bulletCount.Value--;
    }
}
