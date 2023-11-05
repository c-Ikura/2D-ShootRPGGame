using QFramework;
using ShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUi : ViewController
{
    public IGunSystem gunSystem;
    public IGunConfigModel gunConfigModel;

    public GUIStyle labelSkin;
    private void Awake()
    {
        gunSystem = this.GetSystem<IGunSystem>();
        gunConfigModel = this.GetModel<IGunConfigModel>();
    }
    private void Start()
    {

        labelSkin = new GUIStyle()
        {
            fontSize = 24,
        };

    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 100), "��ǰǹ " + gunSystem.curGun.name, labelSkin);
        GUI.Label(new Rect(10, 50, 100, 100), "��ǰǹ�ӵ� " + gunSystem.curGun.bulletCount.Value, labelSkin);
        GUI.Label(new Rect(10, 90, 100, 100), "��ǰǹ״̬ " + gunSystem.curGun.gunState.Value, labelSkin);
        GUI.Label(new Rect(10, 130, 100, 100), "��ǰЯ��ǹ���� " + (gunSystem as GunSystem).gunList.Count, labelSkin);
    }


}
