using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bronya;

public class Shotgun : Gun
{
    public int bulletNum = 3;   //由于是霰弹枪，bulletNum表示一次开火发射多少颗子弹
    public float bulletAngle = 15;   //每颗子弹之间的间隔角度


    //霰弹枪与普通枪的区别只在于开火时，所以只需要重写Fire即可
    protected override void Fire()
    {
        SoundManager.Instance.PlaySound("Pistal");
        animator.SetTrigger("Shoot");
        int medium = bulletNum / 2;
        for(int i=0;i<bulletNum;i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = muzzlePos.position;
            if(bulletNum%2==1)  //奇数发子弹
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
