using Bronya;
using UnityEngine;

public class Lasergun : Gun
{
    [SerializeField] private GameObject _effect;
    [SerializeField] private GameObject _Laser;
    bool isShooting;    //控制动画切换，这里就要使用到GetButtonDown了
    private LineRenderer laser;
    private float hitRate = 0.1f;
    private float LastHitTime;
    public Rigidbody2D PlayerBody;

    protected override void Start()
    {
        base.Start();
        laser =muzzlePos.GetComponent<LineRenderer>();
    }


    protected override void Shoot()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;
        if (Input.GetButtonDown("Fire1"))
        {
            SoundManager.Instance.PlaySound("laser");
            isShooting = true;
            laser.enabled = true;
            _Laser.SetActive(true);
            _effect.SetActive(true);
        }
        if(Input.GetButtonUp("Fire1"))
        {
            isShooting = false;
            laser.enabled = false;
            _effect.SetActive(false);
        }
        animator.SetBool("Shoot",isShooting);
        if(isShooting)
        {
            Fire();
        }
    }

    protected override void Fire()
    {
        
        //发射一条射线，参数1：枪口位置 参数2：自身指向鼠标位置的方向 参数3：射线发射的最大距离。
        //RayCast函数将返回射线击中的物体的信息，我们拿RaycastHit2D类型对象接受这个返回的值
        RaycastHit2D hit2D =  Physics2D.Raycast(muzzlePos.position, direction, 30000);
        //Debug.DrawLine(muzzlePos.position, hit2D.point,Color.red);
        IDamagable damagable = hit2D.transform.GetComponent<IDamagable>();
        if(damagable!=null)
        {
            if (Time.time > LastHitTime + hitRate)
            {
                int damage = Random.Range(5, 15);
                damagable.TakeDamage(damage);
                Popuptext.Creat(hit2D.point, damage, true);
                LastHitTime = Time.time;        //更新LastHitTime
            }
            
        }

        PlayerBody.AddForce(-direction,ForceMode2D.Impulse);
        KonckBack();

        laser.SetPosition(0, muzzlePos.position);
        laser.SetPosition(1, hit2D.point);
        _effect.transform.position = hit2D.point;
    }


    public void KonckBack()
    {
        PlayerBody.AddRelativeForce(-direction * 10,ForceMode2D.Force);
    }
}
