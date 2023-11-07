using QFramework;
using ShootGame;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HandGun : Gun
{
    public Bullet bullet1;
    public Transform shootPoint;
    public IGameObjectPool pool;

    private void Start()
    {
        pool = this.GetSystem<IGameObjectPool>();
        bullet1 = Resources.Load<Bullet>("Bullet_Single");       
    }

    protected override void GunShoot(Vector2 shootDir)
    {
        var bullet = pool.Get("Bullet_Single");
        bullet.transform.rotation = shootPoint.rotation;
        bullet.transform.position = shootPoint.position;
    }
}
