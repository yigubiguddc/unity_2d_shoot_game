using UnityEngine;
using UnityEngine.SceneManagement;
using Bronya;


public class GameStart : MonoBehaviour
{
    private void Awake()
    {
        //让游戏物体跳转场景不被销毁
        DontDestroyOnLoad(this);    //会自动创建一个文件夹名为DontDestroyOnLoad，将所有物体移动到这个的下方

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

    //不能直接从Game场景打开游戏，因为这时游戏资源管理器都没有加载，必须从这个GameStart场景开始
    //一下特性的作用是unity运行开始时会调用这个特性下的方法
    [RuntimeInitializeOnLoadMethod]
    public static void OnGameLoaded()
    {
        if (SceneManager.GetActiveScene().name == "GameStart")
        {
            return;
        }
        SceneManager.LoadScene("GameStart");    //为了保证游戏每次都从GameStart开始
    }

}
