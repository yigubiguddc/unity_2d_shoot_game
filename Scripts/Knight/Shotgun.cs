using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bronya;

public class Shotgun : Gun
{
    public int bulletNum = 3;   //����������ǹ��bulletNum��ʾһ�ο�������ٿ��ӵ�
    public float bulletAngle = 15;   //ÿ���ӵ�֮��ļ���Ƕ�


    //����ǹ����ͨǹ������ֻ���ڿ���ʱ������ֻ��Ҫ��дFire����
    protected override void Fire()
    {
        SoundManager.Instance.PlaySound("Pistal");
        animator.SetTrigger("Shoot");
        int medium = bulletNum / 2;
        for(int i=0;i<bulletNum;i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = muzzlePos.position;
            if(bulletNum%2==1)  //�������ӵ�
            {
                bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - medium), Vector3.forward) * direction);
            }
            else
            {
                bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - medium) + bulletAngle / 2, Vector3.forward) * direction);
            }
        }
        for (int i = 0; i < bulletNum; i++)
        {
            GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
            shell.transform.position = shellPos.position;
            shell.transform.rotation = shellPos.rotation;
        }
    }


}
