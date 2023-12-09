using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PopupInAppPurchase : MonoBehaviour
{

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

    public void SetUI()
    {

    }

    private void OnDisable()
    {
        GameManager.instance.PopupActive(false);
    }

}
