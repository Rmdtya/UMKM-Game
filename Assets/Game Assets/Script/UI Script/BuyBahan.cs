using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyBahan : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    public TextMeshProUGUI textNama;
    public TextMeshProUGUI textHarga;
    public TextMeshProUGUI textMaksimal;

    public string namaBahan;
    public string textFailure;

    [SerializeField]
    private double hargaBahan;
    [SerializeField]
    private Storage storage;

    

    private void Start()
    {
        slider.value = 0f;
        double total = hargaBahan * slider.value;
        textHarga.text = "Rp." + total.ToString("F0");
        textNama.text = namaBahan;
    }

    public void OnSliderValueChanged()
    {
        double total = hargaBahan * slider.value;
        textHarga.text = "Rp." + total.ToString("F0");
        textMaksimal.text = "x" + slider.value.ToString();
    }

    public void BeliBahan(int index)
    {
        double jumlahBeli = slider.value;
        double total = jumlahBeli * hargaBahan;

        if(slider.value <= 0f)
        {
            PopupMarket.instance.ShowNotification("Masukan Jumlah Yang Ingin Dibeli");
        }
        else
        {
            if (storage.GetTotalBahan() < storage.GetLimitMaksimal() + jumlahBeli)
            {
                if (UserStatus.instance.kurangiCoin(total))
                {
                    storage.penyimpananBahan[index].jumlah += slider.value;
                    PopupMarket.instance.ShowNotification("Kamu Berhasil Membeli " + namaBahan + " x" + slider.value.ToString("F0"));

                    slider.value = 0f;
                }
                else
                {
                    slider.value = 0f;
                    PopupMarket.instance.ShowNotification("Uang Tidak Cukup");
                }
            }
            else
            {
                PopupMarket.instance.ShowNotification("Gudang Penuh, Keluarkan Beberapa Bahan");
            }
        }

        
        
    }
}
