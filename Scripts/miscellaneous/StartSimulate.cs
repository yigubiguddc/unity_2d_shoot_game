using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bronya;

public class StartSimulate : MonoBehaviour
{
    [SerializeField] private GameObject WaveManager;
    [SerializeField] private GameObject Knight;
    [SerializeField] private GameObject Red;
    [SerializeField] private GameObject WoddenTips;
    private Collider2D cl2d;
    private void Start()
    {
        cl2d = GetComponent<Collider2D>();
        WaveManager.SetActive(false);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            WaveManager.SetActive(true);
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlayMusic("Main1");
            Knight.SetActive(false);
            Red.SetActive(false);
            WoddenTips.SetActive(false);
            cl2d.gameObject.SetActive(false);
        }
    }
}
