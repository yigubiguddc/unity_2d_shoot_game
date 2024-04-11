using UnityEngine;
using System;
//设置Gun为所有基类 ，所以把所有的是private的变量都声明成protected类型，然后将函数也声明成protected类型
//最后把这里的所有函数都加上virtual声明为虚函数，使得枪械有不同的行为时可以进行重写。

public class Gun: MonoBehaviour
{
    [SerializeField] private LivingEntity player;
    public float interval;
    public GameObject bulletPrefab;     //对象池
    public GameObject shellPrefab;
    protected Transform muzzlePos;
    protected Transform shellPos;
    protected Vector2 mousePos;
    protected Vector2 direction;
    protected float timer;
    protected float flipY;
    protected Animator animator;

    private PlayerData pd;
    [Header("武器属性")]
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
        else if (Math.Abs(interval) < 0.0001f)  // 当interval接近0时
        {
            interval = 0;
            // 射速上限
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
             //射速上限
             TMPlevel++;
         }
     }*/
    private void BigBullet()
    {
        Debug.Log("射速达到上限，开启子弹变大模式");
        //懒得写了，摆！ 23:13 2023/10/4

    }
    protected virtual void Shoot()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;                //简单实现了跟随鼠标指针

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
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);    //传入bulletPrefab预设体
        bullet.transform.position = muzzlePos.position;
        float angle =UnityEngine.Random.Range(-5f, 5f);

        //绕着Z轴（指向画面内）旋转-5° -- 5°的范围来模拟偏转角度。
        bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(angle, Vector3.forward) * direction);  //把瞄准方向传进去，SetSpeed返会Vector2向量，方向再这里传入
        GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;
    }
}