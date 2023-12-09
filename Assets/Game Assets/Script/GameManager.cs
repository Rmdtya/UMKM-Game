using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private LoadSaveData loadSaveData;

    [SerializeField]
    private int activeStand;
    public bool popupActive;

    public bool popupStandActive;
    public bool popupResepActive;
    public bool popupStatisticActive;
    public bool popupstorageActive;
    public bool popupMarket;

    public bool popupCreateMakanan;

    [SerializeField]
    private Stand[] standScript;

    public Waktu waktu;

    public enum Waktu
    {
        pagi,
        siang,
        sore,
        malam,
    }


    private void Awake()
    {
        instance = this;
        QualitySettings.vSyncCount = 0; // Matikan V-Sync
        Application.targetFrameRate = 120; // Kunci FPS ke 60
    }


    void Start()
    {
        activeStand = 1;
        popupActive = false;
        LoadTime();
        LoadStand();
    }

    public void UpdateStandActive(int active)
    {
        activeStand = active;
        UIManager.instance.ShowHideUIPanel(true);

        if (active == 1)
            return;

        if(active == 2)
        {
            if (!UserStatus.instance.GetUnlockedStand2())
            {
                UIManager.instance.ShowHideUIPanel(false);
            }
        }else if (active == 3)
        {
            if (!UserStatus.instance.GetUnlockedStand3())
            {
                UIManager.instance.ShowHideUIPanel(false);
            }
        }

    }

    public int GetStandActive()
    {
        return activeStand;
    }

    public Stand GetStandScriptActive()
    {
        return standScript[activeStand - 1];
    }

    public Stand[] GetAllStandScript()
    {
        return standScript;
    }

    public void LoadTime()
    {
        DateTime currentTime = DateTime.Now;

        int currentHour = currentTime.Hour;

        if (currentHour >= 5 && currentHour < 12)
        {
            waktu = Waktu.pagi;
        }
        else if (currentHour >= 12 && currentHour <= 15)
        {
            waktu = Waktu.siang;
        }
        else if (currentHour >= 16 && currentHour < 18)
        {
            waktu = Waktu.sore;
        }
        else
        {
            waktu = Waktu.malam;
        }

        UIManager.instance.SetBackgroundSky(waktu);
    }

    public void LoadStand()
    {
        int jumlah = standScript.Length;
        bool[] statusStand = new bool[jumlah];

        UserData data = UserStatus.instance.GetUserdata();

        statusStand[0] = data.GetUnlockedStand1();
        statusStand[1] = data.GetUnlockedStand2();
        statusStand[2] = data.GetUnlockedStand3();

        UIManager.instance.ShowStand(statusStand);

    }

    public void PopupActive(bool kondisi)
    {
        popupActive = kondisi;
    }
}

