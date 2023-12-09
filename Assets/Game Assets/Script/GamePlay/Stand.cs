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
    public TextMeshProUGUI textLevelStand;


    [Header("Pedagang")]
    public Image imagePedagang;
    public Sprite[] spritePedagang;
    private int currentSpriteIndex = 0;
    public float changeInterval = 2f; // Interval in seconds



    void Start()
    {
        SetAutomaticInvoke();

        InvokeRepeating("PlayAudioSales", 0f, 5f);
        SetImageStand();
        
        SetTextStand();
        SetTextTotalMakanan();
        SetLevelStand();

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

    public void SetLevelStand()
    {
        textLevelStand.text = standStatus.GetLevelStand().ToString();
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

    public void ReinvokeFoodCreate()
    {
        SetAutomaticInvoke();
    }

    private void SetAutomaticInvoke()
    {
        StopAllCoroutines();
        UserData userdata = UserStatus.instance.GetUserdata();

        for (int i = 0; i < makanan.Length; i++)
        {
            float speedCreate = makanan[i].speedPembuatan * userdata.multipilerAutoCreateBonus / standStatus.GetBonusAutoCreate();
            
            if(i == 0)
            {
                StartCoroutine(CreateFoodParameter(speedCreate, 0));
            }
            else if (i == 1 && standStatus.GetLevelStand() >= 2)
            {
                StartCoroutine(CreateFoodParameter(speedCreate, 1));
            }
            else if (i == 2 && standStatus.GetLevelStand() >= 4)
            {
                StartCoroutine(CreateFoodParameter(speedCreate, 2));
            }
        }

        StartCoroutine(ChangeSpritePedagang());
    }

    IEnumerator ChangeSpritePedagang()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeInterval);
            ChangePose();
        }
    }

    void ChangePose()
    {
        // Change the sprite to the next one in the array
        currentSpriteIndex = (currentSpriteIndex + 1) % spritePedagang.Length;
        imagePedagang.sprite = spritePedagang[currentSpriteIndex];
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
                if (storage.penyimpananBahan[i].jumlah <= makanan[jenisMakanan].requiriment[i - start])
                {
                    statusKetersediaan = false;
                    break; // Keluar dari loop jika persyaratan tidak terpenuhi
                }
            }

            if(statusKetersediaan)
            {
                 for (int i = 0 + start; i < makanan[jenisMakanan].requiriment.Length + start; i++)
                 {
                    storage.penyimpananBahan[i].jumlah -= makanan[jenisMakanan].requiriment[i - start];
                 }

                 SuccesToAddMakanan(jenisMakanan, 1);
            }
            else
            {
                UIManager.instance.ShowTopNotification("Stand " + standStatus.namaStand + " - Bahan Tidak Cukup");
            }       
    }

    public int GetStarterIndexByJenisStand()
    {
        int start = 0;
        if (jenisStand == 0)
        {
            start = 0;
        }else if(jenisStand == 1)
        {
            start = 11;
        }else if(jenisStand == 2)
        {
            start = 0;
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
            if (storage.penyimpananBahan[i].jumlah <= makanan[jenisMakanan].requiriment[i - start])
            {
                statusKetersediaan = false;
                break; // Keluar dari loop jika persyaratan tidak terpenuhi
            }
        }

        if (statusKetersediaan)
        {
            for (int i = 0 + start; i < makanan[jenisMakanan].requiriment.Length + start; i++)
            {
                storage.penyimpananBahan[i].jumlah -= makanan[jenisMakanan].requiriment[i - start];
            }

             SuccesToAddMakanan(jenisMakanan, 1);
        }
        else
        {
            UIManager.instance.ShowTopNotification("Stand " + standStatus.namaStand + " - Bahan Tidak Cukup");
        }
    }

    private void SuccesToAddMakanan(int jenisMakanan, int jumlah)
    {
        standStatus.jumlahMakanan[jenisMakanan] += jumlah;
        SetTextTotalMakanan();

        UpdateJumlahMakanan();
    }

    private void UpdateJumlahMakanan()
    {
        if (GameManager.instance.popupStandActive)
        {
            StandPopup.instance.updateJumlahMakanan();
        }

        if (GameManager.instance.popupCreateMakanan)
        {
            PopupCreateMakanan.instance.updateJumlahMakanan();
        }
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

                UpdateJumlahMakanan();

            return true;
            }
            else
            {
                UIManager.instance.ShowTopNotification("Stand " + standStatus.namaStand + " - Makanan Tidak Tersedia");
                return false;
            }
    }

    public void ShowUpgradeStandLevel()
    {
        UIManager.instance.ShowStandUpgradePopup(this, standStatus.spriteStand[standStatus.GetLevelStand()], standStatus.GetLevelStand(), standStatus.namaStand, standStatus.hargaUpgradeStand[standStatus.GetLevelStand() - 1]);
    }

    public void UpgradeStandLevel()
    {
        standStatus.UpgradeLevelStand();
        SetImageStand();
        SetLevelStand();

        LoadSaveData.instance.SaveData();

        int levelNow = standStatus.GetLevelStand();
        if(levelNow == 2)
        {
            if (!makanan[1].GetUnlockStatus())
            {
                makanan[1].UnlockMakanan();
                ReinvokeFoodCreate();
            }
        }else if(levelNow == 4)
        {
            if (!makanan[2].GetUnlockStatus())
            {
                makanan[2].UnlockMakanan();
                ReinvokeFoodCreate();
            }
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

    public StandStatus GetStandStatus()
    {
        return standStatus;
    }


    public int UpgradeDekorasiStand(int index, int standLevel)
    {
        standStatus.UpgradeDekorasi(index, standLevel);

        if (index == 2)
        {
            SpawnPeople.instance.ReinvokeSpawn();
            Debug.Log("Reinvoke Spawn");
        }else if(index == 4)
        {
            ReinvokeFoodCreate();
        }

        return standStatus.GetSpesifikasiLevelDekorasi(index);
    }

    public ResepMakanan[] GetResepMakanan()
    {
        return makanan;
    }

    public double TotalHargaPorsi()
    {
        double harga = 0;
        for(int i = 0; i < makanan.Length; i++)
        {
            if (makanan[i].GetUnlockStatus())
            {
                harga += makanan[i].hargaMakanan;
            }
        }

        return harga;
    }

    public void UnlockFirstFood()
    {
        makanan[0].UnlockMakanan();
    }

    public bool OfflineCreateMakanan(int jenisMakanan, int standNomor)
    {
        if(standNomor == 0)
        {
            jenisMakanan -= 0;
        }else if(standNomor == 1)
        {
            jenisMakanan -= 3;
        }else if(standNomor == 2)
        {
            jenisMakanan -= 6;
        }

        int start = GetStarterIndexByJenisStand();

        bool statusKetersediaan = true;
        for (int i = 0 + start; i < makanan[jenisMakanan].requiriment.Length + start; i++)
        {
            if (storage.penyimpananBahan[i].jumlah <= makanan[jenisMakanan].requiriment[i - start])
            {
                statusKetersediaan = false;
                break; // Keluar dari loop jika persyaratan tidak terpenuhi
            }
        }

        if (statusKetersediaan)
        {
            for (int i = 0 + start; i < makanan[jenisMakanan].requiriment.Length + start; i++)
            {
                storage.penyimpananBahan[i].jumlah -= makanan[jenisMakanan].requiriment[i - start];
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}
