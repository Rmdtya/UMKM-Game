using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static UIManager instance;
    public static UnityEvent<string> OnGamePlayNotification = new UnityEvent<string>();

    public static UnityEvent<string> BottonNotification = new UnityEvent<string>();

    [Header("Stand UI")]
    public GameObject[] standObject;
    public CameraSwifeMovement mainCamera;
    public GameObject[] UIPanel;
    

    [Header("PopUp Windows")]
    public GameObject standPopupUI;
    public GameObject resepPopupUI;
    public GameObject marketUI;
    public GameObject storageUI;
    public GameObject statistikUI;

    public Animator _UIAnimator;

    private bool standPopupCondition;

    private bool marketPopup;
    private bool storagePopup;
    private bool buyMarketPopup;

    public GameObject[] standUI;

    [Header("Sky Background")]
    public Image skyBackground;
    public Sprite[] skyBG;
    public GameObject lampu;

    [Header("Popup Upgrade Stand")]
    public GameObject popupStandUpgrade;
    public Image standImage;
    public TextMeshProUGUI textLevelStand;
    public TextMeshProUGUI textNamaStand;
    public TextMeshProUGUI textHarga;
    public Button upgradeStandButton;
    public Image imageButton;
    public Sprite[] buttonSprite;

    [Header("Popup Create Makanan")]
    public GameObject popupCreateMakanan;
    public PopupCreateMakanan popupCreateScript;

    private Stand standScriptActive;
    private double hargaUpgrade;

    [Header("PopupNotification")]
    public GameObject GameplayNotification;
    public GameObject bottomNotification;

    public bool isGameplayNotification;
    public bool isBottomNotification;

    private void Awake()
    {
        instance = this;
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

        isGameplayNotification = false;

        popupStandUpgrade.SetActive(false);
    }

    public void ShowStand(bool[] standUnlock)
    {
        for(int i=0; i < standObject.Length; i++)
        {
            standObject[i].SetActive(standUnlock[i]);
        }
    }

    public void ShowHideUIPanel(bool condition)
    {
        for(int i = 0; i < UIPanel.Length; i++)
        {
            UIPanel[i].SetActive(condition);
        }
    }

    public void ShowStandPopup()
    {
        standPopupUI.SetActive(true);
        standPopupCondition = true;
        GameManager.instance.popupActive = true;
        GameManager.instance.popupStandActive = true;
        HideShowStandUI(false);
    }

    public void HideStandPopup()
    {
        standPopupUI.SetActive(false);
        standPopupCondition = false;
        GameManager.instance.popupActive = false;
        GameManager.instance.popupStandActive = false;
        HideShowStandUI(true);
    }

    public void ShowResepPopup()
    {
        resepPopupUI.SetActive(true);
        GameManager.instance.popupActive = true;
        GameManager.instance.popupResepActive = true;
        HideShowStandUI(false);
    }

    public void HideResepPopup()
    {
        resepPopupUI.SetActive(false);
        GameManager.instance.popupActive = false;
        GameManager.instance.popupResepActive = false;
        HideShowStandUI(true);
    }

    public void ShowStoragePopup()
    {
        storageUI.SetActive(true);
        GameManager.instance.popupActive = true;
        GameManager.instance.popupstorageActive = true;
        HideShowStandUI(false);
    }

    public void HideStoragePopup()
    {
        storageUI.SetActive(false);
        GameManager.instance.popupActive = false;
        GameManager.instance.popupstorageActive = false;
        HideShowStandUI(true);
    }

    public void ShowMarketPopup()
    {
        marketUI.SetActive(true);
        GameManager.instance.popupActive = true;
        GameManager.instance.popupMarket = true;
        HideShowStandUI(false);
    }

    public void HideMarketPopup()
    {
        marketUI.SetActive(false);
        GameManager.instance.popupActive = false;
        GameManager.instance.popupMarket = false;
        HideShowStandUI(true);
    }

    public void ShowStatisticPopup()
    {
        statistikUI.SetActive(true);
        GameManager.instance.popupActive = true;
        GameManager.instance.popupStatisticActive = true;
        HideShowStandUI(false);
    }

    public void HideStatisticPopup()
    {
        statistikUI.SetActive(false);
        GameManager.instance.popupActive = false;
        GameManager.instance.popupStatisticActive = false;
        HideShowStandUI(true);
    }

    public void ShowPopupCreateMakanan()
    {
        popupCreateMakanan.SetActive(true);
        GameManager.instance.popupActive = true;
        GameManager.instance.popupCreateMakanan = true;
        HideShowStandUI(false);
        popupCreateScript.InitializeUI();
    }

    public void HidePopupCreateMakanan()
    {
        popupCreateMakanan.SetActive(false);
        GameManager.instance.popupActive = false;
        GameManager.instance.popupCreateMakanan = false;
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

    public void ShowStandUpgradePopup(Stand standScript, Sprite standSprite, int level, string namaStand, double harga)
    {
        /*SetTrigger("ShowPopUpStand");*/
        hargaUpgrade = harga;
        standScriptActive = standScript;
        popupStandUpgrade.SetActive(true);
        standImage.sprite = standSprite;
        textNamaStand.text = namaStand;
        textHarga.text = harga.ToString();
        textLevelStand.text = "Level " + level.ToString();

        if(UserStatus.instance.GetCoinValue() >= harga)
        {
            upgradeStandButton.interactable = true;
            imageButton.sprite = buttonSprite[1];
        }
        else
        {
            upgradeStandButton.interactable = false;
            imageButton.sprite = buttonSprite[0];
        }
    }

    public void HideStandUpgradePopup()
    {
        popupStandUpgrade.SetActive(false);
    }

    public void UpgradeStand()
    {
        HideStandUpgradePopup();
        popupStandUpgrade.SetActive(false);
        standScriptActive.UpgradeStandLevel();
        UserStatus.instance.kurangiCoin(hargaUpgrade);

        SpawnPeople.instance.ReinvokeSpawn();
    }

    public void SetTrigger(string triggerName)
    {
        _UIAnimator.SetTrigger(triggerName);
        popupStandUpgrade.SetActive(false);
    }

    public void ShowUnlockedStand(int standNomor)
    {
        if(standNomor == 2)
        {
            standObject[1].SetActive(true);
            mainCamera.MoveCameraToStand(2);
        }else if(standNomor == 3)
        {
            standObject[2].SetActive(true);
            mainCamera.MoveCameraToStand(3);
        }
    }

    public void TopUpCoin()
    {
        UserStatus.instance.CallAddCoin(1000000);
    }

    public void ShowTopNotification(string textNotif)
    {
        if (!isGameplayNotification)
        {
            isGameplayNotification = true;
            GameplayNotification.SetActive(true);
            OnGamePlayNotification.Invoke(textNotif);
        }
    }

    public void ShowBottomNotification(string textNotif)
    {
        if (!isBottomNotification)
        {
            isBottomNotification = true;
            bottomNotification.SetActive(true);
            BottonNotification.Invoke(textNotif);
        }
    }

    
}
