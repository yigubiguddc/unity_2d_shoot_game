using UnityEngine;
using TMPro;        //TestMeshPro的命名空间如此
using Bronya;


public class Popuptext : MonoBehaviour
{
    //和setup对应，静态启动方法
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
    public const float DisappearTimeMax = 0.5f; //0.2s后透明度降低开始
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
        if(isCriticalHit)       //暴击
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
        //transform.position += Vector3.up * Time.deltaTime * moveSpeed;  //Time.deltaTime 在实现伤害数字漂浮的效果中的作用就是确保这个漂浮效果在不同的设备或不同的性能下都能够保持一致的速度，不会因为帧率的变化而变快或变慢。
        //move up
        if(_disappearTimer > DisappearTimeMax*0.5f)
        {
            transform.position += moveUpVector * Time.deltaTime;
            moveUpVector += moveUpVector * (Time.deltaTime * moveSpeed);        //text's speed up
            transform.localScale += Vector3.one * (Time.deltaTime * 1f);        //这里的1.0f后期可以修改，时缩放的速率
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
            _textColor.a -= Time.deltaTime * _disappearSpeed;       //减少伽马值（降低透明度）
            _textMeshPro.color = _textColor;
            if(_textColor.a < 0)
            {
                Destroy(gameObject);
                //Debug.Log("Destroy the textMeshPro.");
            }
        }
    }
}
