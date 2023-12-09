using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "userData", menuName = "UserData")]

public class UserData : ScriptableObject
{
    [SerializeField]
    private double coin;

    [SerializeField]
    private int diamond;

    [SerializeField]
    private double popularity;

    public float multipilerCoinBonus;
    public float plusCoinBonus;
    public float multipilerSpeedSpawnBonus;
    public int multipilerSpawn;

    public float multipilerAutoCreateBonus;
    public float multipilerPopularityBonus;

    public bool manualCreateBonus;
    public bool douleAutoCreateBonus;
    public bool createWithOutResource;

    [SerializeField]
    private bool standUnlocked1;
    [SerializeField]
    private bool standUnlocked2;
    [SerializeField]
    private bool standUnlocked3;

    public double GetCoin()
    {
        return coin;
    }

    public void SetCoin(double addcoin)
    {
        coin += addcoin;
    }

    public int GetDiamond()
    {
        return diamond;
    }

    public void SetDiamond(int diamondValue)
    {
        diamond += diamondValue;
    }

    public double GetPopularity()
    {
        return popularity;
    }

    public void SetPopularity(double popularityValue)
    {
        popularity += popularityValue;
    }

    public bool GetUnlockedStand1()
    {
        return standUnlocked1;
    }

    public bool GetUnlockedStand2()
    {
        return standUnlocked2;
    }

    public bool GetUnlockedStand3()
    {
        return standUnlocked3;
    }

    public void SetLoadData(double loadCoin, int loadDiamond, double loadPopularity)
    {
        coin = loadCoin;
        diamond = loadDiamond;
        popularity = loadPopularity;
    }

    public void SetLoadStandUnlock(bool stand1, bool stand2, bool stand3)
    {
        standUnlocked1 = stand1;
        standUnlocked2 = stand2;
        standUnlocked3 = stand3;
    }

    public void ResetStatus()
    {
        coin = 0;
        popularity = 0;
        multipilerCoinBonus = 1;
        plusCoinBonus = 0;
        multipilerSpeedSpawnBonus = 1;
        multipilerSpawn = 1;

        standUnlocked1 = true;
        standUnlocked2 = false;
        standUnlocked3 = false;
}

    public void SetLoadStatusData(float loadmultipilerCoinBonus, float loadplusCoinBonus, float loadmultipilerSpeedSpawnBonus, int loadmultipilerSpawn, float loadmultipilerAutoCreateBonus, float loadmultipilerPopularityBonus, 
        bool loadmanualCreateBonus, bool loaddouleAutoCreateBonus, bool loadcreateWithOutResource)
    {
        multipilerCoinBonus = loadmultipilerCoinBonus;
        plusCoinBonus = loadplusCoinBonus;
        multipilerSpeedSpawnBonus = loadmultipilerSpeedSpawnBonus;
        multipilerSpawn = loadmultipilerSpawn;

        multipilerAutoCreateBonus = loadmultipilerAutoCreateBonus;
        multipilerPopularityBonus = loadmultipilerPopularityBonus;

        manualCreateBonus = loadmanualCreateBonus;
        douleAutoCreateBonus = loaddouleAutoCreateBonus;
        createWithOutResource = loadcreateWithOutResource;
    }
}
