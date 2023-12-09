using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupStorage : MonoBehaviour
{
    public Transform box;
    public CanvasGroup background;
    public Transform worldCanvas;

    [SerializeField]
    private TextMeshProUGUI textJumlahBahan;
    [SerializeField]
    private TextMeshProUGUI textLimitBahan;

    [Header("Info Object")]
    public RectTransform canvas;
    [SerializeField]
    private GameObject prefabBahan;
    [SerializeField]
    private Transform panelContent;

    [SerializeField]
    Storage storage;

    public int jumlahBahan;

    private void OnEnable()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(-1459.262f, 0.5f).setEaseOutExpo().delay = 0.1f;

        worldCanvas.LeanMoveLocalY(1795f, 0.5f).setEaseOutExpo().delay = 0.1f;

        SetHeadeContent();
        InitialObject();

    }

    private void SetHeadeContent()
    {
        textLimitBahan.text = "/" + storage.GetLimitMaksimal().ToString();
        textJumlahBahan.text = storage.GetTotalBahan().ToString("F1");
    }

    private void InitialObject()
    {
        KebutuhanBahan[] bahan = storage.penyimpananBahan;
        jumlahBahan = 0;

        for (int i = 0; i < storage.penyimpananBahan.Length; i++)
        {
            if (bahan[i].jumlah >= 0f)
            {
                GameObject instantiatedPrefab = Instantiate(prefabBahan, panelContent);

                Image imageComponent = instantiatedPrefab.transform.GetChild(1).GetComponent<Image>();
                imageComponent.sprite = bahan[i].spriteBahan;

                TextMeshProUGUI[] textComponents = new TextMeshProUGUI[2];

                for (int j = 2; j <= 3; j++)
                {
                    textComponents[j - 2] = instantiatedPrefab.transform.GetChild(j).GetComponent<TextMeshProUGUI>();

                    if (j == 2)
                    {
                        textComponents[j - 2].text = bahan[i].namaBahan;
                    }
                    else if (j == 3)
                    {
                        textComponents[j - 2].text = bahan[i].jumlah.ToString("F1") + "x";
                    }
                }

                int currentIndex = i;

                Button buttonComponent = instantiatedPrefab.transform.GetChild(4).GetComponent<Button>();

                buttonComponent.onClick.AddListener(() => KurangiMakanan(currentIndex, textComponents[1], instantiatedPrefab));

                jumlahBahan++;
            }

        }

        SetHeightCanvas(jumlahBahan);
    }

    private void KurangiMakanan(int index, TextMeshProUGUI text, GameObject obj)
    {
        if(storage.penyimpananBahan[index].jumlah >= 1)
        {
            storage.penyimpananBahan[index].jumlah--;
            text.text = storage.penyimpananBahan[index].jumlah.ToString("F1");
        }
        else if(storage.penyimpananBahan[index].jumlah < 1)
        {
            storage.penyimpananBahan[index].jumlah = 0;
            Destroy(obj);
        }
    }

    private void SetHeightCanvas(int jumlah)
    {

        if (jumlah <= 5)
        {
            canvas.sizeDelta = new Vector2(1895.737f, 550);
        }
        else if(jumlah >= 6 && jumlah <= 10)
        {
            canvas.sizeDelta = new Vector2(1895.737f, 985);
        }else if(jumlah >= 11 && jumlah <= 15)
        {
            canvas.sizeDelta = new Vector2(1895.737f, 1500);
        }else if(jumlah >= 16 && jumlah <= 20)
        {
            canvas.sizeDelta = new Vector2(1895.737f, 1950);
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
        for (int i = 0; i < childCount; i++)
        {
            // Dapatkan child objek pada indeks i
            Transform child = panelContent.GetChild(i);

            // Hancurkan objek
            Destroy(child.gameObject);
        }
    }
}
