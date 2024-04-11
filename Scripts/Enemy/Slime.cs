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
    [Header("�����ж�")]
    private float previousX;
    private float previousY;

    public void Setup(Transform player, float upgrade)
    {
        target = player;
        aiPath.maxSpeed += upgrade;  //ÿ�����������ٶ�Up
        startHealth += upgrade * 100;        //ÿ�μ�10��Ѫ
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
        //�����Ƿ�ִ��յ�
        if (target == null)
        {
            //��ʱ��Ϊ�˱�����￨ס�����˽�ɫ�������㣬�����ǵ��յ�����Ϊ�Լ���position
            aiPath.destination = transform.position;        //target��gameObject������֮������Ѱ·aiȫ�������յ�ֹͣ�˶�
            _animator.SetBool(AnimatorHash.IsMoving, false);
            return;     // ��������
        }
        //����
        aiPath.destination = target.position;
        if (aiPath.reachedDestination)
        {
            _animator.SetBool(AnimatorHash.IsMoving, false);    //ֹͣ�˶�����
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


