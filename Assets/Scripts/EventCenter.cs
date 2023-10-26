using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter : MonoBehaviour
{
    public static Action onSwitchGun;
    public static void OnSwitchGun()
    {
        onSwitchGun?.Invoke();
    }
}
