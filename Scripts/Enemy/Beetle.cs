using UnityEngine;
using Pathfinding;


//寻路算法插件官网（开梯子上去）:https://arongranberg.com/astar/
//The official website of A* function:https://arongranberg.com/astar/

public class Beetle : LivingEntity      //继承自有生命的物体类
{
    [Header("AI")]
    private Animator _animator;
    public AIPath aiPath;
    public Transform target;
    public LayerMask whatToHit;        //这个习惯好，先声明一个LayerMask，然后去unity里面调整谁会受到攻击
    public float damage = 10.0f;        //伤害
    public float hitRate = 1.0f;       //攻速
    private float _lastHit;

    public void Setup(Transform player,float upgrade)
    {
        target = player;
        aiPath.maxSpeed += upgrade; //每次升级怪物速度+0.1
        startHealth += upgrade * 100;        //每次加10滴血
        damage += 2.0f;
        hitRate += upgrade * 2;           
    }

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
    }

    private void Update()
    {
        //怪物是否抵达终点
        if(target == null)
        {
            //这时候为了避免怪物卡住到不了角色的死亡点，将他们的终点设置为自己的position
            aiPath.destination = transform.position;
            _animator.SetBool(AnimatorHash.IsMoving,false);
            return;     // 主角死亡
        }

        aiPath.destination = target.position;
        if(aiPath.reachedDestination)
        {
            _animator.SetBool(AnimatorHash.IsMoving, false);    //停止运动动画
            if(Time.time>_lastHit + 1/hitRate)
            {
                Hit();
                _lastHit = Time.time;
            }
        }
        else
        {
            _animator.SetBool(AnimatorHash.IsMoving,true);
        }
    }

    //虫豸的进攻
    /*private void Hit()
    {
        Vector3 selfPosition = transform.position;      //自身位置
        //Diretion to target
        Vector3 targetDirection = (target.position - selfPosition).normalized;  //意思是只保留方向特性

        Debug.DrawLine(selfPosition, target.transform.position);
        //攻击仍然是发射射线，只不过不画出来而已

        RaycastHit2D hit2D = Physics2D.Raycast(selfPosition, targetDirection, aiPath.endReachedDistance + 2100.0f, whatToHit);

        if (hit2D.collider!= null)
        {
            Debug.Log("$hitPlayer{ hit2D.transform.name}");     //受到攻击者的名字
            IDamagable damageable = hit2D.transform.GetComponent<IDamagable>();
            damageable?.TakeDamage(damage);
        }
    }*/
    /*private void Hit()
    {
        Vector3 selfPosition = transform.position;      //自身位置
        //Diretion to target
        Vector3 targetDirection = (target.position - selfPosition).normalized;  //意思是只保留方向特性

        Debug.DrawLine(selfPosition, target.transform.position);
        //攻击仍然是发射射线，只不过不画出来而已

        RaycastHit2D hit2D = Physics2D.Raycast(selfPosition, targetDirection, aiPath.endReachedDistance + 2.0f, whatToHit);

        if (hit2D.collider != null)
        {
            Debug.Log("$hitPlayer{ hit2D.transform.name}");     //受到攻击者的名字
            IDamagable damageable = hit2D.transform.GetComponent<IDamagable>();
            damageable?.TakeDamage(damage);
        }
        Debug.Log("Hit function called!");
    }*/
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
