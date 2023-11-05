using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public class MouseManager : MonoBehaviour
    {
        public Texture2D shootCursor;
        public static Vector3 mousePos;

        private void Start()
        {
            SwitchCursorSprite();
        }
        private void Update()
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
        }
        private void SwitchCursorSprite()
        {
            Cursor.SetCursor(shootCursor, Vector2.zero, CursorMode.Auto);
        }


    }
}

