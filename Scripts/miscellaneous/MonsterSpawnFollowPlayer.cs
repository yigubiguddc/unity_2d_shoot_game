using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform PlayerBody;
    private void Update()
    {
        if(PlayerBody == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = PlayerBody.position;
        }

    }
}
