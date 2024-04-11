using System.Collections;
using Bronya;
using UnityEngine;


public class Rocket : MonoBehaviour
{
    public float lerp;          //ת���ٶ�
    public float speed = 15;    //�ƶ��ٶ�
    public GameObject explosionPrefab;
    private Rigidbody2D _rigidbody;

    //��������λ�úͷ���
    private Vector3 targetPos;
    private Vector3 direction;

    //������������λ��֮��ʼֱ���˶������Ǽ��������˶�
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
        //ʹ�û�������ҷ�����Ŀ�귽�����ƽ����ֵ
        if (!arrived)
        {
            //�����ò�ֵ�ٶ�Ϊlerp/Vector2.Distance(transform.position,targetPos)�����û��������Ŀ���Խ��lerp/(distance)Խ��Ҳ��ת���ٶ�Խ��
            transform.right = Vector3.Lerp(transform.right, direction, lerp / Vector2.Distance(transform.position, targetPos));
            _rigidbody.velocity = transform.right * speed;
        }
        if (Vector2.Distance(transform.position, targetPos) < 1f && !arrived)     //��Ŀ���ǳ�����ʱ�������Ϊi�Ѿ��ﵽ��Ŀ��㡣
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
