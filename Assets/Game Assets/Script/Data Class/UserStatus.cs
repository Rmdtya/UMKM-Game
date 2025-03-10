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


    public CustomerStatus[] defaultCustomer;

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
        textJumlahCoin.text = FormatRupiah(userData.GetCoin());
        textJumlahDiamond.text = userData.GetDiamond().ToString();
        textPularity.text = userData.GetPopularity().ToString("F0");
    }

    public void CallAddCoin(double plusCoin)
    {
        AddCoin(plusCoin);
    }

    private void AddCoin(double plusCoin)
    {
        userData.SetCoin(plusCoin);
        textJumlahCoin.text = FormatRupiah(userData.GetCoin());
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

    public void CheatPopularity()
    {
        userData.SetPopularity(1000);
        textPularity.text = FormatForPupularity(userData.GetPopularity());
    }

    string FormatForPupularity(double value)
    {
        if (value < 1000)
        {
            return value.ToString("F0");
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

    public int GetDiamondValue()
    {
        return userData.GetDiamond();
    }

    public double GetCoinValue()
    {
        return userData.GetCoin();
    }

    public double GetPopularity()
    {
        return userData.GetPopularity();
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

    public string FormatRupiah(double total)
    {
        string formatRupiah = string.Format("{0:N0}", total);
        return formatRupiah;
    }
    public void UnlockStand()
    {
        Stand[] standScript = GameManager.instance.GetAllStandScript();

        if (!userData.GetUnlockedStand2()){
           if(standScript[0].GetLevelStand() >= 2)
            {
                userData.SetLoadStandUnlock(true, true, false);
                UIManager.instance.ShowUnlockedStand(2);

                ResourceStorage.instance.UnlockStand2(100);
                defaultCustomer[0].UnlockStatus(0);

                standScript[1].UnlockFirstFood();
            }
        }

        if (!userData.GetUnlockedStand3())
        {
            if (standScript[0].GetLevelStand() >= 3 && standScript[1].GetLevelStand() >= 3)
            {
                userData.SetLoadStandUnlock(true, true, true);
                UIManager.instance.ShowUnlockedStand(3);
                standScript[2].UnlockFirstFood();
            }
        }

        if(standScript[1].GetLevelStand() == 2)
        {
            defaultCustomer[0].UnlockStatus(1);
        }

        if (standScript[2].GetLevelStand() == 2)
        {
            defaultCustomer[0].UnlockStatus(2);
        }


        if (standScript[0].GetLevelStand() == 4)
        {
            defaultCustomer[1].UnlockStatus(0);
        }

        if (standScript[1].GetLevelStand() == 4)
        {
            defaultCustomer[1].UnlockStatus(1);
        }

        if (standScript[2].GetLevelStand() == 4)
        {
            defaultCustomer[1].UnlockStatus(2);
        }

        SpawnPeople.instance.ReinvokeSpawn();
    }

    public bool kurangiCoin(double harga)
    {
        if(userData.GetCoin() >= harga)
        {
            userData.SetCoin(-1 * harga);
            textJumlahCoin.text = userData.GetCoin().ToString("F0");
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetTotalUnlockStand()
    {
        int i = 0;
        if (GetUnlockedStand1())
        {
            i++;
        }

        if (GetUnlockedStand2())
        {
            i++;
        }

        if (GetUnlockedStand3())
        {
            i++;
        }

        return i;
    }

}
