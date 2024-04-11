using System.Globalization;
using UnityEngine;
using TMPro;
using System.Collections;

public class WaveForm : MonoBehaviour
{
    [SerializeField] private TMP_Text countDownText;        //TmpText是TextMeshPro组件的基类，TMP_Text 类提供了一系列 API，可以用来获取和设置文本的属性，比如文本的内容、颜色、字体、大小、对齐方式等等。当你使用 TextMeshPro 创建 UI 文本或动态文本时，实际上就是在使用 TMP_Text 类及其子类。
    [SerializeField] private TMP_Text WaveNumberText;
    [SerializeField] private CanvasGroup container;

    [SerializeField]private bool isCountingDown;        //是否处于倒计时状态
    private float countDownTimer;       //还有多少怪物即将抵达
    private bool isFading;

    private void Awake()
    {
        Wavemanager.OnNewWave += OnNewWave;
        //Debug.Log("进入了WaveForm的Awake函数");
    }

    private void OnNewWave(int waveNumber)
    {
        isFading = false;
        //Debug.Log("执行了OneNewWave函数了");
        isCountingDown = true;
        countDownTimer = Wavemanager.timeBetweenWaves;      //每一波怪间隔3s钟
        //countDownTimer = 3.0f;        您应该坚定的相信函数间数值的传递的准确性，xd
        countDownText.alpha = 1;
        //Debug.Log("countDownText的透明度调整");
        WaveNumberText.SetText(waveNumber.ToString());      //这个时=是正常的
    }

    private void Update()
    {
        if (!isCountingDown) return;        //注意，这里是!isCountingDown

        countDownTimer -= Time.deltaTime;
        countDownText.gameObject.SetActive(countDownTimer > 0.1f);      //当countDownText的值大于0.1时一直显示，否则隐藏

        //倒计时闪烁
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
                //这里实在太丑陋了，直接消失很不美观
                //container.alpha = 0;
                StartCoroutine(FadeOut());      //只执行一次
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
        float duration = 0.5F;  // 动画持续1秒

        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float elapsed = Time.time - startTime;
            container.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        // 确保alpha准确地变为0
        container.alpha = endAlpha;
        isFading = false;
    }




    private void OnDestroy()
    {
        Wavemanager.OnNewWave -= OnNewWave;
    }
    
}
