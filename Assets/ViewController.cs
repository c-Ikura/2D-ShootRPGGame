using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class ViewController : MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return Global._interface;
    }


}
