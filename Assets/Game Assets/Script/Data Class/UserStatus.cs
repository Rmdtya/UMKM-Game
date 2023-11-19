using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserStatus : MonoBehaviour
{

    [SerializeField]
    private UserData userData;

    public static UserStatus instance;

    public string userDisplayName;

    public TextMeshProUGUI textDisplayName;
    public TextMeshProUGUI textJumlahCoin;
    public TextMeshProUGUI textJumlahDiamond;
    public TextMeshProUGUI textPularity;

    private float deltaTime = 0.0f;


    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {

    }


    void Start()
    {
        textJumlahCoin.text = userData.GetCoin().ToString();
        textJumlahDiamond.text = userData.GetDiamond().ToString();
        textPularity.text = userData.GetPopularity().ToString();
    }

    public void CallAddCoin(double plusCoin)
    {
        AddCoin(plusCoin);
    }

    private void AddCoin(double plusCoin)
    {
        userData.SetCoin(plusCoin);
        textJumlahCoin.text = userData.GetCoin().ToString();
    }

    public void CallAddDiamond(int plusDiamond)
    {
        AddDiamond(plusDiamond);
    }

    private void AddDiamond(int plusDiamond)
    {
        userData.SetDiamond(plusDiamond);
        textJumlahDiamond.text = userData.GetDiamond().ToString();
    }

    public void SetPopularityPoint(double popularityValue)
    {
        userData.SetPopularity(popularityValue);
        textPularity.text = FormatForPupularity(userData.GetPopularity());
    }

    string FormatForPupularity(double value)
    {
        if (value < 1000)
        {
            return value.ToString();
        }
        else if (value < 1000000)
        {
            double valueInK = value / 1000;
            return valueInK.ToString("F1") + "k";
        }
        else
        {
            double valueInM = value / 1000000;
            return valueInM.ToString("F1") + "jt";
        }
    }

    public UserData GetUserdata()
    {
        return userData;
    }

    public float GetBonusSpawn()
    {
        return userData.multipilerSpeedSpawnBonus;
    }

    public bool GetUnlockedStand1()
    {
        return userData.GetUnlockedStand1();
    }

    public bool GetUnlockedStand2()
    {
        return userData.GetUnlockedStand2();
    }

    public bool GetUnlockedStand3()
    {
        return userData.GetUnlockedStand3();
    }

    public bool GetDouleAutoCreateBonus()
    {
        return userData.douleAutoCreateBonus;
    }

}
