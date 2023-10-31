using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnim : MonoBehaviour
{
    private Animator bulletAnim;
    private void Awake()
    {
        bulletAnim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //bulletAnim.SetTrigger("isBoom");
    }
}
