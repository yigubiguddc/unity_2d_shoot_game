using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashStone : MonoBehaviour
{
    [SerializeField] private Transform flashTransform;
    [SerializeField] private GameObject WaveManager;

    private void Start()
    {
        // �����¼�
        Knight.OnPressedIKey += HandleIPressed;
    }
    private void HandleIPressed()
    {
        // �� "I" ��������ʱִ�еĴ���
        gameObject.SetActive(false);
        flashTransform.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        // ����������ʱȡ�����ģ��Ա���Ǳ�ڵ��ڴ�й©��������쳣
        Knight.OnPressedIKey -= HandleIPressed;
    }

}
