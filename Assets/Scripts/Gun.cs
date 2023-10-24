using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public interface IGun
    {
        /// <summary>
        /// 射击
        /// </summary> <summary>
        /// 
        /// </summary>
        void Shoot(Vector2 dir);

        /// <summary>
        /// 后坐力
        /// </summary>
        void Recoil();
    }

    // public abstract class Gun : MonoBehaviour, IGun
    // {
    //     public virtual void Recoil()
    //     {

    //     }

    //     public virtual void Shoot()
    //     {
    //         Recoil();
    //     }
    // }
}
