using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePauseForm : MonoBehaviour
{
    [SerializeField] private GameObject Masks;
    [SerializeField] private Button Continue;
    [SerializeField] private Button menuButton;
    //[SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;


    private void Awake()
    {
        Masks.SetActive(false);
        //The button could get function by the use of "AddListener"
        Continue.onClick.AddListener(OnContinueButtonClick);
        menuButton.onClick.AddListener(OnMenuButtonClick);
        //settingButton.onClick.AddListener(OnSettingButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }
    private void OnEnable()
    {
        GameEvents.GamePause += GamePause;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Masks.activeSelf)
        {
            OnContinueButtonClick();
        }
    }

    private void GamePause()
    {
        Masks.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnContinueButtonClick()
    {
        Masks.SetActive(false);
        Time.timeScale = 1.0f;
    }
    private void OnMenuButtonClick()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1.0f;
    }

   /* private void OnSettingButtonClick()
    {
        Debug.Log("no such things!");
        Time.timeScale = 1.0f;
        //SceneManager.LoadScene("Main");
    }*/
    private void OnQuitButtonClick()
    {
        Application.Quit();
        //宏定义平台判断
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;     //相当于在unity中点一下播放按钮

#endif
    }
    private void OnDisable()
    {
        GameEvents.GamePause -= GamePause;
    }
}
