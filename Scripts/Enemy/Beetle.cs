using UnityEngine;
using Pathfinding;


//Ѱ·�㷨�����������������ȥ��:https://arongranberg.com/astar/
//The official website of A* function:https://arongranberg.com/astar/

public class Beetle : LivingEntity      //�̳�����������������
{
    [Header("AI")]
    private Animator _animator;
    public AIPath aiPath;
    public Transform target;
    public LayerMask whatToHit;        //���ϰ�ߺã�������һ��LayerMask��Ȼ��ȥunity�������˭���ܵ�����
    public float damage = 10.0f;        //�˺�
    public float hitRate = 1.0f;       //����
    private float _lastHit;

    public void Setup(Transform player,float upgrade)
    {
        target = player;
        aiPath.maxSpeed += upgrade; //ÿ�����������ٶ�+0.1
        startHealth += upgrade * 100;        //ÿ�μ�10��Ѫ
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
        //�����Ƿ�ִ��յ�
        if(target == null)
        {
            //��ʱ��Ϊ�˱�����￨ס�����˽�ɫ�������㣬�����ǵ��յ�����Ϊ�Լ���position
            aiPath.destination = transform.position;
            _animator.SetBool(AnimatorHash.IsMoving,false);
            return;     // ��������
        }

        aiPath.destination = target.position;
        if(aiPath.reachedDestination)
        {
            _animator.SetBool(AnimatorHash.IsMoving, false);    //ֹͣ�˶�����
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

    //�����Ľ���
    /*private void Hit()
    {
        Vector3 selfPosition = transform.position;      //����λ��
        //Diretion to target
        Vector3 targetDirection = (target.position - selfPosition).normalized;  //��˼��ֻ������������

        Debug.DrawLine(selfPosition, target.transform.position);
        //������Ȼ�Ƿ������ߣ�ֻ����������������

        RaycastHit2D hit2D = Physics2D.Raycast(selfPosition, targetDirection, aiPath.endReachedDistance + 2100.0f, whatToHit);

        if (hit2D.collider!= null)
        {
            Debug.Log("$hitPlayer{ hit2D.transform.name}");     //�ܵ������ߵ�����
            IDamagable damageable = hit2D.transform.GetComponent<IDamagable>();
            damageable?.TakeDamage(damage);
        }
    }*/
    /*private void Hit()
    {
        Vector3 selfPosition = transform.position;      //����λ��
        //Diretion to target
        Vector3 targetDirection = (target.position - selfPosition).normalized;  //��˼��ֻ������������

        Debug.DrawLine(selfPosition, target.transform.position);
        //������Ȼ�Ƿ������ߣ�ֻ����������������

        RaycastHit2D hit2D = Physics2D.Raycast(selfPosition, targetDirection, aiPath.endReachedDistance + 2.0f, whatToHit);

        if (hit2D.collider != null)
        {
            Debug.Log("$hitPlayer{ hit2D.transform.name}");     //�ܵ������ߵ�����
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
