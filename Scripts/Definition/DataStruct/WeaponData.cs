using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData
{
    public int Damage { get; set; }
    public int ClipSize { get; set; }   //��������
    public int FireRate { get; set; }    //����
    public int CriticalChance { get; set; }   //������
    public float ReloadTime { get; set; }//װ��ʱ��


    //���캯��
    public WeaponData(int damage,int clipsize,int firerate,int criticalchance,float reloadtime)
    {
        Damage = damage;
        ClipSize = clipsize;
        FireRate = firerate;
        CriticalChance = criticalchance;
        ReloadTime = reloadtime;
    }

}
