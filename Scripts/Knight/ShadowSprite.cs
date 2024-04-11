using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;
    private Color color;
    [Header("事件控制参数")]
    public float activeTime;//显示的时间
    public float activeStart;//显示开始的时间
    [Header("不透明度控制")]
    private float alpha;
    public float alphaSet;//初始值
    public float alphaMultiplier;//平滑的变化
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
            //返回对象池
            ObjectPool.Instance.PushObject(this.gameObject);
        }
    }
}
