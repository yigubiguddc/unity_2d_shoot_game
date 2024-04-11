using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;
    private Color color;
    [Header("�¼����Ʋ���")]
    public float activeTime;//��ʾ��ʱ��
    public float activeStart;//��ʾ��ʼ��ʱ��
    [Header("��͸���ȿ���")]
    private float alpha;
    public float alphaSet;//��ʼֵ
    public float alphaMultiplier;//ƽ���ı仯
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;
        activeStart = Time.time;
    }
    private void Update()
    {
        alpha -= alphaMultiplier;
        //Debug.Log($"ALPHA:{alpha}");
        color = new Color(0.5f,0.5f,1,alpha);
        thisSprite.color = color;
        if (Time.time > activeStart + activeTime)
        {
            //���ض����
            ObjectPool.Instance.PushObject(this.gameObject);
        }
    }
}
