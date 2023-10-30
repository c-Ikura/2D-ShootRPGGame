using ShootGame.Gun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunContriller : MonoBehaviour
{
    [SerializeField] private Gun[] guns;

    public Gun curGun;
    private void Awake()
    {
        guns = GetComponentsInChildren<Gun>();    
    }
    private void Start()
    {
        foreach (var gun in guns)
        {
            gun.gameObject.SetActive(false);
        }

        SwitchGun(0);
    }

    public void SwitchGun(int gunIndex)
    {
        
        for (int i = 0; i < guns.Length; i++)
        {
            if(i == gunIndex)
            {
                curGun = guns[gunIndex];
                curGun.gameObject.SetActive(true);
                continue;
            }
            guns[i].gameObject.SetActive(false);
        }
    }
}
