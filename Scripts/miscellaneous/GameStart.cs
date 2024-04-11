using UnityEngine;
using UnityEngine.SceneManagement;
using Bronya;


public class GameStart : MonoBehaviour
{
    private void Awake()
    {
        //����Ϸ������ת������������
        DontDestroyOnLoad(this);    //���Զ�����һ���ļ�����ΪDontDestroyOnLoad�������������ƶ���������·�

        //Init UIManager -> hitfix
        //Load configs
        //Init dataTables;
        //Init maps and so on...
        SceneManager.LoadScene("Menu");
        SceneManager.activeSceneChanged += OnactiveSceneChanged;
    }


    private void OnactiveSceneChanged(Scene previousActiveScene,Scene newActiveScene)
    {
         SoundManager.Instance.PlayMusic(newActiveScene.name);
    }

    //����ֱ�Ӵ�Game��������Ϸ����Ϊ��ʱ��Ϸ��Դ��������û�м��أ���������GameStart������ʼ
    //һ�����Ե�������unity���п�ʼʱ�������������µķ���
    [RuntimeInitializeOnLoadMethod]
    public static void OnGameLoaded()
    {
        if (SceneManager.GetActiveScene().name == "GameStart")
        {
            return;
        }
        SceneManager.LoadScene("GameStart");    //Ϊ�˱�֤��Ϸÿ�ζ���GameStart��ʼ
    }

}
