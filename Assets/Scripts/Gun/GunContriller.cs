using System;
using System.Collections.Generic;
using UnityEngine;


public class GunContriller : MonoBehaviour
{
    public IGun curGun;
    public List<IGun> gunList = new List<IGun>();
    public Action<int> onShiftGun;
    private void Start()
    {
        var guns = GetComponentsInChildren<IGun>();

        foreach (var item in guns)
        {
            gunList.Add(item);
        }

        curGun = gunList[0];
    }

    private void OnEnable()
    {
        onShiftGun += ShiftGun;
    }

    public void ShiftGun(int enableIndex)
    {
        if (enableIndex > gunList.Count - 1 || enableIndex < 0)
            return;

        for (var i = 0; i < gunList.Count; i++)
        {
            if (i == enableIndex)
            {
                curGun = gunList[i];
            }
        }
    }
}
