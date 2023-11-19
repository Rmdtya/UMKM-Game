using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private int activeStand;
    public bool popupActive;

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
        GetWaktu();
    }


    void Start()
    {
        activeStand = 1;
        popupActive = false;
    }

    public void UpdateStandActive(int active)
    {
        activeStand = active;
    }

    public int GetStandActive()
    {
        return activeStand;
    }

    private void GetWaktu()
    {
        SetWaktu();
    }

    private void SetWaktu()
    {
        uiManager.SetBackgroundSky(waktu);
    }
}

