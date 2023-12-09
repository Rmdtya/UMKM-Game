using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StandPopup : MonoBehaviour
{
    // Start is called before the first frame update

    public static StandPopup instance;
    public Transform box;
    public CanvasGroup background;
    public Transform worldCanvas;

    [SerializeField]
    private TextMeshProUGUI textJudulStand;
    [SerializeField]
    private TextMeshProUGUI textLevelStand;
    [SerializeField]
    private TextMeshProUGUI textHargaMakanan;
    [SerializeField]
    private TextMeshProUGUI textJumlahMakanan;
    [SerializeField]
    private TextMeshProUGUI textNumberLevel;

    public GameObject prefabDekorasi;
    public Transform panelContent;

    [Header("Button Upgrade Stand")]
    public Image buttonUpgrade;
    public Button upgradeButton;
    public Sprite[] spriteButton;


    [SerializeField]
    private Stand standScript;

    [SerializeField]
    private StandStatus standStatus;

    [SerializeField]
    private Dekorasi[] dekorasi;


    private bool upgradedStatus = true;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(-1459.262f, 0.5f).setEaseOutExpo().delay = 0.1f;

        worldCanvas.LeanMoveLocalY(1795f, 0.5f).setEaseOutExpo().delay = 0.1f;

        InitialObject();
        SetHeadeContent();
    }

    private void SetHeadeContent()
    {
        textJudulStand.text = standStatus.namaStand;
        textLevelStand.text = "Level " + standStatus.GetStandLevel().ToString();
        textNumberLevel.text = standStatus.GetStandLevel().ToString();

        textJumlahMakanan.text = standStatus.GetTotalJumlahMakanan().ToString();
        textHargaMakanan.text = "Rp." + standScript.TotalHargaPorsi().ToString("F0");

    }

    public void updateJumlahMakanan()
    {
        textJumlahMakanan.text = standStatus.GetTotalJumlahMakanan().ToString();
    }

    private void CheckUpgradeStand(Dekorasi[] standDekorasi, int standLevel)
    {
        
        for (int i = 0; i < standDekorasi.Length; i++)
        {
            if (standDekorasi[i].GetAccesorisLevel() + 1 <= 10 * standLevel)
            {
                upgradedStatus = false;
                upgradeButton.interactable = false;
                break;
            }
            else
            {
                upgradedStatus = true;
            }
        }

        if (upgradedStatus)
        {
            buttonUpgrade.sprite = spriteButton[1];
            upgradeButton.interactable = true;

            upgradeButton.onClick.AddListener(() => UpgradeStandLevel());
        }
    }

    private void InitialObject()
    {
        buttonUpgrade.sprite = spriteButton[0];
        standScript = GameManager.instance.GetStandScriptActive();
        standStatus = standScript.GetStandStatus();
        dekorasi = standStatus.GetStandDekorasi();

        int standLevel = standScript.GetLevelStand();

        CheckUpgradeStand(dekorasi, standLevel);

        for (int i = 0; i < standStatus.GetStandDekorasi().Length; i++)
        {
            GameObject instantiatedPrefab = Instantiate(prefabDekorasi, panelContent);

            Image imageComponent = instantiatedPrefab.transform.GetChild(0).GetComponent<Image>();

            int levelDekorasi = dekorasi[i].GetAccesorisLevel();
            TextMeshProUGUI[] textComponents = new TextMeshProUGUI[6];

            for (int j = 1; j <= standStatus.GetStandDekorasi().Length; j++)
            {
                textComponents[j - 1] = instantiatedPrefab.transform.GetChild(j).GetComponent<TextMeshProUGUI>();

                if (j == 1)
                {
                    textComponents[j - 1].text = dekorasi[i].namaDekorasi;
                }else if(j == 2)
                {
                    textComponents[j - 1].text = dekorasi[i].bonusEfek;
                }else if(j == 3)
                {
                    textComponents[j - 1].text = dekorasi[i].deskripsiDekorasi;
                }
                else if (j == 4)
                {
                    textComponents[j - 1].text = "Level " + levelDekorasi.ToString();
                }
                else if (j == 5)
                {
                    textComponents[j - 1].text = dekorasi[i].hargaUpgrade.ToString("F1");
                }
                else if (j == 6)
                {
                    //max button
                }
            }

            Button buttonComponent = instantiatedPrefab.transform.GetChild(7).GetComponent<Button>();
            RectTransform maxPanel = instantiatedPrefab.transform.GetChild(8).GetComponent<RectTransform>();

            // Simpan nilai i di dalam variabel lokal
            int currentIndex = i;

            if(levelDekorasi >= 10 * standStatus.GetLevelStand())
            {
                buttonComponent.interactable = false;
                maxPanel.gameObject.SetActive(true);
            }
            else
            {
                maxPanel.gameObject.SetActive(false);
                buttonComponent.onClick.AddListener(() => UpgradeKomponenDekorasi(dekorasi[currentIndex].hargaUpgrade, currentIndex, textComponents, standLevel - 1, buttonComponent, maxPanel));
            }

            // Menambahkan listener onClick dengan ekspresi lambda


            if (imageComponent != null)
            {
                Sprite imageDekorasi = dekorasi[i].spriteDekorasi[standLevel - 1];
                if (imageDekorasi != null)
                {
                    imageComponent.sprite = imageDekorasi;
                }
            }

        }
    }



    

    private void UpgradeKomponenDekorasi(double harga, int dekorasiNumber, TextMeshProUGUI[] textComponents, int levelStand, Button buttonUpgrade, RectTransform maxPanel)
    {
        if (UserStatus.instance.kurangiCoin(harga))
        {
            int levelNow = standScript.UpgradeDekorasiStand(dekorasiNumber, levelStand);

            Dekorasi selectedDekorasi = standStatus.GetSpesifikStandDekorasi(dekorasiNumber);

            for (int i = 0; i <= 5; i++)
            {
                if (i == 0)
                {
                    textComponents[i].text = selectedDekorasi.namaDekorasi;
                }
                else if (i == 1)
                {
                    textComponents[i].text = selectedDekorasi.bonusEfek;
                }
                else if (i == 2)
                {
                    textComponents[i].text = selectedDekorasi.deskripsiDekorasi;
                }
                else if (i == 3)
                {
                    textComponents[i].text = "Level " + selectedDekorasi.GetAccesorisLevel().ToString();
                }
                else if (i == 4)
                {
                    textComponents[i].text = selectedDekorasi.hargaUpgrade.ToString("F1");
                }
                else if (i == 5)
                {
                    //max button
                }
            }

            Debug.Log("Level Stand : " + levelStand);
            Debug.Log("Level Now : " + levelNow);

            if (levelNow >= 10 * (levelStand + 1))
            {
                buttonUpgrade.interactable = false;
                maxPanel.gameObject.SetActive(true);
            }

            CheckUpgradeStand(dekorasi, levelStand + 1);
        }
        else
        {
            UIManager.instance.ShowTopNotification("Uang Tidak Mencukupi");
        }
    }

    private void UpgradeStandLevel()
    {
        upgradeButton.interactable = false;
        standScript.ShowUpgradeStandLevel();

        UIManager.instance.HideStandPopup();
    }

    private void OnDisable()
    {
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseOutExpo().delay = 0.1f;

        worldCanvas.LeanMoveLocalY(1095.285f, 0.5f).setEaseOutExpo();
        DestroyObjectAfterClose();
    }

    private void DestroyObjectAfterClose()
    {
        int childCount = panelContent.childCount;

        // Iterasi melalui child objek, mulai dari indeks 1 (objek kedua)
        for (int i = 1; i < childCount; i++)
        {
            // Dapatkan child objek pada indeks i
            Transform child = panelContent.GetChild(i);

            // Hancurkan objek
            Destroy(child.gameObject);
        }
    }

}
