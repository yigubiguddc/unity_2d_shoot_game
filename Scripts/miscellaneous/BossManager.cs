using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bronya;

public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Boss1;
    [SerializeField] private GameObject[] Boss2;
    [SerializeField] private GameObject[] Boss3;
    [SerializeField] private GameObject[] Boss4;
    [SerializeField] private GameObject wavemanager;
    [SerializeField] private LivingEntity player;
    private void Start()
    {
        for(int i=0;i<=10;i++)
        {
            Boss1[i].SetActive(false);
            Boss2[i].SetActive(false);
            Boss3[i].SetActive(false);
            Boss4[i].SetActive(false);
        }

    }
    private void Update()
    {
        if (player.level == 3)
        {
            Boss1[0].SetActive(true);
        }
        else if (player.level == 6)
        {
            Boss2[0].SetActive(true);
        }
        else if (player.level == 9)
        {
            Boss3[0].SetActive(true);
        }
        else if (player.level == 20)
        {
            Boss4[0].SetActive(true);
        }
        else if (player.level == 40)
        {
            for (int i = 1; i <=2; i++)
            {
                Boss1[i].SetActive(true);
            }
        }
        else if (player.level == 50)
        {
            for (int i = 0; i <= 2 ; i++)
            {
                Boss2[i].SetActive(true);

            }
        }
        else if (player.level == 60)
        {
            for(int i=1;i<=2;i++)
            {
                Boss3[i].SetActive(true);
            }
        }
        else if(player.level == 80)
        {
            for(int i=1;i<=2;i++)
            {
                Boss4[i].SetActive(true);
            }
        }
        else if(player.level == 100)
        {
            for(int i=3;i<=10;i++)
            {
                Boss1[i].SetActive(true);
                Boss2[i].SetActive(true);
                Boss3[i].SetActive(true);
                Boss4[i].SetActive(true);
            }
        }
    }
}
