using System.Collections;
using UnityEngine;
using TMPro;
using Bronya;


public class PlayerGun : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform firePoint;   //开火位置
    [SerializeField] private Transform shootTrail;
    [SerializeField] private Transform muzzleFlash;
    [SerializeField] private Transform hitParticle;
    [SerializeField] private TMP_Text remainingBulletText;
    private string remainingBulletCount => (playerData.ClipSize - shootNumber).ToString();
    private PlayerData playerData;

    [Header("武器属性")]
    public LayerMask whatToHit;
    public FireMode fireMode = FireMode.Burst;  //连发属性
    public bool isReloading;
    public float nextShootTime; //射击cd
    public int shootNumber; //射了多少

    public bool isTriggerReleased;




    public void Init(PlayerData pd)
    {
        playerData = pd;
        remainingBulletText.SetText(remainingBulletCount);
        if(fireMode == FireMode.Single)
        {
            isTriggerReleased = true;
        }
    }


    //抽象和封装
    #region 扳机扣动和释放
    public void OnTriggerHold()
    {
        //按住扳机
        if (isReloading) return;    //正在换子弹，不做判断

        if(shootNumber>=playerData.ClipSize)   //子弹数量耗尽，不做判断
        {
            //Reloading装弹
            StartCoroutine(ReloadCorutine());//调用协程函数
            return;
        }

        //通过选择的射击模式进行switch
        switch (fireMode)
        {
            //单发Mode
            case FireMode.Single:
                {
                    playerController.SetShootingState(false);
                    if (!isTriggerReleased) return;
                    Shoot();
                    isTriggerReleased = false;
                    break;
                }
               
            //连射Mode
            case FireMode.Burst:
                {
                    if (Time.time < nextShootTime)
                    {
                        playerController.SetShootingState(false);
                        return;
                    }
                    nextShootTime = Time.time + 1 /(float) playerData.FireRate;    //开火间隙时间，控制射速
                    Shoot();
                    break;
                }
        }
    }

    public void OnTriggerRelease()
    {
        //松开扳机
        Debug.Log("松开扳机");
        isTriggerReleased = true;   //单发逻辑
        playerController.SetShootingState(shootState: false);   //松开扳机，立刻停止射击动画
    }

    #endregion

    #region 装弹部分
    public void Reload()
    {
        //换蛋
        if(shootNumber!=0)      //这个必须有！
        {
            StartCoroutine(ReloadCorutine());//调用协程函数
        }
        else
        {
            return;
        }
        
    }

    //协程reload
    private IEnumerator ReloadCorutine()
    {
        //TODO Sound Animation...
        Debug.Log("换蛋");
        isReloading = true; 
        playerController.SetShootingState(shootState: false);   //换蛋时间禁止抖动（主要是为了解决换蛋期间不松开左键角色仍然抽搐的行为）
        //换蛋音效
        SoundManager.Instance.PlaySound("Assault Reload1");



        yield return new WaitForSeconds(playerData.ReloadTime);      //换蛋时间
        playerController.SetShootingState(shootState: true);    //换完了，开抖！
        shootNumber = 0;                                //重置弹药数量
        isReloading = false;
        remainingBulletText.SetText((playerData.ClipSize - shootNumber).ToString());
    }
    #endregion

    #region 射击与射击效果
    private void Shoot()
    {
            shootNumber++;
            playerController.SetShootingState(shootState: true);
            SoundManager.Instance.PlaySound("Assault Shoot");
            remainingBulletText.SetText(remainingBulletCount);
            Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
            Vector2 shootDirection = GetShootDirection();      //用来提供方向的
            //实际上可以发现这里的shootDirection根本用不着判断，直接都是用firePoint.parent.up即可
            //因为firePoint的父级是Player啊，PAlayer早就和鼠标做过方向判断了，这里其实都是多余的。只不过说熟悉一下过程罢了

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, shootDirection, 100, whatToHit);
            if(hit.collider!=null)
            {
                 ShootEffect(hit);
            }
            else
            {
                 ShootEffect(shootDirection);
            }
        playerController.KnockBack();
    }

    private void ShootEffect(Vector2 shootDirection)
    {
        float shootTrailAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;  //计算夹角
        Quaternion trailRotaion = Quaternion.AngleAxis(shootTrailAngle, Vector3.forward);
        Transform trail = Instantiate(shootTrail, firePoint.position, trailRotaion);   //创建一个临时变量存储当前这颗子弹的值
        Destroy(trail.gameObject, 0.05f);     //在0.05s之后删除掉它

        //Debug.DrawLine(firePointPosition, shootDirection*100, Color.red);
        MuzzleFlash();      //fire
    }

    
    //射击粒子特效
    private void ShootEffect(RaycastHit2D hit)
    {
        Vector3 firePointPosition = firePoint.position;
        Transform trail = Instantiate(shootTrail, firePointPosition, Quaternion.identity);
        LineRenderer lineRender = trail.GetComponent<LineRenderer>();
        Vector3 endPosition = new Vector3(hit.point.x, hit.point.y, firePointPosition.z);   //hit.x和hit.y是射中点的横纵坐标
        lineRender.useWorldSpace = true;
        lineRender.SetPosition(0, firePointPosition);
        lineRender.SetPosition(1, endPosition);
        Destroy(trail.gameObject, 0.05f);

        //打击效果
        Transform sparks = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));    //火花例子特效
        Destroy(sparks.gameObject, 0.2f);   //时间不能太短，不然例子特效没播完就没了


        //制造伤害
        IDamagable damageable = hit.transform.GetComponent<IDamagable>();   //被打击对象hit是否继承子transform窗口
        
        if(damageable!=null)
        {
            (int damage, bool isCriticalHit) = playerData.CalculateDamage();
            damageable.TakeDamage(damage); //吃了没？没吃吃我子弹
            Popuptext.Creat(hit.point,damage, isCriticalHit);   //Random.Range(0,100)<30这么写就有30%的概率暴击
        }

        MuzzleFlash();      //高贵的枪口火焰
    }


    private Vector2 GetShootDirection()
    {
        //直接使用firePoint的正向方向向量即可，不用白不用
        Vector2 shotDir = transform.up;
        //shotDir.x += Random.Range(-playerData.AimOffset, playerData.AimOffset);    //模拟子弹弹道偏移
        //shotDir.y += Random.Range(-playerData.AimOffset, playerData.AimOffset);
        return shotDir.normalized;      //规范化，只需要返回方向即可
        //这里解决鼠标和角色过近导致的问题,以下为废案，原因是多此一举
        //if (Vector2.Distance(mousePosition, firePoint.position) > 0.5)   //大于0.5就不管 firePoint.position隐式转换
        //{
        //   return   mousePosition - (Vector2)firePoint.position;       //强制转换
        //}
        //else
        //{
        //   return  firePoint.parent.up;       //或者transform.up;
        //}
    }


    //枪口火焰
    private void MuzzleFlash()
    {
        //flash是只想预制体muzzleFlash的一个引用，使用Instantiate告诉muzzleFlash的位置以及方向（与firePoint.roration保持一致）
        Transform flash = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
        flash.SetParent(firePoint);
        float randomSize = Random.Range(0.6f, 0.9f);    //0.6到0.9随机数
        flash.localScale = new Vector3(randomSize, randomSize, randomSize);     //随机大小枪口火焰
        Destroy(flash.gameObject, 0.05f);   //和飞行轨迹Trail同时销毁
    }
    #endregion

}
