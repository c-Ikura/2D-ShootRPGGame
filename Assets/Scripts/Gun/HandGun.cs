using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HandGun : MonoBehaviour, IGun
{
    public Bullet bullet;
    public Transform shootPoint;

    private void Awake()
    {
        bullet = Resources.Load<Bullet>("Bullet_Single");
    }
    public void Shoot(Vector2 shootPos)
    {
        var obj = Instantiate(bullet, shootPoint.position, transform.rotation);
        bullet.dir = shootPos;

    }
}
