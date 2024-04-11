using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainForm : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image expBar;

    //使用这两个不需要销毁它，因为是常用的UI界面，直接使用OnEnable和OnDisable
    private void OnEnable()
    {
        GameEvents.OnPlayerHealthChange += OnPlayerHealthChange;
        GameEvents.OnPlayerExpChange += OnPlayerExpChange;
    }

    private void OnPlayerHealthChange(float currentHealth,float startHealth)
    {
        healthBar.fillAmount = currentHealth / startHealth;
        healthText.SetText($"{currentHealth}/{startHealth}");
        if (currentHealth < 0)
        {
            healthText.SetText($"0/{startHealth}");
        }
    }

    //两个参数分别时传入的Exp和maxExp
    private void OnPlayerExpChange(float currentExp,float maxExp)
    {
        expBar.fillAmount = currentExp / maxExp;
        //Debug.Log($"{currentExp}/{maxExp}");
    }


    private void OnDisable()
    {
        GameEvents.OnPlayerHealthChange -= OnPlayerHealthChange;
        GameEvents.OnPlayerExpChange -= OnPlayerExpChange;
    }


}
