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
    public double[] hargaUpgradeStand;

    [Header("Stand Bonus")]
    [SerializeField]
    private float bonusSpeedSpawn;
    [SerializeField]
    private float bonusPopularity;
    [SerializeField]
    private double bonusCoin;
    [SerializeField]
    private int limitMaksimal;
    [SerializeField]
    private float bonusAutoCreate;
    [SerializeField]
    private float bonusManualCreate;

    [Header("Stand Dekorasi")]
    [SerializeField]
    private Dekorasi[] dekorasiStand;

    public int GetLevelStand()
    {
        return levelStand;
    }

    public void UpgradeLevelStand()
    {
        levelStand += 1;
        UserStatus.instance.UnlockStand();
    }

    public int GetStandLevel()
    {
        return levelStand;
    }

    public float GetBonusSpeedSpawn()
    {
        return bonusSpeedSpawn;
    }

    public float GetBonusPopularity()
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

    public int GetTotalJumlahMakanan()
    {
        int jumlah = 0;
        for(int i=0; i < jumlahMakanan.Length; i++)
        {
            jumlah += jumlahMakanan[i];
        }

        return jumlah;
    }

    public Dekorasi[] GetStandDekorasi()
    {
        return dekorasiStand;
    }

    public Dekorasi GetSpesifikStandDekorasi(int i)
    {
        return dekorasiStand[i];
    }

    public int GetSpesifikasiLevelDekorasi(int i)
    {
        return dekorasiStand[i].GetAccesorisLevel();
    }

    public void UpgradeDekorasi(int index, int standLevel)
    {
        dekorasiStand[index].UpgradedDekorasi();
        dekorasiStand[index].hargaUpgrade = dekorasiStand[index].hargaUpgrade * dekorasiStand[index].hargaKelipatan[standLevel];
        dekorasiStand[index].totalBonus += dekorasiStand[index].bonusKelipatan[standLevel];

        if(index == 0)
        {
            bonusPopularity += dekorasiStand[index].bonusKelipatan[standLevel];
        }else if(index == 1)
        {
            ResourceStorage.instance.TambahLimitMaksimal(dekorasiStand[index].bonusKelipatan[standLevel]);
        }else if(index == 2)
        {
            bonusSpeedSpawn += dekorasiStand[index].bonusKelipatan[standLevel];
        }else if(index == 3)
        {
            limitMaksimal += (int)dekorasiStand[index].bonusKelipatan[standLevel];
        }else if(index == 4)
        {
            bonusAutoCreate += dekorasiStand[index].bonusKelipatan[standLevel];
        }
        else if(index == 5)
        {
            bonusCoin += dekorasiStand[index].bonusKelipatan[standLevel];
        }
    }

    public void SetLoadData(int loadJenis, int loadlevelStand, string loadnamaStand, int[] loadjumlahMakanan, int loadselectedManualCreate,
        float loadbonusSpeedSpawn, float loadbonusPopularity, double loadbonusCoin, int loadlimitMaksimal, float loadbonusAutoCreate, float loadbonusManualCreate, 
        int[] loadLevelDekorasi, double[] loadhargaUpgrade, float[] loadTotalBunus)
    {
        jenisStand = loadJenis;
        levelStand = loadlevelStand;
        namaStand = loadnamaStand;

        for(int i = 0; i < loadjumlahMakanan.Length; i++)
        {
            jumlahMakanan[i] = loadjumlahMakanan[i];
        }

        selectedManualCreate = loadselectedManualCreate;

        bonusSpeedSpawn = loadbonusSpeedSpawn;
        bonusPopularity = loadbonusPopularity;
        bonusCoin = loadbonusCoin;
        limitMaksimal = loadlimitMaksimal;
        bonusAutoCreate = loadbonusAutoCreate;
        bonusManualCreate = loadbonusManualCreate;

        for(int j = 0; j < loadLevelDekorasi.Length; j++)
        {
            dekorasiStand[j].SetDekorasiLevel(loadLevelDekorasi[j]);

            dekorasiStand[j].hargaUpgrade = loadhargaUpgrade[j];
            dekorasiStand[j].totalBonus = loadTotalBunus[j];
        }
    }

    public void ResetData()
    {
        levelStand = 1;
        bonusSpeedSpawn = 1f;
        bonusPopularity = 1f;
        bonusCoin = 0;
        limitMaksimal = 100;
        bonusAutoCreate = 1f;
        bonusManualCreate = 1f;

        for(int i = 0; i < dekorasiStand.Length; i++)
        {
            dekorasiStand[i].SetDekorasiLevel(1);
            dekorasiStand[i].totalBonus = 0;
        }

        for(int j = 0; j < jumlahMakanan.Length; j++)
        {
            jumlahMakanan[j] = 0;
        }
    }
}

[System.Serializable]
public struct Dekorasi
{
    public string namaDekorasi;
    [SerializeField]
    private int levelDekorasi;
    public Sprite[] spriteDekorasi;
    public double hargaUpgrade;
    public string bonusEfek;
    public string deskripsiDekorasi;
    public float[] hargaKelipatan;
    public float[] bonusKelipatan;
    public float totalBonus;

    public int GetAccesorisLevel()
    {
        return levelDekorasi;
    }

    public void SetDekorasiLevel(int level)
    {
        levelDekorasi = level;
    }

    public void UpgradedDekorasi()
    {
        levelDekorasi += 1;
    }
}
