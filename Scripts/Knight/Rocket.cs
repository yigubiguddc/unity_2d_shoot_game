using System.Collections;
using Bronya;
using UnityEngine;


public class Rocket : MonoBehaviour
{
    public float lerp;          //转向速度
    public float speed = 15;    //移动速度
    public GameObject explosionPrefab;
    private Rigidbody2D _rigidbody;

    //火箭弹打击位置和方向
    private Vector3 targetPos;
    private Vector3 direction;

    //火箭弹到达鼠标位置之后开始直线运动而不是继续曲线运动
    private bool arrived;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

    }

    /*private void OnEnable()
    {
        Invoke("ReturnToPool", 5f);
    }

    private void ReturnToPool()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }*/

    public void SetTarget(Vector3 _target)
    {
        arrived = false;
        targetPos = _target;
    }
    private void FixedUpdate()
    {
        direction = (targetPos - transform.position).normalized;
        //使用火箭弹的右方向向目标方向进行平滑插值
        if (!arrived)
        {
            //这里让插值速度为lerp/Vector2.Distance(transform.position,targetPos)可以让火箭弹距离目标点越近lerp/(distance)越大，也即转向速度越快
            transform.right = Vector3.Lerp(transform.right, direction, lerp / Vector2.Distance(transform.position, targetPos));
            _rigidbody.velocity = transform.right * speed;
        }
        if (Vector2.Distance(transform.position, targetPos) < 1f && !arrived)     //与目标点非常近的时候可以认为i已经达到了目标点。
        {
            arrived = true;
        }
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        SoundManager.Instance.PlaySound("explore");
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            GameObject explosion = ObjectPool.Instance.GetObject(explosionPrefab);
            explosion.transform.position = transform.position;

            IDamagable damagable = other.GetComponent<IDamagable>();
            if (damagable != null)
            {
                int Randondamage = Random.Range(10, 20);
                damagable.TakeDamage(Randondamage);
                Popuptext.Creat(other.transform.position, Randondamage, Random.Range(1, 100) > 30);
            }

            _rigidbody.velocity = Vector2.zero;
            StartCoroutine(Push(gameObject, 0.3f));
        }
    }
    IEnumerator Push(GameObject _object, float waittime)
    {
        yield return new WaitForSeconds(waittime);
        ObjectPool.Instance.PushObject(_object);
    }


}
