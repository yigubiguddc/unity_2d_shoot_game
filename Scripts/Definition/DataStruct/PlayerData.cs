using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int Vitality { get; set; }   //������
    public int Agility  { get; set; }    //���ݵ�
    public int Accuracy { get; set; }   //׼��׼

    //��׼��ƫ��ֵ

    public WeaponData WeaponData { get; set;  }


    //�����ӵ�ʱ������AccuracyֵԽ�ӽ�8.0f���ߵľ�Խ׼
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



    //�޲������캯��
    public PlayerData()
    {
        Vitality = 100;
        Agility = 80;
        Accuracy = 50;
        WeaponData = new WeaponData(10,1000,10,30,1.0f);
    }

    /// <summary>
    /// �����˺��Ƿ񱩻�
    /// </summary>
    /// <returns>����˺��Ƿ񱩻�</returns>
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
