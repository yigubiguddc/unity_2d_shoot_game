using System.Collections;
using Bronya;
using UnityEngine;

public class RocketLauncher : Gun
{
    public int rocketNum = 3;
    public float rocketAngle = 15;


    protected override void Fire()
    {
        SoundManager.Instance.PlaySound("Rocket");
        animator.SetTrigger("Shoot");
        StartCoroutine(DelayFire(.2f));
    }

    IEnumerator DelayFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        int medium = rocketNum / 2;
        for (int i = 0; i < rocketNum; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = muzzlePos.position;

            if (rocketNum % 2 == 1)  //ÆæÊý·¢×Óµ¯
            {
                bullet.transform.right = Quaternion.AngleAxis(rocketAngle * (i - medium), Vector3.forward) * direction;
            }
            else
            {
                bullet.transform.right = Quaternion.AngleAxis(rocketAngle * (i - medium) + rocketAngle / 2, Vector3.forward) * direction;
            }
            bullet.GetComponent<Rocket>().SetTarget(mousePos);
        }

    }
}

