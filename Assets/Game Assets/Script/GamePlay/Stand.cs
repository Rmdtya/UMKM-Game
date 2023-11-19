using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Stand : MonoBehaviour
{
    
    // Start is called before the first frame update

    [SerializeField]
    private StandStatus standStatus;
    [SerializeField]
    private int jenisStand;

    [SerializeField]
    private ResepMakanan[] makanan;

    [SerializeField]
    private Storage storage;

    [SerializeField]
    private Image imageStand;

    [SerializeField]
    private Image imageKoki;

    [Header("Stand UI")]
    public TextMeshProUGUI textJumlahMakanan;
    public TextMeshProUGUI textNamaToko;


    void Start()
    {
        SetAutomaticInvoke();

        InvokeRepeating("PlayAudioSales", 0f, 5f);
        SetImageStand();
        
        SetTextStand();
        SetTextTotalMakanan();

        jenisStand = standStatus.jenisStand;

        standStatus.selectedManualCreate = 0;
    }

   

    private void SetImageStand()
    {
        imageStand.sprite = standStatus.spriteStand[standStatus.GetLevelStand() - 1];
    }

    public void SetTextStand()
    {
        textNamaToko.text = standStatus.namaStand;
    }

    private void SetTextTotalMakanan()
    {
        int total = GetTotalMakanan();
        textJumlahMakanan.text = total.ToString();
    }

    private int GetTotalMakanan()
    {
        int total = 0;
        for (int i = 0; i < standStatus.jumlahMakanan.Length; i++)
        {
            total += standStatus.jumlahMakanan[i];
        }

        return total;
    }

    private void SetAutomaticInvoke()
    {
        StopAllCoroutines();
        UserData userdata = UserStatus.instance.GetUserdata();

        for (int i = 0; i < makanan.Length; i++)
        {
            float speedCreate = makanan[i].speedPembuatan * userdata.multipilerAutoCreateBonus * standStatus.GetBonusAutoCreate();
            
            if(i == 0)
            {
                StartCoroutine(CreateFoodParameter(speedCreate, 0));
            }
            else if (i == 1 && standStatus.GetLevelStand() >= 3)
            {
                StartCoroutine(CreateFoodParameter(speedCreate, 1));
            }
            else if (i == 2 && standStatus.GetLevelStand() >= 5)
            {
                StartCoroutine(CreateFoodParameter(speedCreate, 2));
            }
        }
    }

    // Update is called once per frame

    System.Collections.IEnumerator CreateFoodParameter(float speedCreate, int jenisMakanan)
    {
        yield return new WaitForSeconds(speedCreate);

        while (true)
        {
            TambahMakananotomatis(jenisMakanan);

            // Tunggu 2 detik sebelum memanggil lagi
            yield return new WaitForSeconds(speedCreate);
        }
    }


    void TambahMakananotomatis(int jenisMakanan)
    {
        if (GetTotalMakanan() >= standStatus.GetLimitMaksimal())
            return;

        int start = GetStarterIndexByJenisStand();

        bool statusKetersediaan = true;
            for(int i = 0 + start; i < makanan[jenisMakanan].requiriment.Length + start; i++)
            {
                if (storage.penyimpananBahan[i].jumlah <= makanan[jenisMakanan].requiriment[i])
                {
                    statusKetersediaan = false;
                    break; // Keluar dari loop jika persyaratan tidak terpenuhi
                }
            }

            if(statusKetersediaan)
            {
                 for (int i = 0 + start; i < makanan[jenisMakanan].requiriment.Length + start; i++)
                 {
                     storage.penyimpananBahan[i].jumlah -= makanan[jenisMakanan].requiriment[i];
                 }

                 SuccesToAddMakanan(jenisMakanan, 1);
        }       
    }

    public int GetStarterIndexByJenisStand()
    {
        int start = 0;
        if (jenisStand == 1)
        {
            start = 0;
        }else if(jenisStand == 2)
        {
            start = 11;
        }else if(jenisStand == 3)
        {

        }

        return start;
    }

    public void TambahMakananPerClick()
    {
        if (GetTotalMakanan() >= standStatus.GetLimitMaksimal())
            return;

        int start = GetStarterIndexByJenisStand();
        int jenisMakanan = standStatus.selectedManualCreate;

        bool statusKetersediaan = true;
        for (int i = 0 + start; i < makanan[jenisMakanan].requiriment.Length + start; i++)
        {
            if (storage.penyimpananBahan[i].jumlah <= makanan[jenisMakanan].requiriment[i])
            {
                statusKetersediaan = false;
                break; // Keluar dari loop jika persyaratan tidak terpenuhi
            }
        }

        if (statusKetersediaan)
        {
            for (int i = 0 + start; i < makanan[jenisMakanan].requiriment.Length + start; i++)
            {
                storage.penyimpananBahan[i].jumlah -= makanan[jenisMakanan].requiriment[i];
            }

             SuccesToAddMakanan(jenisMakanan, 1);
        }
    }

    private void SuccesToAddMakanan(int jenisMakanan, int jumlah)
    {
        standStatus.jumlahMakanan[jenisMakanan] += jumlah;
        SetTextTotalMakanan();
    }

    public bool Transaction(int jumlahBeli, int jenisMakanan, CustomerSpecialSkill customerSkill)
    {
            if (standStatus.jumlahMakanan[jenisMakanan] >= jumlahBeli)
            {
                int totalJumlah = 0;
                standStatus.jumlahMakanan[jenisMakanan] -= jumlahBeli;

                SetTextTotalMakanan();

                UserData userData = UserStatus.instance.GetUserdata();

                UserStatus.instance.CallAddCoin(makanan[jenisMakanan].hargaMakanan * jumlahBeli * userData.multipilerCoinBonus + (userData.plusCoinBonus * jumlahBeli) + (standStatus.GetBonusCoin() * jumlahBeli));
                UserStatus.instance.SetPopularityPoint(jumlahBeli * userData.multipilerPopularityBonus * standStatus.GetBonusPopularity());

                return true;
            }
            else
            {
                return false;
            }
    }

    public int GetJenisStand()
    {
        return standStatus.jenisStand;
    }

    public void PlayAudioSales()
    {
        SoundManager.instance.PlayAudioSales();
    }

    public float GetBonusSpeedSpawn()
    {
        return standStatus.GetBonusSpeedSpawn();
    }

    public int GetLevelStand()
    {
        return standStatus.GetLevelStand();
    }

}
