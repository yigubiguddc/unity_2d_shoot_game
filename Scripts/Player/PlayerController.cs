using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//添加Rigidboty2D的一种方式，如果手动添加的话这句不会生效
[RequireComponent(typeof(Rigidbody2D))] 

public class PlayerController : MonoBehaviour
{
    private float _moveSpeed = 1.0f;
    public Vector2 _moveDirection;
    private Rigidbody2D _rigidbody2D;
    private Camera _mainCamera;
    private PlayerGun _playergun;
    //字符串查找效率低下，所以使用不一样的方法进行管理
    [SerializeField] private Transform playerBody;
    [SerializeField] private Animator _animator;


    public void Init(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            Debug.LogError("Can't find main camera, please check it");  //打印日志查错，质保报错，双击就可以到达这里
        }
        _playergun = GetComponentInChildren<PlayerGun>();   //从子类物体身上获取
    }

    public void Update()
    {

        //移动
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveDirection *= _moveSpeed;
        //动画切换
        _animator.SetFloat( AnimatorHash.MoveSpeed , Mathf.Abs(_moveDirection.x)+Mathf.Abs(_moveDirection.y));   //为了动画的切换为MoveSpeed赋值，之所以写两个绝对值相加就是为了斜着移动的时候不出问题
        //转向和瞄准
        Vector3 direction = Input.mousePosition - _mainCamera.WorldToScreenPoint(playerBody.position); ;     //这是向量相减，表示速度大小和朝向，总体来说方向就是Player到鼠标坐标的方向
        //上面的后半段_mainCamera的操作是借用_mainCamera的功能将Player的世界坐标转为屏幕坐标
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;    //angle
        playerBody.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        //尝试平滑旋转
        //float speed = 0.01f; 
        //Quaternion targetRotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed);


        //输入
        if (Input.GetMouseButton(0))
        {
            //扣动扳机
            _playergun.OnTriggerHold();
            //_animator.SetBool(AnimatorHash.isShooting, true);
        }
        if(Input.GetMouseButtonUp(0))   //这里有个Up....之前看掉了
        {
            //松开扳机GetMouseButtonUp
            _playergun.OnTriggerRelease();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //换子弹
            _playergun.Reload();
        }

    }

    //
    public void FixedUpdate()
    {
        _rigidbody2D.AddForce(_moveDirection, ForceMode2D.Impulse);//AddForce 方法用于在运行时给物体施加力。ForceMode2D.Impulse 是一个枚举值，表示施加的力的类型为冲量。冲量是一个瞬时的力，会立即改变物体的速度。4

    }

    public void KnockBack()
    {
        //反面教材_rigidbody2D.AddRelativeForce(-transform.up * 180,ForceMode2D.Force);//添加力的方式更加真实
        //要时刻记住，我们把主角的整个物体和主角的身体做出了区分，现在要转动的只有PlayerBody  But not the Assault!
        _rigidbody2D.AddRelativeForce(-playerBody.up * 180, ForceMode2D.Force);
    }

    public void SetShootingState(bool shootState)
    {
        _animator.SetBool(AnimatorHash.isShooting, shootState);//开火配置首先为false

    }

}
