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


    [Header("�����ж�")]
    private float previousX;
    private float previousY;


    public void Setup(Transform player, float upgrade)
    {
        target = player;
        aiPath.maxSpeed += upgrade;  //ÿ�����������ٶ�+0.1
        startHealth += upgrade * 100;        //ÿ�μ�10��Ѫ
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
        //�����Ƿ�ִ��յ�
        if (target == null)
        {
            //��ʱ��Ϊ�˱�����￨ס�����˽�ɫ�������㣬�����ǵ��յ�����Ϊ�Լ���position
            aiPath.destination = transform.position;        //target��gameObject������֮������Ѱ·aiȫ�������յ�ֹͣ�˶�
            _animator.SetBool(AnimatorHash.IsMoving, false);
            _animator.SetBool("attack", false);
            return;     // ��������
        }

        

        //����
        aiPath.destination = target.position;
        if (aiPath.reachedDestination)
        {
            _animator.SetBool(AnimatorHash.IsMoving, false);    //ֹͣ�˶�����
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

    //����
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


