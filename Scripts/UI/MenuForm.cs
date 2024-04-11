using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Bronya;

public class MenuForm : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;
    private Toggle currentSelcetion => toggleGroup.GetFirstActiveToggle();  //为它赋值为第一个激活的开关
    private Toggle onToggle;    //用于记录当前打开的开关



    private void Start()
    {
        var toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        foreach(var toggle in toggles)
        {
            //c# 7.0更新的弃元特性
            toggle.onValueChanged.AddListener(_ => OnToggleValueChanged(toggle));
        }
        currentSelcetion.onValueChanged?.Invoke(true);
    }

    private void OnToggleValueChanged(Toggle toggle)
    {
        SoundManager.Instance.PlaySound("Finger Snap");

        //需要点击两次才能进入判断
        if(currentSelcetion == onToggle)
        {
            switch(toggle.name)
            {
                case "GameStart":
                    SceneManager.LoadScene("Main");
                    SoundManager.Instance.PlaySound("Select");
                    break;
                case "Seetings":
                    SoundManager.Instance.PlaySound("Select");
                    break;
                case "Quit":
                    SoundManager.Instance.PlaySound("Select");
                    Application.Quit();
                    //宏定义判断平台
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;     //相当于在unity中点一下播放按钮
                    #endif
                    break;
                default:
                    //抛出异常
                    throw new UnityException("Invaild action");
            }
            //Debug.Log(currentSelcetion.name);   
            return;
        }

        if(toggle.isOn)
        {
            //Debug.Log($"当前开关{toggle.name}"); 
            onToggle = toggle;
            onToggle.transform.Find("Label").GetComponent<TMP_Text>().color = Color.blue;

        }
        else
        {
            //Debug.Log($"被关闭的开关{toggle.name}");
            onToggle.transform.Find("Label").GetComponent<TMP_Text>().color = Color.white;
        }
    }

}
