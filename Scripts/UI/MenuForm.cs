using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Bronya;

public class MenuForm : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;
    private Toggle currentSelcetion => toggleGroup.GetFirstActiveToggle();  //Ϊ����ֵΪ��һ������Ŀ���
    private Toggle onToggle;    //���ڼ�¼��ǰ�򿪵Ŀ���



    private void Start()
    {
        var toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        foreach(var toggle in toggles)
        {
            //c# 7.0���µ���Ԫ����
            toggle.onValueChanged.AddListener(_ => OnToggleValueChanged(toggle));
        }
        currentSelcetion.onValueChanged?.Invoke(true);
    }

    private void OnToggleValueChanged(Toggle toggle)
    {
        SoundManager.Instance.PlaySound("Finger Snap");

        //��Ҫ������β��ܽ����ж�
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
                    //�궨���ж�ƽ̨
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;     //�൱����unity�е�һ�²��Ű�ť
                    #endif
                    break;
                default:
                    //�׳��쳣
                    throw new UnityException("Invaild action");
            }
            //Debug.Log(currentSelcetion.name);   
            return;
        }

        if(toggle.isOn)
        {
            //Debug.Log($"��ǰ����{toggle.name}"); 
            onToggle = toggle;
            onToggle.transform.Find("Label").GetComponent<TMP_Text>().color = Color.blue;

        }
        else
        {
            //Debug.Log($"���رյĿ���{toggle.name}");
            onToggle.transform.Find("Label").GetComponent<TMP_Text>().color = Color.white;
        }
    }

}
