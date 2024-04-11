using UnityEngine;
using UnityEngine.UI;

//�������ǺͿ����ص����ݣ������ٴ��ó�Bronya�����ռ�
namespace Bronya
{
    public abstract class UGuiForm : MonoBehaviour  //������һ�������࣬������Ҫ��ʵ����
    {

        public Transform cachedTransform;    //��transform�����������
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

        //��װ,��ֹ�������ط������޸Ķ����ֵ
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
            //ê��λ���븸����ê������ͬ
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.anchorMin = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;  //�븸�ڵ�����û��ƫ��ֵ
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            //���н��汾�ػ�localization


        }

        public virtual void OnOpen()    //�����ʱ
        {
            //gameObject.SetActive(true);
            Visible = true;
            canvasGroup.alpha = 0;
            //����Ч��
            StartCoroutine(canvasGroup.FadeToAlpha(1f, DurationFadeTime));
        }
        public virtual void OnUpdate()
        {

        }
        public virtual void OnClose()   //����ر�
        {
            Visible = false;
        }

        public void PlaySound()
        {
            
        }
    }
}

