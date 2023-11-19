using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "standStatus", menuName = "StandStatus")]

public class StandStatus : ScriptableObject
{
    [Header("Stand Info")]
    public int jenisStand;
    [SerializeField]
    private int levelStand;
    public string namaStand;
    [SerializeField]
    public int[] jumlahMakanan;
    public int selectedManualCreate;
    public Sprite[] spriteStand;

    [Header("Stand Bonus")]
    [SerializeField]
    private float bonusSpeedSpawn;
    [SerializeField]
    private double bonusPopularity;
    [SerializeField]
    private double bonusCoin;
    [SerializeField]
    private int limitMaksimal;
    [SerializeField]
    private float bonusAutoCreate;
    [SerializeField]
    private int bonusManualCreate;

    [Header("Stand Dekorasi")]
    [SerializeField]
    private int levelDekorasi1;
    public Sprite[] spriteDekorasi1;

    [SerializeField]
    private int levelDekorasi2;
    public Sprite[] spriteDekorasi2;

    [SerializeField]
    private int levelDekorasi3;
    public Sprite[] spriteDekorasi3;

    [SerializeField]
    private int levelDekorasi4;
    public Sprite[] spriteDekorasi4;

    [SerializeField]
    private int levelDekorasi5;
    public Sprite[] spriteDekorasi5;

    [SerializeField]
    private int levelDekorasi6;
    public Sprite[] spriteDekorasi6;

    public int GetLevelStand()
    {
        return levelStand;
    }

    public void UpgradeLevelStand()
    {
        // Lakukan validasi jika diperlukan
        levelStand += 1;
    }

    public void UpgradeAccesoris01()
    {
        levelDekorasi1 += 1;
    }

    public void UpgradeAccesoris02()
    {
        levelDekorasi2 += 1;
    }

    public void UpgradeAccesoris03()
    {
        levelDekorasi3 += 1;
    }

    public void UpgradeAccesoris04()
    {
        levelDekorasi4 += 1;
    }

    public void UpgradeAccesoris05()
    {
        levelDekorasi5 += 1;
    }

    public int GetStandLevel()
    {
        return levelStand;
    }

    public int GetAccesoris01()
    {
        return levelDekorasi1;
    }
    public int GetAccesoris02()
    {
        return levelDekorasi2;
    }
    public int GetAccesoris03()
    {
        return levelDekorasi3;
    }
    public int GetAccesoris04()
    {
        return levelDekorasi4;
    }
    public int GetAccesoris05()
    {
        return levelDekorasi5;
    }

    public float GetBonusSpeedSpawn()
    {
        return bonusSpeedSpawn;
    }

    public double GetBonusPopularity()
    {
        return bonusPopularity;
    }

    public  double GetBonusCoin()
    {
        return bonusCoin;
    }

    public int GetLimitMaksimal()
    {
        return limitMaksimal;
    } 

    public float GetBonusAutoCreate(){
        return bonusAutoCreate;
    }

    public float GetBonusManualCreate()
    {
        return bonusManualCreate;
    }


}
