using UnityEngine;
using Pathfinding;
using System.Collections;

public class Boss1 : LivingEntity
{
    //[SerializeField] private Transform GirlAnimPerfeb;

    [Header("AI")]
    private Animator _animator;
    public AIPath aiPath;
    public Transform target;
    public Transform parentTransform;


    public LayerMask whatToHit;
    public float damage = 10f;
    public float hitRate = 1f;
    private float _lastHit;

    private float tempspeed;


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
        tempspeed = aiPath.maxSpeed;
        previousX = transform.position.x;
        previousY = transform.position.y;

        _animator = GetComponentInParent<Animator>();
        aiPath = GetComponent<AIPath>();
    }


    private void Update()
    {
        //怪物是否抵达终点
        if (target == null)
        {
            //这时候为了避免怪物卡住到不了角色的死亡点，将他们的终点设置为自己的position
            aiPath.destination = transform.position;        //target的gameObject被销毁之后所有寻路ai全部到达终点停止运动
            _animator.SetBool(AnimatorHash.IsMoving, false);
            _animator.SetBool("attack", false);
            return;     // 主角死亡
        }

        

        //动画
        aiPath.destination = target.position;
        if (aiPath.reachedDestination)
        {
            _animator.SetBool(AnimatorHash.IsMoving, false);    //停止运动动画
            if (Time.time > _lastHit + 1 / hitRate)
            {
                _animator.SetBool("attack", true);
                Hit();
                _lastHit = Time.time;
            }   
        }
        else
        {
            _animator.SetBool(AnimatorHash.IsMoving, true);
            _animator.SetBool("attack", false);
        }


        float deltaX = transform.position.x - previousX;
        if (deltaX < 0)
        {
            parentTransform.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (deltaX > 0)
        {
            parentTransform.transform.localScale = new Vector3(1, 1, 1);
        }
        previousX = transform.position.x;

        parentTransform.position = transform.position;
        transform.localPosition = Vector3.zero;



        //Enemy Boss Dash Code
        if(Physics2D.OverlapCircle(transform.position,7.0f,LayerMask.GetMask("Player")))
        {
            //Debug.Log("Time to hit THE player!");
            aiPath.maxSpeed = 100;
            StartCoroutine(SpeedDown());
            aiPath.maxSpeed = tempspeed;
        }
        
        IEnumerator SpeedDown()
        {
            yield return new WaitForSeconds(1.0f);
        }

    }

    //进攻
    private void Hit()
    {
        ApplyDamageToTarget(target);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ApplyDamageToTarget(other.transform);
    }

    void ApplyDamageToTarget(Transform target)
    {
        IDamagable damageable = target.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}


