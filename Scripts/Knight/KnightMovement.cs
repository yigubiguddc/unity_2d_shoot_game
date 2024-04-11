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
    public GameObject[] guns;   //ǹ֧����

    [Header("UI's component")]
    public Image cdImage;


    [Header("Dash")]
    public float dashTime;
    private float dashTimeLeft;
    public bool isDashing;   //���!
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
        guns[0].SetActive(true);        //�����һ��ǹе��Ϊ��ʼǹе
    }

    private void Update()
    {
        SwitchGun();    //��ʱ����
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        _rigidbody.velocity = input.normalized * moveSpeed;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //ת����һ������transform.scale��x��ֵΪ+1��-1����
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
        //ת����������������Y����ת180��  
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
            if(--gunNumber<0)       //gunNumberΪ0ʱ���õ���㣬--0 == -1����gunNumberֱ�ӱ�ɿ��Ե�������ֵ
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

                //�ƶ�����
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


