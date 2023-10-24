using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
namespace ShootGame
{
    public class Gun_Musket : MonoBehaviour, IGun
    {
        public Sprite aimSprite;

        private RaycastHit2D hitInfo;
        private LayerMask shootLayer;
        private void Start()
        {
            shootLayer = 1 << 6;
        }
        public void Recoil()
        {
            print("后坐力");
        }

        public void Shoot(Vector2 dir)
        {
            Recoil();

            //var shootPoint = transform.position + new Vector3(0.7f, 0, 0);

            hitInfo = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, shootLayer);
            Debug.DrawLine(transform.position, hitInfo.point, Color.red, 1000);
            Debug.DrawRay(transform.position, dir * 1000, Color.green);

        }
    }
}

