using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("PopUp Windows")]
    public GameObject standPopupUI;
    public GameObject marketUI;
    public GameObject storageUI;

    private bool standPopupCondition;

    private bool marketPopup;
    private bool storagePopup;
    private bool buyMarketPopup;

    public GameObject[] standUI;

    [Header("Sky Background")]
    public Image skyBackground;
    public Sprite[] skyBG;
    public GameObject lampu;

    private void Awake()
    {

    }

    void Start()
    {
        marketUI.SetActive(false);
        storageUI.SetActive(false);
        standPopupUI.SetActive(false);

        marketPopup = false;
        storagePopup = false;
        buyMarketPopup = false;

        standPopupCondition = false;

    }

    public void ShowMarket()
    {
        marketUI.SetActive(true);
        marketPopup = true;
        GameManager.instance.popupActive = true;
    }

    public void ShowStrorage()
    {
        storageUI.SetActive(true);
        storagePopup = true;
        GameManager.instance.popupActive = true;
    }

    public void ShowStandPopup()
    {
        standPopupUI.SetActive(true);
        standPopupCondition = true;
        GameManager.instance.popupActive = true;
        HideShowStandUI(false);
    }

    public void HideStandPopup()
    {
        standPopupUI.SetActive(false);
        standPopupCondition = false;
        GameManager.instance.popupActive = false;
        HideShowStandUI(true);
    }

    private void OnDestroy()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (marketPopup)
            {
                marketUI.SetActive(false);
                marketPopup = false;
                GameManager.instance.popupActive = false;
            }
            else if (storagePopup)
            {
                storageUI.SetActive(false);
                storagePopup = false;
                GameManager.instance.popupActive = false;
            }
        }
    }

    public void BackButton()
    {
        if (marketPopup)
        {
            marketUI.SetActive(false);
            marketPopup = false;
            GameManager.instance.popupActive = false;
        }
        else if (storagePopup)
        {
            storageUI.SetActive(false);
            storagePopup = false;
            GameManager.instance.popupActive = false;
        }
    }

    private void HideShowStandUI(bool kondition)
    {
        for(int i = 0; i < standUI.Length; i++)
        {
            standUI[i].SetActive(kondition);
        }
    }

    public void SetBackgroundSky(GameManager.Waktu waktu)
    {
        if(waktu == GameManager.Waktu.pagi)
        {
            skyBackground.sprite = skyBG[0];
            lampu.SetActive(false);
        }
        else if(waktu == GameManager.Waktu.siang)
        {
            skyBackground.sprite = skyBG[1];
            lampu.SetActive(false);
        }
        else if (waktu == GameManager.Waktu.sore)
        {
            skyBackground.sprite = skyBG[2];
            lampu.SetActive(false);
        }
        else if (waktu == GameManager.Waktu.malam)
        {
            skyBackground.sprite = skyBG[3];
            lampu.SetActive(true);
        }
    }

}
