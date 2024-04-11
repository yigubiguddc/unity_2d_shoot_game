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

    //OnEnable���������屻����ʱ������
    private void OnEnable()
    {
        float angle = Random.Range(-30f, 30f);
        _rigidbody.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * speed;    //�ӵ�Ҳ������������ˣ�������ʵ
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        _rigidbody.gravityScale = 3;
        transform.rotation = Quaternion.identity;
        //�ӵ��Ǹձ����ɾͿ�ʼִ��Stop��Э�̣��ܷ��㣬����������Ĺ�����
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
