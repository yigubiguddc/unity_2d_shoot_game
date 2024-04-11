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

    //5S���������壬������кܶྯ��
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
        //��5s֮����ײ�������������壬��ȡ��Invoke
        CancelInvoke("ReturnToPool");
        if (other.CompareTag("Enemy")||other.CompareTag("Obstacle"))
        {
            GameObject explosion = ObjectPool.Instance.GetObject(explosionEffect);        //�Ӷ�����л�ȡ��������Instance�����ǵ���ģʽ���ص���
            IDamagable damageable = other.GetComponent<IDamagable>();
            if (damageable != null)
            {
                damageable.TakeDamage(10); //����û��û�Գ����ӵ�
                Popuptext.Creat(other.transform.position, 10, false);   //Random.Range(0,100)<30��ôд����30%�ĸ��ʱ���
                //Debug.LogError("Text has been created!");
            }
            explosion.transform.position = transform.position;
            ObjectPool.Instance.PushObject(gameObject);     //������֮��黹���������
        }
    }

}
