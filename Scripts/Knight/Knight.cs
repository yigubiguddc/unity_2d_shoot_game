using UnityEngine;
using Bronya;

//玩家也继承自LivingEntity类
public class Knight : LivingEntity
{
    //Player's data set
    private PlayerData playerData;
    private Animator _animator;     //和livingentity关联，用来播放死亡动画
    public delegate void PressedIKeyHandler();
    public static System.Action OnPressedIKey;
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private int expCount = 0;
    [SerializeField] private Transform levelUp;
    private float hideAtTime;




    protected override void Start()
    {
        levelUp.gameObject.SetActive(false);
        playerData = new PlayerData();
        startHealth = 100;

        maxExp = 10;        //等级1，经验最多可以得到100.

        _animator = GetComponent<Animator>();
        base.Start();
        //Init playerController..
        //GetComponent<PlayerController>().Init(playerData.MoveSpeed);
        //Init weapon system..
        //GetComponentInChildren<PlayerGun>().Init(playerData);
        GameEvents.OnPlayerHealthChange?.Invoke(Health, startHealth);
        GameEvents.OnPlayerExpChange?.Invoke(Exp, maxExp);
    }


    private void Update()
    {
        if (levelUp.gameObject.activeSelf && Time.time >= hideAtTime)
        {
            levelUp.gameObject.SetActive(false);
        }
    }

    public override void TakeDamage(float damage)
    {
        //如果必死一击
        if (damage >= Health)
        {
            GameEvents.GameOver?.Invoke();
            _animator.SetBool("IsDead", true);
            //这里重载主要是为了写销毁枪械的代码。
            for(int i=0;i<gameObjects.Length;i++)
            {
                Destroy(gameObjects[i]);
            }
        }
        //如果能扛得住
        base.TakeDamage(damage);
        GameEvents.OnPlayerHealthChange?.Invoke(Health, startHealth);        //LivingEntity中的血量

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.gameObject.layer == 12)                      //Layer12是Collection,只有ExpPrefab
        if(collision.gameObject.tag == "Collection")
        {
            expCount++;
            ExpChange();
            GameEvents.OnPlayerExpChange?.Invoke(Exp, maxExp);
            ObjectPool.Instance.PushObject(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Boss")
        {
            expCount += 100;
            ExpChange();
            GameEvents.OnPlayerExpChange?.Invoke(Exp, maxExp);
            ObjectPool.Instance.PushObject(collision.gameObject);
        }
        else if(collision.gameObject.tag=="friends")
        {
            //Debug.Log(collision.gameObject.name);
        }
        else if(collision.gameObject.tag=="Heal")
        {
            ObjectPool.Instance.PushObject(collision.gameObject);
            HealPlayer(10);
            GameEvents.OnPlayerHealthChange?.Invoke(Health, startHealth);
            //Debug.Log("Heal the player!");
        }
    }

    public void ExpChange()
    {
        if(expCount < maxExp)
        {
            base.ExpGet(expCount);
        }
        else if(expCount>=maxExp)
        {
            while(expCount>=maxExp)
            {
                level++;
                SoundManager.Instance.PlaySound("LevelUp");

                levelUp.gameObject.SetActive(true);
                hideAtTime = Time.time + 2f;
                expCount -= maxExp;
                maxExp += 2;
                //Health change when level up.
                if (Health == startHealth)
                {
                    startHealth += 10;
                    GameEvents.PlayerUpdate?.Invoke(startHealth);

                }
                else
                {
                    startHealth += 10;
                }
                GameEvents.OnPlayerHealthChange?.Invoke(Health, startHealth);
                //Debug.Log($"{startHealth}");

            }
        }
    }

    


    public void HealPlayer(int health)
    {
        base.HealthGet(health);
    }

    [ContextMenu("Nothing")]
    public void nothing()
    {
        Debug.Log("Nothing is nothing,bro");
    }

    [ContextMenu("Suicide")]
    public void suicide()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Could only be used when the game start.");
            return;
        }
        TakeDamage(9999999);
    }
}
