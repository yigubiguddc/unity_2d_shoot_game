//有血条的生物(活物)
//
//2023/9/15修改
//
//
using UnityEngine;
using System;
using Bronya;
public class LivingEntity : MonoBehaviour, IDamagable
{
    public GameObject expPrefab;
    public GameObject BossPrizePrefab;
    public GameObject HealPrefab;
    public float startHealth;                            //血量
    public int maxExp;                                 //经验
    public int level = 1;
    public int levelIncreaseCount = 0;

    protected float Health { get; private set; }         //当前血量  这是定义的属性
    protected float Exp { get; private set; }            //完全模仿血量吧.

    protected bool IsDead;
    public ParticleSystem deathParticle;                 //死亡特效
    public string deathSound;                            //死亡音效
    public event Action OnDeath;                         //事件 注意需要using System;

    private void OnEnable()
    {
        GameEvents.PlayerUpdate += PlayerUpdate;
    }

    public void PlayerUpdate(float startHealth)
    {
        if(gameObject.transform.tag =="Player")
        {
            Health = startHealth;
        }
    }

    protected virtual void Start()
    {
        //assign the Health and Exp a start value.
        Health = startHealth;
        Exp = 0;        //初始值为0
    }

    public virtual void ExpGet(int expCount)
    {
        Exp = expCount;
    }

    public virtual void HealthGet(int health)
    {

        if(Health + health > startHealth)
        {
            Health = startHealth;
        }
        else
        {
            Health += health;
        }  
    }


    public virtual void TakeDamage(float damage)         //虚方法,可重写
    {
        Health -= damage;
        if (Health > 0 || IsDead) return;
        //The followiung code will only be run when the LivingEntity is OnDead
        IsDead = true;
        OnDeath?.Invoke();                               //如果OnDeath非空则执行Invoke
        //Just the enemy's death would leave the epxPrefab.
        if (gameObject.transform.tag == "Enemy")
        {
            if (gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                //Debug.Log("就你jb是boss啊？");
                BossPrizePrefab = ObjectPool.Instance.GetObject(BossPrizePrefab);
                BossPrizePrefab.transform.position = transform.position;
                BossPrizePrefab.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-50f, 50f));
            }
            else if(gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //一定要注意这一段代码，要进行一个赋值的操作，我之前没有在前面写expPrefab，因此在执行expPrefab.transform.position = transform.posiotion实际上没起实际作用
                //敌对物体死亡掉落经验值，需要拾取。
                expPrefab = ObjectPool.Instance.GetObject(expPrefab);
                expPrefab.transform.position = transform.position;
                expPrefab.transform.localScale = transform.localScale;
                expPrefab.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-45f, 45f));
            }    
            else if(gameObject.layer == LayerMask.NameToLayer("Box"))
            {
                HealPrefab = ObjectPool.Instance.GetObject(HealPrefab);
                HealPrefab.transform.position = transform.position;
                HealPrefab.transform.localScale = transform.localScale;
                HealPrefab.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-50f, 50f));
            }
        }
        //销毁父级Transform的gameObject。
        Transform _parent = transform.parent;
        if(_parent!=null)
        {
            //ObjectPool.Instance.PushObject(_parent.gameObject);
            Destroy(_parent.gameObject);
        }
        //销毁本体。
        if(gameObject.layer == 9)       //Player，使用了相对原始的方法
        {
            //enough time to play the player's Dying Anim.
            Destroy(gameObject, 1.6f);
        }
        else
        {
            //ObjectPool.Instance.PushObject(gameObject);
            Destroy(gameObject);
        }

        //没有死亡特效的xd就直接return掉
        if (deathParticle == null)
        {
            return;                                      
        }
        //死亡特效
        ParticleSystem ps = Instantiate(deathParticle, transform.position, Quaternion.identity);    //别忘了，Quaterion.identity的意思是说不需要旋转！
        Destroy(ps.gameObject, ps.main.duration);
        if(!string.IsNullOrEmpty(deathSound))
        {
            SoundManager.Instance.PlaySound(deathSound);
        }
    }


    private void OnDisable()
    {
        GameEvents.PlayerUpdate -= PlayerUpdate;
    }
}