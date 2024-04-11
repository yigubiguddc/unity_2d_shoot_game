using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShell : MonoBehaviour
{
    public float speed;
    public float stopTime;
    public float fadeSpeed;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer sprite;

    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        
    }

    //OnEnable函数再物体被激活时被调用
    private void OnEnable()
    {
        float angle = Random.Range(-30f, 30f);
        _rigidbody.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * speed;    //子弹也不是随机掉落了，更加真实
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        _rigidbody.gravityScale = 3;
        transform.rotation = Quaternion.identity;
        //子弹壳刚被生成就开始执行Stop的协程，很方便，不用做多余的工作。
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(stopTime);
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.gravityScale = 0;
        while (sprite.color.a > 0)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - fadeSpeed);
            yield return new WaitForFixedUpdate();
        }
        //Destroy(gameObject);

        ObjectPool.Instance.PushObject(gameObject);
    }

}
