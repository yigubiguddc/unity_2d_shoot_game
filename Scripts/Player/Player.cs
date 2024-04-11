using UnityEngine;

//玩家也继承自LivingEntity类
public class Player : LivingEntity
{
    //Player's data set
    private PlayerData playerData;
    protected override void Start()
    {
        playerData = new PlayerData();
        startHealth = playerData.Health;
        base.Start();

        //Init playerController..
        GetComponent<PlayerController>().Init(playerData.MoveSpeed);
        //Init weapon system..
        GetComponentInChildren<PlayerGun>().Init(playerData);

        GameEvents.OnPlayerHealthChange?.Invoke(Health, startHealth);
    }


    public override void TakeDamage(float damage)
    {
        if(damage >= Health)
        {
            GameEvents.GameOver?.Invoke();
        }

        base.TakeDamage(damage);

        GameEvents.OnPlayerHealthChange?.Invoke(Health,startHealth);        //LivingEntity中的血量
    }

    [ContextMenu("Suicide")]
    public void suicide()
    {
        if(!Application.isPlaying)
        {
            Debug.LogError("Could only be used when the game start.");
            return;
        }
        TakeDamage(99999);
    }
}
