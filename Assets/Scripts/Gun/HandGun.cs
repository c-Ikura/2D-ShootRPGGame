using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HandGun : Gun
{
    public Bullet bullet;
    public Transform shootPoint;

    private void Start()
    {
        
        bullet = Resources.Load<Bullet>("Bullet_Single");       
    }

    protected override void GunShoot(Vector2 shootDir)
    {
        var obj = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
       
    }
}
