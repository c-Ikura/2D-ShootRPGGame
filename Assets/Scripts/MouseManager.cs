using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public class MouseManager : MonoBehaviour
    {
        public Texture2D shootCursor;

        private void Start()
        {

        }
        private void Update()
        {
            SwitchCursorSprite();
        }

        private void SwitchCursorSprite()
        {
            Cursor.SetCursor(shootCursor, Vector2.zero, CursorMode.Auto);
        }


    }
}

