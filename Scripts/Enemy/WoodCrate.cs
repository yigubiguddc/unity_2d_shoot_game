using UnityEngine;
public class WoodCrate :LivingEntity
{
    public void TakeDamege(float damage)    //啊啊啊，为什么不能加override
    {
        if(damage>=Health&&!IsDead)
        {
            OnDeath += () => { Debug.Log("Wood crate die!!!"); };   //Debug与OnDeath事件建立订阅
        }

        //死亡特效、死亡音效、被打击特效、被打击音乐效果都可以在这里实现


        //死亡特效

        //死亡音效
        base.TakeDamage(damage);
    }
}