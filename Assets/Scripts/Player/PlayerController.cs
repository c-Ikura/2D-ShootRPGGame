using ShootGame.Gun;
using UnityEngine;
using UnityEngine.InputSystem;
namespace ShootGame
{
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector] public Rigidbody2D playerRb;
        private GunContriller gun;
        private PlayerInputControl inputControl;

        public int speedRang;

        public float a;//加速度
        public float g;//重力加速度
        public float horizontal;

        public bool isSame;
       
        private void Awake()
        {
            inputControl = new PlayerInputControl();
            playerRb = GetComponent<Rigidbody2D>();
            gun = GetComponentInChildren<GunContriller>();
        }
        private void Start()
        {
            EventCenter.OnSwitchGun();
        }
        private void OnEnable()
        {
            inputControl.Enable();
        }

        private void OnDisable()
        {
            inputControl.Disable();
        }

        private void Update()
        {
            Movement();
            Aim();
            SetGravity();
            SwitchGun();
        }
        //移动
        private void Movement()
        {
            horizontal = inputControl.Game.Move.ReadValue<float>();
            

            playerRb.velocity = new Vector2(Mathf.Clamp(playerRb.velocity.x, -speedRang, speedRang), playerRb.velocity.y);

            if (horizontal != 0)
            {
                playerRb.velocity = new Vector2(Mathf.MoveTowards(playerRb.velocity.x, speedRang * horizontal, a), playerRb.velocity.y);
            }
            else
            {
                playerRb.velocity = new Vector2(Mathf.MoveTowards(playerRb.velocity.x, 0, a), playerRb.velocity.y);
            }

            if (playerRb.velocity.x < 0 && horizontal > 0)
            {

                playerRb.AddForce(3 * Vector2.right, ForceMode2D.Impulse);
            }
            if (playerRb.velocity.x > 0 && horizontal < 0)
            {

                playerRb.AddForce(3 * -Vector2.right, ForceMode2D.Impulse);
            }

        }
        //重力
        private void SetGravity()
        {
            if (playerRb.velocity.y < 0)
            {
                playerRb.velocity = Vector2.MoveTowards(playerRb.velocity, new Vector2(playerRb.velocity.x, -15), g * Time.deltaTime);
            }
            if (playerRb.velocity.y > 0)
            {
                playerRb.velocity = Vector2.Lerp(playerRb.velocity, new Vector2(playerRb.velocity.x, 0), g * Time.deltaTime);
            }
        }
        //切换武器
        private void SwitchGun()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                gun.SwitchGun(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gun.SwitchGun(1);
            }
        }
        private void Aim()
        {
            GameObject waepon = gun.gameObject;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(inputControl.Game.Aim.ReadValue<Vector2>());
            mousePos.z = 0;

            //武器转向角度
            Vector2 aimDir = (mousePos - waepon.transform.position).normalized;
            float aimAngle = Vector2.Angle(aimDir, Vector2.up);

            //武器反转
            if (mousePos.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                aimAngle = -aimAngle;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }


            waepon.transform.rotation = Quaternion.Euler(0, 0, aimAngle);

            //判断是否鼠标和人物朝向是否相同
            if (Mathf.Sign(mousePos.x) == Mathf.Sign(transform.localScale.x))
            {
                //同向
                isSame = true;
            }
            else
            {
                //不同向
                isSame = false;
            }


            if (gun.curGun.isCombinable)
            {
                if (Input.GetMouseButton(0))
                {
                    gun.curGun.Shoot(mousePos, playerRb);
                }
                else
                {
                    (gun.curGun as RayGun).isPower = false;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    gun.curGun.Shoot(mousePos, playerRb);

                }
            }

        }
    }


}
