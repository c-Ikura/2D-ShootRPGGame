using System.Collections;
using System.Collections.Generic;
using QFramework;
using ShootGame;
using UnityEngine;

public class Global : Architecture<Global>
{
    protected override void Init()
    {
        RegisterSystem<IGunSystem>(new GunSystem());
    }
}
