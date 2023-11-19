using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSlider : MonoBehaviour
{
    public Slider slider;
    public float loadingTime = 10f; // Waktu yang dibutuhkan untuk mengisi slider (dalam detik)
    private float timer = 0f;

    private void Update()
    {
        if (timer < loadingTime)
        {
            timer += Time.deltaTime;
            float fillAmount = timer / loadingTime;
            slider.value = fillAmount;
        }
        else
        {
            // Proses telah selesai, Anda dapat menambahkan tindakan selanjutnya di sini.
        }
    }
}
