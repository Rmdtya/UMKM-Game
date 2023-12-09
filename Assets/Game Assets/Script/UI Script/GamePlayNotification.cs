using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GamePlayNotification : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public float fadeInTime = 0.2f;
    public float fadeOutTime = 0.3f;
    public float destroyDelay = 1.5f;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Mendengarkan event pembelian barang

    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0;
        UIManager.OnGamePlayNotification.AddListener(ShowPopup);
    }

    private void ShowPopup(string textPopup)
    {
        // Mengatur teks berdasarkan parameter yang dikirim
        popupText.text = textPopup;

        FadeIn();

        Invoke("FadeOutAndDestroy", fadeInTime + destroyDelay);
    }

    private void FadeIn()
    {
        canvasGroup.alpha = 0;
        StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, fadeInTime));
    }

    private void FadeOutAndDestroy()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0, fadeOutTime));
        Invoke("HideObject", fadeOutTime);
    }

    private void HideObject()
    {
        UIManager.instance.isGameplayNotification = false;
        gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float currentTime = 0;

        while (currentTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}