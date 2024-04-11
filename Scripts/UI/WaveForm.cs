using System.Globalization;
using UnityEngine;
using TMPro;
using System.Collections;

public class WaveForm : MonoBehaviour
{
    [SerializeField] private TMP_Text countDownText;        //TmpText��TextMeshPro����Ļ��࣬TMP_Text ���ṩ��һϵ�� API������������ȡ�������ı������ԣ������ı������ݡ���ɫ�����塢��С�����뷽ʽ�ȵȡ�����ʹ�� TextMeshPro ���� UI �ı���̬�ı�ʱ��ʵ���Ͼ�����ʹ�� TMP_Text �༰�����ࡣ
    [SerializeField] private TMP_Text WaveNumberText;
    [SerializeField] private CanvasGroup container;

    [SerializeField]private bool isCountingDown;        //�Ƿ��ڵ���ʱ״̬
    private float countDownTimer;       //���ж��ٹ��Ｔ���ִ�
    private bool isFading;

    private void Awake()
    {
        Wavemanager.OnNewWave += OnNewWave;
        //Debug.Log("������WaveForm��Awake����");
    }

    private void OnNewWave(int waveNumber)
    {
        isFading = false;
        //Debug.Log("ִ����OneNewWave������");
        isCountingDown = true;
        countDownTimer = Wavemanager.timeBetweenWaves;      //ÿһ���ּ��3s��
        //countDownTimer = 3.0f;        ��Ӧ�üᶨ�����ź�������ֵ�Ĵ��ݵ�׼ȷ�ԣ�xd
        countDownText.alpha = 1;
        //Debug.Log("countDownText��͸���ȵ���");
        WaveNumberText.SetText(waveNumber.ToString());      //���ʱ=��������
    }

    private void Update()
    {
        if (!isCountingDown) return;        //ע�⣬������!isCountingDown

        countDownTimer -= Time.deltaTime;
        countDownText.gameObject.SetActive(countDownTimer > 0.1f);      //��countDownText��ֵ����0.1ʱһֱ��ʾ����������

        //����ʱ��˸
        if(countDownTimer%1>0.5f)
        {
            countDownText.alpha -= Time.deltaTime * 2;
        }
        else
        {
            countDownText.alpha += Time.deltaTime * 2;
        }

        if(countDownTimer<Wavemanager.timeBetweenWaves * 0.5f)
        {
             container.alpha += Time.deltaTime * 0.5f;
        }


        if(countDownTimer<0.1f)
        {
            
            if(!isFading)
            {
                //����ʵ��̫��ª�ˣ�ֱ����ʧ�ܲ�����
                //container.alpha = 0;
                StartCoroutine(FadeOut());      //ִֻ��һ��
                isFading = true;
            }
            isCountingDown = false;
        }
        else
        {
            countDownText.SetText(Mathf.Round(countDownTimer).ToString(CultureInfo.InvariantCulture));
        } 
    }


    private IEnumerator FadeOut()
    {
        float startAlpha = 1;
        float endAlpha = 0;
        float duration = 0.5F;  // ��������1��

        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float elapsed = Time.time - startTime;
            container.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        // ȷ��alpha׼ȷ�ر�Ϊ0
        container.alpha = endAlpha;
        isFading = false;
    }




    private void OnDestroy()
    {
        Wavemanager.OnNewWave -= OnNewWave;
    }
    
}
