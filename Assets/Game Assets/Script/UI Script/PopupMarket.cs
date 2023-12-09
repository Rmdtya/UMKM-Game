using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PopupMarket : MonoBehaviour
{
    public static PopupMarket instance;
    public Transform box;
    public CanvasGroup background;
    public Transform worldCanvas;

    [SerializeField]
    private TextMeshProUGUI textJumlahBahan;
    [SerializeField]
    private TextMeshProUGUI textLimitBahan;

    [Header("Info Object")]
    [SerializeField]
    private GameObject prefabBahan;
    [SerializeField]
    private Transform panelContent;

    [SerializeField]
    Storage storage;

    public GameObject notification;
    public static UnityEvent<string> OnPurchaseComplete = new UnityEvent<string>();

    public bool notificationMarket;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        notificationMarket = false;
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
        
    }

    private void OnDisable()
    {
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseOutExpo().delay = 0.1f;

        worldCanvas.LeanMoveLocalY(1095.285f, 0.5f).setEaseOutExpo();
    }

   

    public void ShowNotification(string text)
    {
        notification.SetActive(true);
        OnPurchaseComplete.Invoke(text);
    }



}
