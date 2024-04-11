using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Bronya;

public class GameOverForm : MonoBehaviour
{
    //TODO Tween
    [SerializeField] private GameObject Masks;
    [SerializeField] private Button menuButton;
    //[SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        Masks.SetActive(false);
        menuButton.onClick.AddListener(OnMenuButtonClick);
      //  restartButton.onClick.AddListener(OnRestartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }


    private void OnEnable()
    {
        GameEvents.GameOver += GameOver;
    }


    private void OnMenuButtonClick()
    {
        //Deactivate prefabs.
        ObjectPool.Instance.DeactivateAllObjects();
        SceneManager.LoadScene("Menu");
    }

    /*private void OnRestartButtonClick()
    {

        SceneManager.LoadScene("Main");
        Time.timeScale = 1.0f;
    }*/
    private void OnQuitButtonClick()
    {
        Application.Quit();
        //宏定义判断平台
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;     //相当于在unity中点一下播放按钮

#endif
    }

    private void GameOver()
    {
        Masks.SetActive(true);
    }


    private void OnDisable()
    {
        GameEvents.GameOver -= GameOver;
    }

}
