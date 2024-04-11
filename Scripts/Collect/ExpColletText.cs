using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpColletText : MonoBehaviour
{
    [SerializeField] private Transform[] ExpPrefabs;

    private void Start()
    {
        float fX = transform.position.x;
        float fY = transform.position.y;

        foreach(var Exps in ExpPrefabs)
        {
            float Randomvalue = Random.Range(-10f, 10f);
            if(!Exps.CompareTag("Player"))
            {
                float RandomX, RandomY;
                RandomX = fX + Randomvalue;
                RandomY = fY + Randomvalue;
                //随机赋值，分散在周围
                Exps.position = new Vector2(RandomX,RandomY);
            }
        }
    }



}
