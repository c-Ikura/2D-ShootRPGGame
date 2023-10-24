using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public class AnimController : MonoBehaviour
    {
        private Animator playerAnim;
        private PlayerController playerController;

        private void Awake()
        {
            playerAnim = GetComponent<Animator>();
            playerController = GetComponentInParent<PlayerController>();
        }

        private void Update()
        {
            SetAnimator();
        }

        private void SetAnimator()
        {
            playerAnim.SetFloat("InputX", Mathf.Abs(playerController.playerRb.velocity.x));
            playerAnim.SetBool("isSame", playerController.isSame);
        }
    }
}

