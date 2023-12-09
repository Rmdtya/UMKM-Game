using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PopupBank : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button[] panelButton;
    [SerializeField] int[] hargaDiamond;
    [SerializeField] double[] coinCollect;
    [SerializeField] TextMeshProUGUI[] textComponent;

    [SerializeField] TextMeshProUGUI textcoinWatch;
    public double coinWatch = 500000;

    [SerializeField] AdsInitializer adsInitializer;

    private void OnEnable()
    {
        GameManager.instance.PopupActive(true);
        SetUI();

        try
        {
            adsInitializer.InitializeAds();
        }
        catch (Exception e)
        {
            Debug.Log("Error Reload Adds " + e.Message);
        }
    }

    private void SetUI()
    {
        textcoinWatch.text = FormatRupiah(coinWatch);
        int jumlahDiamond = UserStatus.instance.GetDiamondValue();
        for (int i = 0; i < panelButton.Length; i++)
        {
            if(jumlahDiamond >= hargaDiamond[i])
            {
                panelButton[i].interactable = true;
            }
            else
            {
                panelButton[i].interactable = false;
            }

            textComponent[i].text = FormatRupiah(coinCollect[i]);
        }
    }

    public void ChangeCoin(int index)
    {
        UserStatus.instance.CallAddCoin(coinCollect[index]);
        UserStatus.instance.CallAddDiamond(-1 * hargaDiamond[index]);

        UIManager.instance.ShowTopNotification("Berhasil Menukar " + hargaDiamond[index] + " Diamond");

        SetUI();
    }

    private void OnDisable()
    {
        GameManager.instance.PopupActive(false);
    }

    public string FormatRupiah(double total)
    {
        string formatRupiah = string.Format("{0:N0}", total);
        return formatRupiah;
    }
}
