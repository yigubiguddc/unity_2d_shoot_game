using UnityEngine;
using Pathfinding;


//寻路算法插件官网（开梯子上去）:https://arongranberg.com/astar/
//The official website of A* function:https://arongranberg.com/astar/

public class BigGirl : LivingEntity      
{
    //[SerializeField] private Transform GirlAnimPerfeb;

    [Header("AI")]
    private Animator _animator;
    public AIPath aiPath;
    public Transform target;
    public Transform parentTransform;


    public LayerMask whatToHit;          //这个习惯好，先声明一个LayerMask，然后去unity里面调整谁会受到攻击
    public float damage = 10f;           //伤害
    public float hitRate = 1f;           //攻速
    private float _lastHit;



    [Header("动画判断")]
    private float previousX;
    private float previousY;


    public void Setup(Transform player, float upgrade)
    {
        target = player;
        aiPath.maxSpeed += upgrade;  //每次升级怪物速度+0.1
        startHealth += upgrade * 100;        //每次加10滴血
        damage += upgrade * 5;
        hitRate += upgrade * 2;
    }


    protected override void Start()
    {
        base.Start();
        previousX = transform.position.x;
        previousY = transform.position.y;

        _animator = GetComponentInParent<Animator>();
        aiPath = GetComponent<AIPath>();  //从子物体中获取
    }


    private void Update()
    {
        //怪物是否抵达终点
        if (target == null)
        {
            //这时候为了避免怪物卡住到不了角色的死亡点，将他们的终点设置为自己的position
            aiPath.destination = transform.position;        //target的gameObject被销毁之后所有寻路ai全部到达终点停止运动
            _animator.SetBool(AnimatorHash.IsMoving, false);
            return;     // 主角死亡
        }

        //动画
        aiPath.destination = target.position;
        if (aiPath.reachedDestination)
        {
            _animator.SetBool(AnimatorHash.IsMoving, false);    //停止运动动画
            SetDirectionAnimation(false, false, false, false);
            if (Time.time > _lastHit + 1 / hitRate)
            {
                Hit();
                _lastHit = Time.time;
            }
        }
        else
        {
            _animator.SetBool(AnimatorHash.IsMoving, true);
        }


        float deltaX = transform.position.x - previousX;
        float deltaY = transform.position.y - previousY;

        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) // 水平移动更明显
        {
            if (deltaX < 0)
            {
                SetDirectionAnimation(true, false, false, false);
            }
            else if (deltaX > 0)
            {
                SetDirectionAnimation(false, true, false, false);
            }
        }
        else // 垂直移动更明显或两者相等
        {
            if (deltaY < 0)
            {
                SetDirectionAnimation(false, false, false, true);
            }
            else if (deltaY > 0)
            {
                SetDirectionAnimation(false, false, true, false);
            }
        }

        previousX = transform.position.x;
        previousY = transform.position.y;

        parentTransform.position = transform.position;    
        transform.localPosition = Vector3.zero;            

        #region test 1
        /*

        float currentX = transform.position.x;
        float currentY = transform.position.y;


        if (currentX < previousX)
        {
            _animator.SetBool(AnimatorHash.faceLeft, true);
            _animator.SetBool(AnimatorHash.faceRight, false);
            _animator.SetBool(AnimatorHash.faceUp, false);
            _animator.SetBool(AnimatorHash.faceDown, false);
        }
        else if (currentX > previousX)
        {
            _animator.SetBool(AnimatorHash.faceRight, true);
            _animator.SetBool(AnimatorHash.faceLeft, false);
            _animator.SetBool(AnimatorHash.faceUp, false);
            _animator.SetBool(AnimatorHash.faceDown, false);
        }  

        if(currentY < previousY)
        {
            _animator.SetBool(AnimatorHash.faceDown, true);
            _animator.SetBool(AnimatorHash.faceUp, false);
            //_animator.SetBool(AnimatorHash.faceLeft, false);
            //_animator.SetBool(AnimatorHash.faceRight, false);
        }
        else if(currentY>previousY)
        {
            _animator.SetBool(AnimatorHash.faceUp, true);
            _animator.SetBool(AnimatorHash.faceDown, false);
           // _animator.SetBool(AnimatorHash.faceLeft, false);
            //_animator.SetBool(AnimatorHash.faceRight, false);
        }



        //数据更新
        previousX = transform.position.x;   
        previousY = transform.position.y;

        parentTransform.position = transform.position;      //父亲（动画实现者）的世界坐标系位置（相对于地图原点）时刻等于子物体（寻路者）的位置
        transform.localPosition = Vector3.zero;             //子物体（寻路者）的相对位置瞬间与父物体重合，接着进行寻路
        */
        #endregion

    }



    //动画选择的函数SetDirectionAnimation
    private void SetDirectionAnimation(bool left, bool right, bool up, bool down)
    {
        _animator.SetBool(AnimatorHash.faceLeft, left);
        _animator.SetBool(AnimatorHash.faceRight, right);
        _animator.SetBool(AnimatorHash.faceUp, up);
        _animator.SetBool(AnimatorHash.faceDown, down);
    }


    //虫豸的进攻
    private void Hit()
    {
        Vector3 selfPosition = transform.position;      //自身位置
        //Diretion to target
        Vector3 targetDirection = (target.position - selfPosition).normalized;  //normalized意思是只保留方向特性
        //攻击仍然是发射射线，只不过不画出来而已
        RaycastHit2D hit2D = Physics2D.Raycast(selfPosition, targetDirection, aiPath.endReachedDistance + 2.0f, whatToHit);
        if (hit2D.collider != null)
        {
            //Debug.Log("$hitPlayer{ hit2D.transform.name}");     //受到攻击者的名字
            IDamagable damageable = hit2D.transform.GetComponent<IDamagable>();
            damageable?.TakeDamage(damage);
        }
    }
}
