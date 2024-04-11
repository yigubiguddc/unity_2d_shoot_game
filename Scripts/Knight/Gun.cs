using UnityEngine;
using System;
//����GunΪ���л��� �����԰����е���private�ı�����������protected���ͣ�Ȼ�󽫺���Ҳ������protected����
//������������к���������virtual����Ϊ�麯����ʹ��ǹе�в�ͬ����Ϊʱ���Խ�����д��

public class Gun: MonoBehaviour
{
    [SerializeField] private LivingEntity player;
    public float interval;
    public GameObject bulletPrefab;     //�����
    public GameObject shellPrefab;
    protected Transform muzzlePos;
    protected Transform shellPos;
    protected Vector2 mousePos;
    protected Vector2 direction;
    protected float timer;
    protected float flipY;
    protected Animator animator;

    private PlayerData pd;
    [Header("��������")]
    public LayerMask whatToHit;
    private int  TMPlevel = 2;



    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        muzzlePos = transform.Find("Muzzle");
        shellPos = transform.Find("BulletShell");
        flipY = transform.localScale.y;
    }

    protected virtual void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(flipY, -flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);
        Shoot();

        //level up and shoot speed up
        /*if (player.level == TMPlevel)
        {
            if(player.level == TMPlevel)
            {
                IntervalDown(interval);
            }
            else if(player.level> TMPlevel)
            {
                int count = player.level - TMPlevel;
                while(count>0)
                {
                    IntervalDown(interval);
                }
                TMPlevel = player.level + 1;

            }
        }*/
        if (player.level == TMPlevel)
        {
            IntervalDown(ref interval);
        }
        else if (player.level > TMPlevel)
        {
            int count = player.level - TMPlevel;
            while (count > 0)
            {
                IntervalDown(ref interval);
                count--;
            }
            TMPlevel = player.level + 1;
        }
    }
    public void IntervalDown(ref float interval)
    {
        if (interval > 0.1f)
        {
            interval -= 0.05f;
        }
        else if (interval < 0.1f && interval > 0)
        {
            interval -= 0.001f;
        }
        else if (Math.Abs(interval) < 0.0001f)  // ��interval�ӽ�0ʱ
        {
            interval = 0;
            // ��������
        }
        TMPlevel++;
    }
    /* public void IntervalDown(float interval)
     {
         if (interval > 0.1f)
         {
             interval -= 0.1f;
             TMPlevel++;
         }
         else if (interval < 0.1f)
         {
             interval -= 0.01f;
             TMPlevel++;
         }
         else if (interval == 0)
         {
             interval = 0;
             //��������
             TMPlevel++;
         }
     }*/
    private void BigBullet()
    {
        Debug.Log("���ٴﵽ���ޣ������ӵ����ģʽ");
        //����д�ˣ��ڣ� 23:13 2023/10/4

    }
    protected virtual void Shoot()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;                //��ʵ���˸������ָ��

        if (Input.GetButton("Fire1"))
        {
            if (timer != 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 0;
                }
            }
            if (timer == 0)
            {
                Fire();
                timer = interval;
            }
        }
    }
    protected virtual void Fire()
    {
        animator.SetTrigger("Shoot");
        //GameObject bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);    //����bulletPrefabԤ����
        bullet.transform.position = muzzlePos.position;
        float angle =UnityEngine.Random.Range(-5f, 5f);

        //����Z�ᣨָ�����ڣ���ת-5�� -- 5��ķ�Χ��ģ��ƫת�Ƕȡ�
        bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(angle, Vector3.forward) * direction);  //����׼���򴫽�ȥ��SetSpeed����Vector2���������������ﴫ��
        GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;
    }
}