using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupCreateMakanan : MonoBehaviour
{
    public static PopupCreateMakanan instance;

    public GameObject prefabResep;
    public GameObject prefabBahan;
    public Transform panelContent;

    public TextMeshProUGUI[] textJumlahMakanan;
    public Image[] imageMakanan;
    public GameObject[] lockedStatus;

    [SerializeField]
    private ResepMakanan[] resepMakanan;
    [SerializeField]
    private StandStatus standStatus;

    [SerializeField]
    private Sprite activeBackground;
    [SerializeField]
    private Sprite nonAktifBackground;
    [SerializeField]
    private int activeCreate;

    [SerializeField]
    private GameObject[] minusMakanan;

    [SerializeField]
    List<Image> prefabUIMakanan = new List<Image>();

    private void Awake()
    {
        instance = this;
    }

    public void InitializeUI()
    {
        Stand standActive = GameManager.instance.GetStandScriptActive();
        resepMakanan = standActive.GetResepMakanan();
        standStatus = standActive.GetStandStatus();
        int level = standStatus.GetStandLevel();
        activeCreate = standStatus.selectedManualCreate;

        for (int i=0; i<resepMakanan.Length; i++)
        {
            if (resepMakanan[i].GetUnlockStatus())
            {
                lockedStatus[i].SetActive(false);
                textJumlahMakanan[i].text = standStatus.jumlahMakanan[i].ToString();
                imageMakanan[i].sprite = resepMakanan[i].gambarMakanan;
                textJumlahMakanan[i].gameObject.SetActive(true);
                imageMakanan[i].gameObject.SetActive(true);
            }
            else
            {
                textJumlahMakanan[i].text = "Locked";
                imageMakanan[i].gameObject.SetActive(false);
                lockedStatus[i].SetActive(true);

            }
        }

        ShowKonten();
    }


    private void ShowKonten()
    {
        for (int i = 0; i < resepMakanan.Length; i++)
        {
            if (!resepMakanan[i].GetUnlockStatus())
            {
                minusMakanan[i].SetActive(false);
            }
            else
            {
                minusMakanan[i].SetActive(true);
                GameObject instantiatedPrefab = Instantiate(prefabResep, panelContent);
                
                Image imageComponent = instantiatedPrefab.transform.GetChild(0).GetComponent<Image>();

                TextMeshProUGUI[] textComponents = new TextMeshProUGUI[4];

                for (int j = 1; j <= textComponents.Length; j++)
                {
                    textComponents[j - 1] = instantiatedPrefab.transform.GetChild(j).GetComponent<TextMeshProUGUI>();

                    if (j == 1)
                    {
                        textComponents[j - 1].text = resepMakanan[i].namaMakanan;
                    }
                    else if (j == 2)
                    {
                        textComponents[j - 1].text = resepMakanan[i].hargaMakanan.ToString("F0");
                    }
                    else if (j == 4)
                    {
                        textComponents[j - 1].text = resepMakanan[i].speedPembuatan.ToString("F1") + " / Porsi";
                    }
                }

                Button buttonComponent = instantiatedPrefab.GetComponent<Button>();
                Image prefabBackground = instantiatedPrefab.GetComponent<Image>();
                prefabUIMakanan.Add(prefabBackground);

                int currentIndex = i;
                buttonComponent.onClick.AddListener(() => UpdateActiveMakanan(currentIndex));

                if (i == activeCreate)
                {
                    prefabBackground.sprite = activeBackground;
                }
                else
                {
                    prefabBackground.sprite = nonAktifBackground;
                }

                Transform panelBahan = instantiatedPrefab.transform.GetChild(5).GetComponent<Transform>();

                Sprite[] bahan = resepMakanan[i].GetSpriteBahan();

                for (int k = 0; k < bahan.Length; k++)
                {
                    GameObject instantiateBahan = Instantiate(prefabBahan, panelBahan);
                    Image spriteBahan = instantiateBahan.GetComponent<Image>();

                    spriteBahan.sprite = bahan[k];
                }

                if (imageComponent != null)
                {
                    Sprite imageDekorasi = resepMakanan[i].gambarMakanan;
                    if (imageDekorasi != null)
                    {
                        imageComponent.sprite = imageDekorasi;
                    }
                }
            }
        }
    }

    private void UpdateActiveMakanan(int makananActive)
    {
        if (makananActive == standStatus.selectedManualCreate)
            return;

        for(int i = 0; i < prefabUIMakanan.Count; i++)
        {
            prefabUIMakanan[i].sprite = nonAktifBackground;

            if(i == makananActive)
            {
                prefabUIMakanan[i].sprite = activeBackground;
                standStatus.selectedManualCreate = i;
            }
        }
    }

    public void updateJumlahMakanan()
    {
        for (int i = 0; i < resepMakanan.Length; i++)
        {
            if (resepMakanan[i].GetUnlockStatus())
            {
                lockedStatus[i].SetActive(false);
                textJumlahMakanan[i].text = standStatus.jumlahMakanan[i].ToString();
                imageMakanan[i].sprite = resepMakanan[i].gambarMakanan;
                textJumlahMakanan[i].gameObject.SetActive(true);
                imageMakanan[i].gameObject.SetActive(true);
            }
            else
            {
                textJumlahMakanan[i].gameObject.SetActive(false);
                imageMakanan[i].gameObject.SetActive(false);
                lockedStatus[i].SetActive(true);
            }
        }
    }

    public void BuangMakanan(int jenisMakanan)
    {
        if (standStatus.jumlahMakanan[jenisMakanan] <= 0)
        {

        }
        else
        {
            standStatus.jumlahMakanan[jenisMakanan] -= 1;
            textJumlahMakanan[jenisMakanan].text = standStatus.jumlahMakanan[jenisMakanan].ToString();
        }
    }

    private void OnDisable()
    {
        int childCount = panelContent.childCount;
        prefabUIMakanan.Clear();

        // Iterasi melalui child objek, mulai dari indeks 1 (objek kedua)
        for (int i = 0; i < childCount; i++)
        {
            // Dapatkan child objek pada indeks i
            Transform child = panelContent.GetChild(i);

            // Hancurkan objek
            Destroy(child.gameObject);
        }
    }
}
