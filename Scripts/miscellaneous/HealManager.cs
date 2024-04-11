using System.Collections;
using UnityEngine;

public class HealManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] HealBoxes;
    [SerializeField] private LivingEntity player;
    public GameObject heals;

    private int TMPlevel = 2;
    /*private void Awake()
    {
        Wavemanager.HealTips += HealGet;
    }

    private void HealGet()
    {
        for (int i = 0; i < 10; i++)
        {
            heals = ObjectPool.Instance.GetObject(HealBoxes[Random.Range(0, 5)]);
            heals.transform.position = spawnPoints[Random.Range(0, 7)].position;
        }
    }


    private void OnDestroy()
    {
        Wavemanager.HealTips -= HealGet; ;
    }*/

    private void Update()
    {
        if(player.level == TMPlevel)
        {
            for (int i = 0; i < 10; i++)
            {
                heals = ObjectPool.Instance.GetObject(HealBoxes[Random.Range(0, 5)]);
                heals.transform.position = spawnPoints[Random.Range(0, 7)].position;
            }
            TMPlevel++;
        }
    }
}
