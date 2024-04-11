using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject explosionEffect;
    private Rigidbody2D _rigidbody;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    //5S后销毁物体，否则会有很多警告
    /*private void OnEnable()
    {
        Invoke("ReturnToPool", 5f);
    }

    private void ReturnToPool()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }*/

    public void SetSpeed(Vector2 direction)
    {
        _rigidbody.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //在5s之内碰撞到了其他的物体，则取消Invoke
        CancelInvoke("ReturnToPool");
        if (other.CompareTag("Enemy")||other.CompareTag("Obstacle"))
        {
            GameObject explosion = ObjectPool.Instance.GetObject(explosionEffect);        //从对象池中获取对象，至于Instance属于是单例模式的特点了
            IDamagable damageable = other.GetComponent<IDamagable>();
            if (damageable != null)
            {
                damageable.TakeDamage(10); //吃了没？没吃吃我子弹
                Popuptext.Creat(other.transform.position, 10, false);   //Random.Range(0,100)<30这么写就有30%的概率暴击
                //Debug.LogError("Text has been created!");
            }
            explosion.transform.position = transform.position;
            ObjectPool.Instance.PushObject(gameObject);     //用完了之后归还到对象池中
        }
    }

}
