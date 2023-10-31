using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour, IGun
{
    public RaycastHit2D hitInfo;
    public LayerMask canHurtLayer;
    public void Shoot(Vector2 dir)
    {
        hitInfo = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, canHurtLayer);

        if (hitInfo.point != Vector2.zero)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.green);
        }

    }
}
