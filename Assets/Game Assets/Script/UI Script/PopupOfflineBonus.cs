using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupOfflineBonus : MonoBehaviour
{
    [Header("Info Object")]
    public GameObject prefabPanel;
    public Transform panelContent;
    public GameObject UIPopup;

    [SerializeField]
    private TextMeshProUGUI textTotalBonus;

    public void SetUI()
    {
        UIPopup.SetActive(true);
        GameManager.instance.popupActive = true;
    }

    public void AddPanel(Sprite gambarMakanan, string namaMakanan, int makananTerjual, double pendapatan, double popularity)
    {
        GameObject instantiatedPrefab = Instantiate(prefabPanel, panelContent);

        Image imageComponent = instantiatedPrefab.transform.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI textNamaMakanan = instantiatedPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI textJumlah = instantiatedPrefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI textPopularity = instantiatedPrefab.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI textTotal = instantiatedPrefab.transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        string formatRupiah = string.Format("{0:N0}", pendapatan);

        imageComponent.sprite = gambarMakanan;
        textNamaMakanan.text = namaMakanan;
        textJumlah.text = makananTerjual.ToString() + " Porsi";
        textPopularity.text = "+ " + popularity.ToString("F0");
        textTotal.text = formatRupiah;
    }

    public void SetTotalBonus(double total)
    {
        string formatRupiah = string.Format("{0:N0}", total);
        textTotalBonus.text = formatRupiah;
    }

    private void OnDisable()
    {
        GameManager.instance.popupActive = false;
    }
}
