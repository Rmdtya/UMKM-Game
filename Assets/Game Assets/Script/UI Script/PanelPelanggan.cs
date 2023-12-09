using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelPelanggan : MonoBehaviour
{
    public Sprite[] buttonBG;
    public Image imagePelanggan;
    public Image backgroundButton;
    public Button buttonUnlock;
    public TextMeshProUGUI textUnlock;

    [SerializeField]
    private CustomerStatus customerStatus;

    int activeStand;
    bool unlockAble;

    public void SetPanel(int standActive, int levelStand, double popularityPoint)
    {
        activeStand = standActive;
        if (customerStatus.GetUnlockStatus(standActive))
        {
            imagePelanggan.color = Color.white;
            buttonUnlock.interactable = false;
            textUnlock.text = "Terbuka";
            backgroundButton.sprite = buttonBG[0];
        }
        else
        {
            if(levelStand >= customerStatus.syaratLevel && popularityPoint >= customerStatus.syaratPopularity)
            {
                unlockAble = true;
                SetButtonAs();
            }
            else
            {
                unlockAble = false;
                SetButtonAs();
            }

        }
    }

    private void SetButtonAs()
    {
        if(unlockAble)
        {
            imagePelanggan.color = Color.black;
            buttonUnlock.interactable = true;
            textUnlock.text = "Unlock Now";
            backgroundButton.sprite = buttonBG[0];
        }
        else
        {
            imagePelanggan.color = Color.black;
            buttonUnlock.interactable = true;
            textUnlock.text = "Lock";
            backgroundButton.sprite = buttonBG[1];
        }
    }

    public void UnlockCustomer()
    {
        if (unlockAble)
        {
            customerStatus.UnlockStatus(activeStand);
            UIManager.instance.HideStatisticPopup();
            SpawnPeople.instance.ReinvokeSpawn();
        }
        else
        {
            Debug.Log("Mencoba Notifikasi");
            UIManager.instance.ShowBottomNotification("Capai Stand Level " + customerStatus.syaratLevel.ToString() + " dan " + customerStatus.syaratPopularity.ToString() + " Popularity");
        }
    }
}
