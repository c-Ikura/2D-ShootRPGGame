using QFramework;
using ShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnim : ViewController
{
    private Animator bulletAnim;
    private void Awake()
    {
        bulletAnim = GetComponent<Animator>();        
    }
   
    
}

