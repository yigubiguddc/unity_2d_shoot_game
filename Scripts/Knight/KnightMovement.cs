using UnityEngine;
using UnityEngine.UI;
using Bronya;

public class KnightMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Vector2 input;
    private Vector2 mousePosition;
    private int gunNumber;
    public GameObject[] guns;   //枪支数组

    [Header("UI's component")]
    public Image cdImage;


    [Header("Dash")]
    public float dashTime;
    private float dashTimeLeft;
    public bool isDashing;   //冲锋!
    private float lastDash = -10f;
    public float dashCoolDown;
    public float dashSpeed;
    public GameObject shadowPrefab;

    private float lastSoundPlay = -2f; // Initialize with -2 seconds to allow immediate play on game start
    private float soundCoolDown = 2f;  // 2 seconds cooldown


    private void Awake()
    {
        GameEvents.GameOver += gameOver;
    }


    protected void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        guns[0].SetActive(true);        //激活第一个枪械作为初始枪械
    }

    private void Update()
    {
        SwitchGun();    //随时待命
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        _rigidbody.velocity = input.normalized * moveSpeed;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //转身方法一：调整transform.scale的x的值为+1和-1即可
        /*if(mousePosition.x > transform.position.x)
        {
            Vector3 scale = new Vector3(1, 1, 1);
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = new Vector3(-1, 1, 1);
            transform.localScale = scale;
        }*/
        //转身方法二：让物体绕Y轴旋转180°  
        if (mousePosition.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        if(input!= Vector2.zero)
        {
            _animator.SetBool(AnimatorHash.IsMoving, true);
        }
        else
        {
            _animator.SetBool(AnimatorHash.IsMoving, false);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time >= (lastSoundPlay + soundCoolDown))
            {
                SoundManager.Instance.PlaySound("dash");
                lastSoundPlay = Time.time;  // Update the last sound play time
            }
            if (Time.time >= (lastDash + dashCoolDown))
            {
                //READY TO DASH
                ReadyToDash();
            }
        }
        cdImage.fillAmount -= 1.0f / dashCoolDown * Time.deltaTime;
        //game pause,by use of timescale
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameEvents.GamePause?.Invoke();
        }
    } 

    void SwitchGun()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            guns[gunNumber].SetActive(false);
            if(--gunNumber<0)       //gunNumber为0时做得到这点，--0 == -1，则gunNumber直接变成可以到达的最大值
            {
                gunNumber = guns.Length - 1;
            }
            guns[gunNumber].SetActive(true);
            Debug.Log($"The gun {gunNumber} has been setactive!");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            guns[gunNumber].SetActive(false);
            if (++gunNumber > guns.Length - 1)
            {
                gunNumber = 0;
            }
            guns[gunNumber].SetActive(true);
           // Debug.Log($"The gun {gunNumber} has been setactive!");
        }
    }

    private void gameOver()
    {
        moveSpeed = 0;
        
    }

    #region DASH
    private void FixedUpdate()
    {
        Dash();
    }

    private void ReadyToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        cdImage.fillAmount = 1;
    }

    private void Dash()
    {
        if(isDashing)
        {
            if(dashTimeLeft > 0)
            {

                //移动控制
                float moveX = Input.GetAxis("Horizontal");
                float moveY = Input.GetAxis("Vertical");
                Vector2 dashDirection = new Vector2(moveX, moveY).normalized;

                _rigidbody.velocity = dashDirection * dashSpeed;
                dashTimeLeft -= Time.deltaTime;
                //
                ObjectPool.Instance.GetObject(shadowPrefab);
            }
            if(dashTimeLeft<=0)
            {
                isDashing = false;
            }
        }
    }
    #endregion

}


