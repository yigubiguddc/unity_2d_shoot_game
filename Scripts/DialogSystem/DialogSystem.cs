using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class DialogSystem : MonoBehaviour
{
    [Header("ͷ��")]
    public Sprite head1;
    public Sprite head2;

    [Header("UI���")]
    public Text textLabel;
    public Image faceImage;

    [Header("�ı��ļ�")]
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
                gameObject.SetActive(false);    //�ر�Panel��Ȼ������LineIndex
                LineIndex = 0;
                return;
            }
            else if(TextFinished)
            {
                StartCoroutine(SetTextUI());    //ȷ��Э�̲�ͬ����ʼ
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
                    textLabel.text += textList[LineIndex][i];   //��Э����һ����һ���ֵؼӣ�ÿ����ʾһ���ֶ���ͣ��ʱ��TextSpeedBetween
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
                    textLabel.text += textList[LineIndex][i];   //��Э����һ����һ���ֵؼӣ�ÿ����ʾһ���ֶ���ͣ��ʱ��TextSpeedBetween
                    yield return new WaitForSeconds(TextSpeedBetween);
                }
                TextFinished = true;
                LineIndex++;
                break;
        }
        */
        #region "ʧЧ����"
        //����������֮���������������ַ���ʧЧ�ˣ��Һ����ߣ�����
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
            textLabel.text += textList[LineIndex][i];   //��Э����һ����һ���ֵؼӣ�ÿ����ʾһ���ֶ���ͣ��ʱ��TextSpeedBetween
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

        //C#�е�varӦ�ú�C++�е�auto�����
        var lineData = file.text.Split('\n');       //�ֽ��string���͵�����

        foreach(var line in lineData)
        {
            textList.Add(line);
        }
    }

}
