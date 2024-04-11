using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashStone : MonoBehaviour
{
    [SerializeField] private Transform flashTransform;
    [SerializeField] private GameObject WaveManager;

    private void Start()
    {
        // 订阅事件
        Knight.OnPressedIKey += HandleIPressed;
    }
    private void HandleIPressed()
    {
        // 当 "I" 键被按下时执行的代码
        gameObject.SetActive(false);
        flashTransform.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        // 当对象被销毁时取消订阅，以避免潜在的内存泄漏或空引用异常
        Knight.OnPressedIKey -= HandleIPressed;
    }

}
