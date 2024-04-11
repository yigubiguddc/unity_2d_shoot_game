using UnityEngine;
using Pathfinding;
using Unity.Collections;
public class Slime : LivingEntity
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
    [Header("动画判断")]
    private float previousX;
    private float previousY;

    public void Setup(Transform player, float upgrade)
    {
        target = player;
        aiPath.maxSpeed += upgrade;  //每次升级怪物速度Up
        startHealth += upgrade * 100;        //每次加10滴血
        damage += 1;
        hitRate += upgrade * 2;
    }

    protected override void Start()
    {
        base.Start();
        aiPath.maxSpeed = Random.Range(2, 6);
        previousX = transform.position.x;
        previousY = transform.position.y;
        transform.SetParent(parentTransform);
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
            return;     // 主角死亡
        }
        //动画
        aiPath.destination = target.position;
        if (aiPath.reachedDestination)
        {
            _animator.SetBool(AnimatorHash.IsMoving, false);    //停止运动动画
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


        //The most easy way to deal with this problem...
        if(parentTransform.gameObject.layer == LayerMask.NameToLayer("Stone"))
        {
            if (deltaX < 0)
            {
                parentTransform.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (deltaX > 0)
            {
                parentTransform.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            if (deltaX < 0)
            {
                parentTransform.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (deltaX > 0)
            {
                parentTransform.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        


        previousX = transform.position.x;
        //Update the positon of the previousX
        parentTransform.position = transform.position;
        transform.localPosition = Vector3.zero;
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


