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
        diamond += diamond;
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
}
