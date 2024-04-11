using UnityEngine;
using UnityEngine.UI;

//这里又是和框架相关的内容，我们再次拿出Bronya命名空间
namespace Bronya
{
    public abstract class UGuiForm : MonoBehaviour  //这里是一个抽象类，它不需要被实例化
    {

        public Transform cachedTransform;    //将transform组件缓存下来
        private const float DurationFadeTime = 0.3f;


        private bool isVisible;
        public bool Visible
        {
            get => isVisible;
            set
            {
                if(isVisible == value)
                {
                    return;
                }
                isVisible = value;
                gameObject.SetActive(value);
            }
        }

        //封装,防止在其他地方意外修改对象的值
        private string Name
        {
            get => gameObject.name;
            set => gameObject.name = value;
        }

        private CanvasGroup canvasGroup;
        public virtual void OnInit()
        {
            if(cachedTransform == null)
            {
                cachedTransform = transform;
            }
            canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            gameObject.GetOrAddComponent<GraphicRaycaster>();

            RectTransform rectTransform = GetComponent<RectTransform>();
            //锚点位置与父物体锚点框架相同
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.anchorMin = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;  //与父节点物体没有偏移值
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            //所有界面本地化localization


        }

        public virtual void OnOpen()    //界面打开时
        {
            //gameObject.SetActive(true);
            Visible = true;
            canvasGroup.alpha = 0;
            //渐变效果
            StartCoroutine(canvasGroup.FadeToAlpha(1f, DurationFadeTime));
        }
        public virtual void OnUpdate()
        {

        }
        public virtual void OnClose()   //界面关闭
        {
            Visible = false;
        }

        public void PlaySound()
        {
            
        }
    }
}

