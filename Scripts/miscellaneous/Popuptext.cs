using UnityEngine;
using TMPro;        //TestMeshPro�������ռ����
using Bronya;


public class Popuptext : MonoBehaviour
{
    //��setup��Ӧ����̬��������
    public static void Creat(Vector3 position,int damageAcount, bool isCriticalHit)
    {
        Popuptext popupText = Instantiate(SourceManager.Instance.Load<Popuptext>("Popuptext"),position, Quaternion.identity);
        popupText.Setup(damageAcount, isCriticalHit);
    }

    private TextMeshPro _textMeshPro;
    private Color _textColor;

    [Header("Move Up")]
    public Vector3 moveUpVector = new Vector3(0, 1, 0);
    public float moveSpeed = 4.0f;

    [Header("Move Down")]
    public Vector3 moveDownVector = new Vector3(-0.7f, 1, 0);

    [Header("Disappear")]
    public const float DisappearTimeMax = 0.5f; //0.2s��͸���Ƚ��Ϳ�ʼ
    public float _disappearTimer;
    public float _disappearSpeed = 3.0f;

    [Header("Damage Colors")]
    public Color normalColor;
    public Color criticalHitColor;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAcount,bool isCriticalHit)
    {
        _textMeshPro.SetText(damageAcount.ToString());
        if(isCriticalHit)       //����
        {
            _textMeshPro.fontSize = 5;
            _textColor = criticalHitColor;
        }
        else
        {
            _textMeshPro.fontSize = 3;
            _textColor = normalColor;
        }
        _textMeshPro.color = _textColor;
        _disappearTimer = DisappearTimeMax;
    }

    private void Update()
    {
        //transform.position += Vector3.up * Time.deltaTime * moveSpeed;  //Time.deltaTime ��ʵ���˺�����Ư����Ч���е����þ���ȷ�����Ư��Ч���ڲ�ͬ���豸��ͬ�������¶��ܹ�����һ�µ��ٶȣ�������Ϊ֡�ʵı仯�����������
        //move up
        if(_disappearTimer > DisappearTimeMax*0.5f)
        {
            transform.position += moveUpVector * Time.deltaTime;
            moveUpVector += moveUpVector * (Time.deltaTime * moveSpeed);        //text's speed up
            transform.localScale += Vector3.one * (Time.deltaTime * 1f);        //�����1.0f���ڿ����޸ģ�ʱ���ŵ�����
        }
        //move down
        else
        {
            transform.position -= moveDownVector * Time.deltaTime;
            transform.localScale -= Vector3.one * (Time.deltaTime * 1f);
        }

        
        //disappear
        _disappearTimer -= Time.deltaTime;
        if(_disappearTimer<0)
        {
            //alpha down
            _textColor.a -= Time.deltaTime * _disappearSpeed;       //����٤��ֵ������͸���ȣ�
            _textMeshPro.color = _textColor;
            if(_textColor.a < 0)
            {
                Destroy(gameObject);
                //Debug.Log("Destroy the textMeshPro.");
            }
        }
    }
}
