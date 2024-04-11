using UnityEngine;
using UnityEngine.UI;
using Bronya;
public class MusicSound : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }


    public void ValueChangeCheck()
    {
        SoundManager.Instance.ChangeMusicVolume(slider.value);
        SoundManager.Instance.ChangeSoundVolume(slider.value);
    }

}
