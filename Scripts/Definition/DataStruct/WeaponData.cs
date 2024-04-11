using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData
{
    public int Damage { get; set; }
    public int ClipSize { get; set; }   //弹夹容量
    public int FireRate { get; set; }    //射速
    public int CriticalChance { get; set; }   //暴击率
    public float ReloadTime { get; set; }//装填时间


    //构造函数
    public WeaponData(int damage,int clipsize,int firerate,int criticalchance,float reloadtime)
    {
        Damage = damage;
        ClipSize = clipsize;
        FireRate = firerate;
        CriticalChance = criticalchance;
        ReloadTime = reloadtime;
    }

}
