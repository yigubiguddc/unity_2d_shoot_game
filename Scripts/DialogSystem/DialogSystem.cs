using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class DialogSystem : MonoBehaviour
{
    [Header("头像")]
    public Sprite head1;
    public Sprite head2;

    [Header("UI组件")]
    public Text textLabel;
    public Image faceImage;

    [Header("文本文件")]
    public TextAsset textFile;  //Drag a file which named behind the .txt .md and so on.
    public int LineIndex;

    List<string> textList = new List<string>();
    public float TextSpeedBetween;
    bool TextFinished;

    private void Awake()
    {
        GetTextFromFile(textFile);
    }
    private void OnEnable()
    {
        TextFinished = true;
        StartCoroutine(SetTextUI());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(LineIndex == textList.Count)
            {
                gameObject.SetActive(false);    //关闭Panel，然后重置LineIndex
                LineIndex = 0;
                return;
            }
            else if(TextFinished)
            {
                StartCoroutine(SetTextUI());    //确保协程不同步开始
            }
               

        }
    }
    IEnumerator SetTextUI()
    {
        /*switch (LineIndex%4)
        {
            case 0:
                faceImage.sprite = head1;
                LineIndex++;
                TextFinished = false;
                textLabel.text = "";
                for (int i = 0; i < textList[LineIndex].Length; i++)
                {
                    TextSpeedBetween = Random.Range(0.01f, 0.05f);
                    textLabel.text += textList[LineIndex][i];   //在协程中一个字一个字地加，每次显示一个字都会停顿时间TextSpeedBetween
                    yield return new WaitForSeconds(TextSpeedBetween);
                }
                TextFinished = true;
                LineIndex++;
                break;
            case 2:
                faceImage.sprite = head2;
                LineIndex++;
                TextFinished = false;
                textLabel.text = "";
                for (int i = 0; i < textList[LineIndex].Length; i++)
                {
                    TextSpeedBetween = Random.Range(0.01f, 0.05f);
                    textLabel.text += textList[LineIndex][i];   //在协程中一个字一个字地加，每次显示一个字都会停顿时间TextSpeedBetween
                    yield return new WaitForSeconds(TextSpeedBetween);
                }
                TextFinished = true;
                LineIndex++;
                break;
        }
        */
        #region "失效方案"
        //出现了意料之外的情况，以下这种方法失效了，我很气愤！！！
        switch (textList[LineIndex])
        {
            case "A":
                faceImage.sprite = head1;
                LineIndex++;
                Debug.Log($"{LineIndex}");
                break;
            case "B":
                faceImage.sprite = head2;
                LineIndex++;
                Debug.Log($"{LineIndex}");
                break;
            default:
                break;
        }
        TextFinished = false;
        textLabel.text = "";
        for(int i=0;i<textList[LineIndex].Length;i++)
        {
            TextSpeedBetween = Random.Range(0.01f, 0.05f);
            textLabel.text += textList[LineIndex][i];   //在协程中一个字一个字地加，每次显示一个字都会停顿时间TextSpeedBetween
            yield return new WaitForSeconds(TextSpeedBetween);
        }
        TextFinished = true;
        LineIndex++;
        #endregion
    }



    public void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        LineIndex = 0;

        //C#中的var应该和C++中的auto差不多矣
        var lineData = file.text.Split('\n');       //分解成string类型的数组

        foreach(var line in lineData)
        {
            textList.Add(line);
        }
    }

}
