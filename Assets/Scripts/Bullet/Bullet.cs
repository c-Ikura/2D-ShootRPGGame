using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D bulletRb;
    public Vector2 dir;
    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        bulletRb.AddForce(transform.up * 10, ForceMode2D.Impulse);
    }
    private void OnDisable()
    {
        bulletRb.Sleep();
    }

}
