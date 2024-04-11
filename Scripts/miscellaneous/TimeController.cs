using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0f;

    float defaultFixedDeltaTime;

    private void Awake()
    {
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            BulletTime();

        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            BulletTimeOver();
        }
    }

    public void BulletTime()
    {
        Time.timeScale = bulletTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
    }

    public void BulletTimeOver()
    {
        Time.timeScale = 1.0f;
    }
}