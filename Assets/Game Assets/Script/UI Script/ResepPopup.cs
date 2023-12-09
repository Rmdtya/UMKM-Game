using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResepPopup : MonoBehaviour
{

    public static ResepPopup instance;

    public Transform box;
    public CanvasGroup background;
    public Transform worldCanvas;

    [SerializeField]
    private TextMeshProUGUI textLimitMakanan;
    [SerializeField]
    private TextMeshProUGUI textHargaMakanan;
    [SerializeField]
    private TextMeshProUGUI textJumlahMakanan;

    [Header("Info Object")]
    public GameObject prefabMakanan;
    public GameObject prefabBahan;
    public Transform panelContent;

    private ResepMakanan[] makanan;

    [SerializeField]
    private Stand standScript;

    [SerializeField]
    private StandStatus standStatus;

    // Start is called before the first frame update

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
    }

    private void SetHeadeContent()
    {
        textLimitMakanan.text = standStatus.GetLimitMaksimal().ToString();
        textHargaMakanan.text = makanan[standStatus.selectedManualCreate].hargaMakanan.ToString();

        textJumlahMakanan.text = standStatus.GetTotalJumlahMakanan().ToString();
    }

    public void updateJumlahMakanan()
    {
        textJumlahMakanan.text = standStatus.GetTotalJumlahMakanan().ToString();
    }

    private void InitialObject()
    {
        standScript = GameManager.instance.GetStandScriptActive();

        standStatus = standScript.GetStandStatus();
        int levelStand = standScript.GetLevelStand();

        makanan = standScript.GetResepMakanan();
        int standLevel = standScript.GetLevelStand();

        textHargaMakanan.text = "Rp." + makanan[standStatus.selectedManualCreate].hargaMakanan.ToString();
        SetHeadeContent();

        for (int i = 0; i < makanan.Length; i++)
        {
            
            GameObject instantiatedPrefab = Instantiate(prefabMakanan, panelContent);

            
            int levelMakanan = makanan[i].GetLevelMakanan();
            TextMeshProUGUI[] textComponents = new TextMeshProUGUI[6];

            for (int j = 2; j <= 7; j++)
            {
                textComponents[j - 2] = instantiatedPrefab.transform.GetChild(j).GetComponent<TextMeshProUGUI>();

                if (j == 2)
                {
                    textComponents[j - 2].text = makanan[i].namaMakanan;
                }
                else if (j == 3)
                {
                    UserData userdata = UserStatus.instance.GetUserdata();
                    float speed = makanan[i].speedPembuatan * userdata.multipilerAutoCreateBonus / standStatus.GetBonusAutoCreate();
                    textComponents[j - 2].text = speed.ToString("F1") + "s/porsi";
                }
                else if (j == 4)
                {
                    textComponents[j - 2].text = makanan[i].deskripsiMakanan;
                }
                else if (j == 5)
                {
                    textComponents[j - 2].text = "Level " + levelMakanan.ToString();
                }
                else if (j == 6)
                {
                    textComponents[j - 2].text = makanan[i].hargaUpgrade.ToString("F1");
                }
                else if (j == 7)
                {
                    
                }
            }

            
            RectTransform maxPanel = instantiatedPrefab.transform.GetChild(8).GetComponent<RectTransform>();

            // Simpan nilai i di dalam variabel lokal
            int currentIndex = i;
            Image imageComponent = instantiatedPrefab.transform.GetChild(0).GetComponent<Image>();
            Button buttonComponent = instantiatedPrefab.transform.GetChild(1).GetComponent<Button>();

            if (!makanan[i].GetUnlockStatus())
            {
                maxPanel.gameObject.SetActive(false);
                buttonComponent.interactable = false;
                buttonComponent.gameObject.SetActive(false);

                textComponents[0].text = "? ? ? ? ?";
                textComponents[1].gameObject.SetActive(false);
                textComponents[2].text = makanan[i].deskripsiUnlock;
                textComponents[3].text = "RP. ???";
                textComponents[4].gameObject.SetActive(false);
                textComponents[5].gameObject.SetActive(false);

                if (imageComponent != null)
                {
                    Sprite imageDekorasi = makanan[i].gambarLocked;
                    if (imageDekorasi != null)
                    {
                        imageComponent.sprite = imageDekorasi;
                    }
                }
            }
            else
            {
               
                if (imageComponent != null)
                {
                    Sprite imageDekorasi = makanan[i].GetSpriteMakanan();
                    if (imageDekorasi != null)
                    {
                        imageComponent.sprite = imageDekorasi;
                    }
                }

                if (levelMakanan >= 10 * standStatus.GetLevelStand())
                {
                    buttonComponent.interactable = false;
                    maxPanel.gameObject.SetActive(true);
                }
                else
                {
                    maxPanel.gameObject.SetActive(false);
                    buttonComponent.onClick.AddListener(() => UpgradeLevelMakanan(makanan[currentIndex], textComponents[3], textComponents[4], buttonComponent, makanan[currentIndex].hargaUpgrade, levelStand, maxPanel));
                }
            }

            Transform panelBahan = instantiatedPrefab.transform.GetChild(9).GetComponent<Transform>();

            Sprite[] bahan = makanan[i].GetSpriteBahan();

            for (int k = 0; k < bahan.Length; k++)
            {
                GameObject instantiateBahan = Instantiate(prefabBahan, panelBahan);
                Image spriteBahan = instantiateBahan.GetComponent<Image>();

                spriteBahan.sprite = bahan[k];
            }
        }
    }

    public void UpgradeLevelMakanan(ResepMakanan resepMakanan, TextMeshProUGUI textLevel, TextMeshProUGUI textHarga, Button buttonUpgrade, double harga, int levelStand, RectTransform maxPanel)
    {
        if (UserStatus.instance.kurangiCoin(harga))
        {
            resepMakanan.UpgradeMakanan(levelStand - 1);

            int levelNow = resepMakanan.GetLevelMakanan();

            textLevel.text = "Level " + levelNow.ToString();
            textHarga.text = resepMakanan.hargaUpgrade.ToString("F1");

            if (levelNow >= 10 * levelStand)
            {
                buttonUpgrade.interactable = false;
                maxPanel.gameObject.SetActive(true);
            }
        }
        else
        {
            UIManager.instance.ShowTopNotification("Uang Tidak Mencukupi");
        }
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
