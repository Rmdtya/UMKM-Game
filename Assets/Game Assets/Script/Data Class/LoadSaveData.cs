using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class LoadSaveData : MonoBehaviour
{

    public static LoadSaveData instance;

    [SerializeField]
    private PopupOfflineBonus uiOfflineBonus;
    // Start is called before the first frame update
    [SerializeField]
    private Stand[] standObject;


    [SerializeField]
    private StandStatus[] standStatus;
    [SerializeField]
    private UserData userData;
    [SerializeField]
    private Storage storage;
    [SerializeField]
    private ResepMakanan[] resepMakanan;

    [SerializeField]
    private CustomerStatus[] statusCustomer;

    public ResetStatus resetStatus;

    public bool statusStarting;

    private bool hasActive;

    [SerializeField]
    private string fileName = "SaveData.txt";


    void Awake()
    {
        statusStarting = false;
        instance = this;
        LoadData();
    }

    private void Start()
    {
        StartCoroutine(AutoSave());
    }

    public void SaveData()
    {

#if UNITY_EDITOR 
        string directory = Application.dataPath + "/Temporary";
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        string directory = Application.persistentDataPath + "/Temporary";
#endif

        var path = directory + "/" + fileName;

        //membuat File Derectory
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log("Directory has been Created: " + directory);
        }

        // Membuat File Baru
        if (File.Exists(path))
        {
            File.Create(path).Dispose();
            Debug.Log("File Save Created" + path);
        }

        SaveGameData(userData, path);
    }

    public void LoadData()
    {

#if UNITY_EDITOR
        string directory = Application.dataPath + "/Temporary";
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        string directory = Application.persistentDataPath + "/Temporary";
#endif

        var path = directory + "/" + fileName;

        try
        {
            //membuat File Derectory
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Debug.Log("Directory has been Created: " + directory);
            }

            // Membuat File Baru
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                Debug.Log("File Save Created" + path);
                SaveData();
            }
            else
            {
                Debug.Log("File Ditemukan" + path);
                LoadUserData(userData, path);
            }
        }catch(System.Exception e)
        {
            Debug.Log("error LoadData() : " + e.Message);
        }
    }

    private void SaveGameData(UserData userData, string filePath)
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate);
            fileStream.Flush();

            // Create a serializable object to hold both types of data
            SerializableGameData serializableGameData = new SerializableGameData(userData, standStatus[0], standStatus[1], standStatus[2], storage, resepMakanan, statusCustomer);

            formatter.Serialize(fileStream, serializableGameData);
            fileStream.Close();

            Debug.Log("Save Succes");
        }
        catch (System.Exception e)
        {
            Debug.Log("error SaveGameData() : " + e.Message);
        }

    }

    public void LoadUserData(UserData userData, string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = File.Open(filePath, FileMode.Open);

                // Deserialize the data back to a serializable object
                SerializableGameData serializableGameData = (SerializableGameData)formatter.Deserialize(fileStream);
                fileStream.Close();

                // Set the data back to the existing objects
                userData.SetData(serializableGameData.userData);
                standStatus[0].SetData(serializableGameData.standStatusData1);
                standStatus[1].SetData(serializableGameData.standStatusData2);
                standStatus[2].SetData(serializableGameData.standStatusData3);
                storage.SetDataStorage(serializableGameData.storageData);

                int jumlahResep = serializableGameData.resepMakanan.Length;
                for(int i = 0; i < jumlahResep; i++)
                {
                    resepMakanan[i].SetDataMakanan(serializableGameData.resepMakanan[i]);
                }

                int jumlahCustomer = serializableGameData.customerData.Length;
                for (int i = 0; i < jumlahCustomer; i++)
                {
                    statusCustomer[i].SetCustomerData(serializableGameData.customerData[i]);
                }

                LoadGame();
            }
            else
            {
                Debug.LogWarning("No save file found at: " + filePath);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("error LoadUserData() : " + e.Message);
        }

    }

    public void LoadGame()
    {
        if (userData.GetUnlockedStand2())
        {
            standObject[1].gameObject.SetActive(true);
        }
        else
        {
            standObject[1].gameObject.SetActive(false);
        }

        if (userData.GetUnlockedStand3())
        {
            standObject[2].gameObject.SetActive(true);
        }
        else
        {
            standObject[2].gameObject.SetActive(false);
        }
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            // Panggil fungsi yang ingin dijalankan setiap 1 menit di sini
            SaveData();

            // Tunggu 1 menit sebelum menjalankan lagi
            yield return new WaitForSeconds(10f); // 60 detik = 1 menit
        }
    }

    private void OnApplicationPause()
    {
        try
        {
            SaveData();
        }
        catch (Exception e)
        {
            Debug.Log("Error Save Data : " + e.Message);
            SaveData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    void OnDestroy()
    {
        try
        {
            SaveData();
        }catch (Exception e)
        {
            Debug.Log("Error Save Data : " + e.Message);
            SaveData();
        }

    }

    public void GetOfflinePoint(int durasi)
    {
        int[] makananTerbuat = new int[resepMakanan.Length];
        int[] makananTerpesan = new int[resepMakanan.Length];
        int[] makananTerjual = new int[resepMakanan.Length];
        int[] makananSisa = new int[resepMakanan.Length];

        int[] makananGagalTerbuat = new int[resepMakanan.Length];
        int[] makananTidakTerlayani = new int[resepMakanan.Length];
        double totalPendapatan = 0;
        double totalPopularity = 0;
        hasActive = false;

        for (int i = 0; i < resepMakanan.Length; i++)
        {
            if (resepMakanan[i].GetUnlockStatus())
            {
                int standNomor = resepMakanan[i].jenis;

                makananTerbuat[i] = 0;
                makananTerpesan[i] = 0;

                int jumlahCreateMakanan = Mathf.RoundToInt((float)durasi / (resepMakanan[i].speedPembuatan * userData.multipilerAutoCreateBonus / standStatus[standNomor].GetBonusAutoCreate() * 4));
                //Debug.Log(resepMakanan[i].namaMakanan + " di Create : " + jumlahCreateMakanan);

                for (int j = 0; j < jumlahCreateMakanan; j++)
                {
                    if (standObject[standNomor].OfflineCreateMakanan(i, standNomor))
                    {
                        makananTerbuat[i]++;
                        //Debug.Log("dibuat" + j.ToString());
                    }
                    else
                    {
                        makananGagalTerbuat[i]++;
                        //Debug.Log("Tidak Dibuat" + j.ToString());
                    }
                }

               // Debug.Log("Makanan Dibuat : " + makananTerbuat[i].ToString());
            }
        }

        int totalUnlockStand = UserStatus.instance.GetTotalUnlockStand();

        for (int s = 0; s < totalUnlockStand; s++)
        {
            for (int k = 0; k < statusCustomer.Length; k++)
            {
                if (statusCustomer[k].GetUnlockStatus(s))
                {
                    int jumlahSpawn = Mathf.RoundToInt((float)durasi / (statusCustomer[k].speedCustomer * userData.multipilerSpeedSpawnBonus / standStatus[s].GetBonusSpeedSpawn() * 4));
                    //Debug.Log(statusCustomer[k].namaPelanggan + " Jumlah Spawn : " + jumlahSpawn);

                    if (s == 0)
                    {
                        int makananTerpilih = statusCustomer[k].jenisBeli + 0;
                        for (int l = 0; l < jumlahSpawn; l++)
                        {
                            makananTerpesan[makananTerpilih] += statusCustomer[k].jumlahBeli;
                        }

                        //Debug.Log("Makanan Terpesan : " + makananTerpilih.ToString() + " Jumlah = " + makananTerpesan[makananTerpilih].ToString());
                    }
                    else if (s == 1)
                    {
                        int makananTerpilih = statusCustomer[k].jenisBeli + 3;
                        for (int l = 0; l < jumlahSpawn; l++)
                        {
                            makananTerpesan[makananTerpilih] += statusCustomer[k].jumlahBeli;
                        }

                        //Debug.Log("Makanan Terpesan : " + makananTerpilih.ToString() + " Jumlah = " + makananTerpesan[makananTerpilih].ToString());
                    }
                    else if (s == 2)
                    {
                        int makananTerpilih = statusCustomer[k].jenisBeli + 6;
                        for (int l = 0; l < jumlahSpawn; l++)
                        {
                            makananTerpesan[makananTerpilih] += statusCustomer[k].jumlahBeli;
                        }

                        //Debug.Log("Makanan Terpesan : " + makananTerpilih.ToString() + " Jumlah = " + makananTerpesan[makananTerpilih].ToString());
                    }
                }
            }
        }


        for(int i = 0; i < resepMakanan.Length; i++)
        {
            if(makananTerbuat[i] > 0)
            {
                //Debug.Log("Makanan Terbuat : " + makananTerbuat[i].ToString() + "- Makanan Terpesan : " + makananTerpesan[i].ToString());

                int totalTerjual = makananTerbuat[i];
                int totalPesanan = makananTerpesan[i];

                for (int j = 0; j < totalPesanan; j++)
                {
                    if (j < totalTerjual && j < totalPesanan)
                    {
                        makananTerbuat[i] --;
                        makananTerpesan[i] --;
                        makananTerjual[i]++;
                    }
                    else
                    {
                        makananSisa[i] = makananTerbuat[i];
                        makananTidakTerlayani[i] = makananTerpesan[i];
                        //Debug.Log("tidak dijual : " + j.ToString());
                        break;
                    }
                }
             //Debug.Log("Makanan Terjual : " + makananTerjual[i].ToString());

            }
        }


        for (int i = 0; i < resepMakanan.Length; i++)
        {
            if (makananTerjual[i] > 0)
            {
                int standNomor = resepMakanan[i].jenis;
                double pendapatan = resepMakanan[i].hargaMakanan * makananTerjual[i] * userData.multipilerCoinBonus + (userData.plusCoinBonus * makananTerjual[i]) + (standStatus[standNomor].GetBonusCoin() * makananTerjual[i]);
                double popularity = makananTerjual[i] * standStatus[standNomor].GetBonusPopularity() * userData.multipilerPopularityBonus;

                totalPendapatan += pendapatan;
                totalPopularity += popularity;

                
                uiOfflineBonus.AddPanel(resepMakanan[i].gambarMakanan, resepMakanan[i].namaMakanan, makananTerjual[i], pendapatan, popularity);

                if (!hasActive)
                {
                    uiOfflineBonus.SetUI();
                    hasActive = true;
                }

                Debug.Log("Fungsi Popup Dijalankan : " + i.ToString());
             }
        }

        uiOfflineBonus.SetTotalBonus(totalPendapatan);
        UserStatus.instance.CallAddCoin(totalPendapatan);
        UserStatus.instance.SetPopularityPoint(totalPopularity);

        for (int i = 0; i < resepMakanan.Length; i++)
        {
            if (makananTerjual[i] > 0)
            {
                Debug.Log("Makanan Terbuat : " + makananTerbuat[i].ToString() + " - Makanan Terjuan : " + makananTerjual[i].ToString() + " - Sisa Makanan : " + makananSisa[i].ToString() + " - Pelanggan Gagal Terlayani : " + makananTidakTerlayani[i].ToString());
            }
        }
    }

}

[System.Serializable]
public class SerializableGameData
{
    public SerializableUserData userData;
    public SerializableStandStatus standStatusData1;
    public SerializableStandStatus standStatusData2;
    public SerializableStandStatus standStatusData3;
    public SerializableStorage storageData;

    public SerializableResepMakanan[] resepMakanan;
    public SerializableCustomer[] customerData;

    public SerializableGameData(UserData userData, StandStatus standStatus1, StandStatus standStatus2, StandStatus standStatus3, Storage storage, ResepMakanan[] resep, CustomerStatus[] customer)
    {
        this.userData = new SerializableUserData(userData);
        this.standStatusData1 = new SerializableStandStatus(standStatus1, standStatus1.GetStandDekorasi());
        this.standStatusData2 = new SerializableStandStatus(standStatus2, standStatus2.GetStandDekorasi());
        this.standStatusData3 = new SerializableStandStatus(standStatus3, standStatus3.GetStandDekorasi());
        this.storageData = new SerializableStorage(storage);

        int jumlahResep = resep.Length;
        resepMakanan = new SerializableResepMakanan[jumlahResep];
        for (int i = 0; i < resep.Length; i++)
        {
            resepMakanan[i] = new SerializableResepMakanan(resep[i]);
        }

        int jumlahCustomer = customer.Length;
        customerData = new SerializableCustomer[jumlahCustomer];
        for (int i = 0; i < customer.Length; i++)
        {
            customerData[i] = new SerializableCustomer(customer[i]);
        }
    }
}


[System.Serializable]
public class SerializableUserData
{
    public double coin;
    public int diamond;
    public double popularity;

    public float multipilerCoinBonus;
    public float plusCoinBonus;
    public float multipilerSpeedSpawnBonus;
    public int multipilerSpawn;

    public float multipilerAutoCreateBonus;
    public float multipilerPopularityBonus;

    public bool manualCreateBonus;
    public bool douleAutoCreateBonus;
    public bool createWithOutResource;

    public bool standUnlocked1;
    public bool standUnlocked2;
    public bool standUnlocked3;

    // Add other fields as needed

    public SerializableUserData(UserData userData)
    {
        coin = userData.GetCoin();
        diamond = userData.GetDiamond();
        popularity = userData.GetPopularity();

        multipilerCoinBonus = userData.multipilerCoinBonus;
        plusCoinBonus = userData.plusCoinBonus;
        multipilerSpeedSpawnBonus = userData.multipilerSpeedSpawnBonus;
        multipilerSpawn = userData.multipilerSpawn;
        multipilerAutoCreateBonus = userData.multipilerAutoCreateBonus;
        multipilerPopularityBonus = userData.multipilerPopularityBonus;

        manualCreateBonus = userData.manualCreateBonus;
        douleAutoCreateBonus = userData.douleAutoCreateBonus;
        createWithOutResource = userData.createWithOutResource;

        standUnlocked1 = userData.GetUnlockedStand1();
        standUnlocked2 = userData.GetUnlockedStand2();
        standUnlocked3 = userData.GetUnlockedStand3();
    }
}


public static class UserDataExtensions
{
    public static void SetData(this UserData userData, SerializableUserData serializableData)
    {
        userData.SetLoadData(serializableData.coin, serializableData.diamond, serializableData.popularity);

        userData.SetLoadStatusData(serializableData.multipilerCoinBonus, serializableData.plusCoinBonus, serializableData.multipilerSpeedSpawnBonus,
            serializableData.multipilerSpawn, serializableData.multipilerAutoCreateBonus, serializableData.multipilerPopularityBonus,
            serializableData.manualCreateBonus, serializableData.douleAutoCreateBonus, serializableData.createWithOutResource);

        userData.SetLoadStandUnlock(serializableData.standUnlocked1, serializableData.standUnlocked2, serializableData.standUnlocked3);
        // Set other fields as needed
    }
}


[System.Serializable]
public class SerializableStandStatus
{
    public int jenisStand;
    public int levelStand;
    public string namaStand;
    public int[] jumlahMakanan;
    public int selectedManualCreate;

    public float bonusSpeedSpawn;
    public float bonusPopularity;
    public double bonusCoin;
    public int limitMaksimal;
    public float bonusAutoCreate;
    public float bonusManualCreate;

    public int[] levelDekorasi;
    public double[] hargaUpgrade;
    public float[] totalBonus;


    public SerializableStandStatus(StandStatus statusStand, Dekorasi[] dekorasiStand)
    {
        jenisStand = statusStand.jenisStand;
        levelStand = statusStand.GetLevelStand();
        namaStand = statusStand.namaStand;

        int slotMakanan = statusStand.jumlahMakanan.Length;
        jumlahMakanan = new int[slotMakanan];

        for (int i = 0; i < slotMakanan; i++)
        {
            jumlahMakanan[i] = statusStand.jumlahMakanan[i];
        }

        selectedManualCreate = statusStand.selectedManualCreate;

        bonusSpeedSpawn = statusStand.GetBonusSpeedSpawn();
        bonusPopularity = statusStand.GetBonusPopularity();
        bonusCoin = statusStand.GetBonusCoin();
        limitMaksimal = statusStand.GetLimitMaksimal();
        bonusAutoCreate = statusStand.GetBonusAutoCreate();
        bonusManualCreate = statusStand.GetBonusManualCreate();

        int banyakDekorasi = dekorasiStand.Length;
        levelDekorasi = new int[banyakDekorasi];
        hargaUpgrade = new double[banyakDekorasi];
        totalBonus = new float[banyakDekorasi];

        for (int j = 0; j < banyakDekorasi; j++)
        {
            levelDekorasi[j] = dekorasiStand[j].GetAccesorisLevel();
            hargaUpgrade[j] = dekorasiStand[j].hargaUpgrade;
            totalBonus[j] = dekorasiStand[j].totalBonus;
        }

    }
}

public static class StandDataExtensions
{
    public static void SetData(this StandStatus standStatus, SerializableStandStatus serializableStandData)
    {
        standStatus.SetLoadData(serializableStandData.jenisStand, serializableStandData.levelStand, serializableStandData.namaStand, serializableStandData.jumlahMakanan, serializableStandData.selectedManualCreate,
            serializableStandData.bonusSpeedSpawn, serializableStandData.bonusPopularity, serializableStandData.bonusCoin, serializableStandData.limitMaksimal, serializableStandData.bonusAutoCreate, serializableStandData.bonusManualCreate,
            serializableStandData.levelDekorasi, serializableStandData.hargaUpgrade, serializableStandData.totalBonus);
        // Set other fields as needed
    }
}

[System.Serializable]
public class SerializableStorage
{
    public float[] jumlahBahan;
    public float limitMaksimal;


    public SerializableStorage(Storage storage)
    {
        int banyakBahan = storage.GetSizeBahan();
        KebutuhanBahan[] bahan = new KebutuhanBahan[banyakBahan];
        jumlahBahan = new float[banyakBahan];

        bahan = storage.GetbBahanBahan();

        limitMaksimal = storage.GetLimitMaksimal();

        for(int i = 0; i < banyakBahan; i++)
        {
            jumlahBahan[i] = bahan[i].jumlah;
        }
    }
}

public static class StorageDataExtensions
{
    public static void SetDataStorage(this Storage storage, SerializableStorage serializableStorage)
    {
        storage.LoadDataStorage(serializableStorage.jumlahBahan, serializableStorage.limitMaksimal);
        // Set other fields as needed
    }
}

[System.Serializable]
public class SerializableResepMakanan
{
    public int levelMakanan;
    public double hargaMakanan;
    public double hargaUpgrade;
    public bool unlocked;

    public double totalPeningkatanHargaMakanan;

    // Add other fields as needed

    public SerializableResepMakanan(ResepMakanan resep)
    {
        levelMakanan = resep.GetLevelMakanan();
        hargaMakanan = resep.hargaMakanan;
        hargaUpgrade = resep.hargaUpgrade;

        unlocked = resep.GetUnlockStatus();
        totalPeningkatanHargaMakanan = resep.totalPeningkatanHargaMakanan;
    }
}

public static class MakananDataExtensions
{
    public static void SetDataMakanan(this ResepMakanan resep, SerializableResepMakanan serializableResepMakanan)
    {
        resep.LoadDataResep(serializableResepMakanan.levelMakanan, serializableResepMakanan.hargaMakanan, serializableResepMakanan.hargaUpgrade, 
            serializableResepMakanan.unlocked, serializableResepMakanan.totalPeningkatanHargaMakanan);
        // Set other fields as needed
    }
}

[System.Serializable]
public class SerializableCustomer
{
    public bool[] unlocked;

    public double totalPeningkatanHargaMakanan;

    // Add other fields as needed

    public SerializableCustomer(CustomerStatus customer)
    {
        unlocked = customer.GetAllUnlockStatus();
    }
}

public static class CustomerDataExtensions
{
    public static void SetCustomerData(this CustomerStatus customer, SerializableCustomer serializableCustomer)
    {
        customer.LoadData(serializableCustomer.unlocked);
    }
}

