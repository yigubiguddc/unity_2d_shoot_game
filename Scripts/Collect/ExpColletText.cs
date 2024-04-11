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
                //�����ֵ����ɢ����Χ
                Exps.position = new Vector2(RandomX,RandomY);
            }
        }
    }



}
