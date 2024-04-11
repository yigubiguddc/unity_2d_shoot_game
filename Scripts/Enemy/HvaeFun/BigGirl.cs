using UnityEngine;
using Pathfinding;


//Ѱ·�㷨�����������������ȥ��:https://arongranberg.com/astar/
//The official website of A* function:https://arongranberg.com/astar/

public class BigGirl : LivingEntity      
{
    //[SerializeField] private Transform GirlAnimPerfeb;

    [Header("AI")]
    private Animator _animator;
    public AIPath aiPath;
    public Transform target;
    public Transform parentTransform;


    public LayerMask whatToHit;          //���ϰ�ߺã�������һ��LayerMask��Ȼ��ȥunity�������˭���ܵ�����
    public float damage = 10f;           //�˺�
    public float hitRate = 1f;           //����
    private float _lastHit;



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
        previousX = transform.position.x;
        previousY = transform.position.y;

        _animator = GetComponentInParent<Animator>();
        aiPath = GetComponent<AIPath>();  //���������л�ȡ
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

        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) // ˮƽ�ƶ�������
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
        else // ��ֱ�ƶ������Ի��������
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



        //���ݸ���
        previousX = transform.position.x;   
        previousY = transform.position.y;

        parentTransform.position = transform.position;      //���ף�����ʵ���ߣ�����������ϵλ�ã�����ڵ�ͼԭ�㣩ʱ�̵��������壨Ѱ·�ߣ���λ��
        transform.localPosition = Vector3.zero;             //�����壨Ѱ·�ߣ������λ��˲���븸�����غϣ����Ž���Ѱ·
        */
        #endregion

    }



    //����ѡ��ĺ���SetDirectionAnimation
    private void SetDirectionAnimation(bool left, bool right, bool up, bool down)
    {
        _animator.SetBool(AnimatorHash.faceLeft, left);
        _animator.SetBool(AnimatorHash.faceRight, right);
        _animator.SetBool(AnimatorHash.faceUp, up);
        _animator.SetBool(AnimatorHash.faceDown, down);
    }


    //�����Ľ���
    private void Hit()
    {
        Vector3 selfPosition = transform.position;      //����λ��
        //Diretion to target
        Vector3 targetDirection = (target.position - selfPosition).normalized;  //normalized��˼��ֻ������������
        //������Ȼ�Ƿ������ߣ�ֻ����������������
        RaycastHit2D hit2D = Physics2D.Raycast(selfPosition, targetDirection, aiPath.endReachedDistance + 2.0f, whatToHit);
        if (hit2D.collider != null)
        {
            //Debug.Log("$hitPlayer{ hit2D.transform.name}");     //�ܵ������ߵ�����
            IDamagable damageable = hit2D.transform.GetComponent<IDamagable>();
            damageable?.TakeDamage(damage);
        }
    }
}
