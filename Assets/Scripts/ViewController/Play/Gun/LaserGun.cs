using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Gun
{
    public RaycastHit2D hitInfo;
    public LayerMask canHurtLayer;

    protected override void GunShoot(Vector2 shootDir)
    {
        hitInfo = Physics2D.Raycast(transform.position, shootDir, Mathf.Infinity, canHurtLayer);

        if (hitInfo.point != Vector2.zero)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.green);
        }
    }
}
