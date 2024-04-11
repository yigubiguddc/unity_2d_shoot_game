using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int Vitality { get; set; }   //生命力
    public int Agility  { get; set; }    //敏捷点
    public int Accuracy { get; set; }   //准不准

    //瞄准的偏移值

    public WeaponData WeaponData { get; set;  }


    //升级加点时，这里Accuracy值越接近8.0f，蛇的就越准
    public float AimOffset => 1f / Accuracy * 8.0f;
    public float MoveSpeed => 2f + Agility / 30.0f;
    public float Health => Vitality;


    public int Damage 
    {   
        get => WeaponData.Damage;
        set => WeaponData.Damage = value;
    }
    public int ClipSize
    {
        get => WeaponData.ClipSize;
        set => WeaponData.ClipSize = value;
    }
    public int FireRate
    {
        get => WeaponData.FireRate;
        set => WeaponData.FireRate = value;
    }
    public int CriticalChance
    {
        get => WeaponData.CriticalChance;
        set => WeaponData.CriticalChance = Mathf.Clamp(value, 0, 100);
    }

    public float ReloadTime
    {
        get =>WeaponData.ReloadTime;
        set => Mathf.Clamp(value, 0, 10000);
    }



    //无参数构造函数
    public PlayerData()
    {
        Vitality = 100;
        Agility = 80;
        Accuracy = 50;
        WeaponData = new WeaponData(10,1000,10,30,1.0f);
    }

    /// <summary>
    /// 计算伤害是否暴击
    /// </summary>
    /// <returns>获得伤害是否暴击</returns>
    public (int, bool) CalculateDamage()
    {
        float damage = Damage + Damage * Random.Range(-0.1f, 0.1f);
        if (Random.Range(0, 101) > CriticalChance)
        {
            //no critical hit
            return ((int)damage, false);
        }
        damage *= Random.Range(2.0f, 3.0f);
        return ((int)damage, true);
    }

}
