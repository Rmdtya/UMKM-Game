using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPeople : MonoBehaviour
{
    public GameObject[] prefabToSpawn; // Prefab objek dua dimensi yang akan di-spawn
    public List<GameObject> prefabActive = new List<GameObject>();

    public static SpawnPeople instance;
    public GameObject tempatSpawn;

    public GameObject[] standObject;
    public Stand[] standScript;
    public CustomerMovement[] customerStatus;

    private int standActive;

    public GameObject[] locationStartStand1;
    public GameObject[] locationEndStand1;

    public GameObject[] locationStartStand2;
    public GameObject[] locationEndStand2;

    public GameObject[] locationStartStand3;
    public GameObject[] locationEndStand3;

    public int banyak;

    public GameObject spawnPoint;
    public float[] spawnProbabilities;

    public LayerMask blockingLayer; // Lapisan objek 3D yang dapat memblokir klik.

   
    private void Start()
    {
        InvokeStart();
        banyak = standObject.Length;
    }

    private void Awake()
    {
        instance = this;
    }

    public void ReinvokeSpawn()
    {
        InvokeStart();
    }

    public void InvokeStart()
    {
        StopAllCoroutines();

        standScript = new Stand[standObject.Length];
        customerStatus = new CustomerMovement[prefabToSpawn.Length];

        float bonusSpawn = UserStatus.instance.GetBonusSpawn();

        bool[] unlockStandStatus = { UserStatus.instance.GetUnlockedStand1(), UserStatus.instance.GetUnlockedStand2(), UserStatus.instance.GetUnlockedStand3() };

        for (int i = 0; i < standObject.Length; i++)
        {
            if (unlockStandStatus[i])
            {
                standScript[i] = standObject[i].GetComponent<Stand>();
                int standLevel = standScript[i].GetLevelStand();
                int jenisStand = standScript[i].GetJenisStand();
                float bonusStandSpawn = standScript[i].GetBonusSpeedSpawn();

                InvokeSpawn(standObject[i], jenisStand, standLevel, bonusSpawn, bonusStandSpawn);
            }   
        }
    }

    public void InvokeSpawn(GameObject stand, int jenisStand, int standLevel, float bonusUserSpawn, float bonusSpawnStand)
    {
        if (jenisStand == 0)
        {
            GameObject randomLocationStart = locationStartStand1[Random.Range(0, locationStartStand1.Length)];
            GameObject randomLocationEnd = locationEndStand1[Random.Range(0, locationEndStand1.Length)];

            for (int i = 0; i < prefabToSpawn.Length; i++)
            {
                customerStatus[i] = prefabToSpawn[i].GetComponent<CustomerMovement>();

                if (customerStatus[i].GetUnlockedStatus(0))
                {
                    float speed = customerStatus[i].GetCustomerSpeed() * bonusUserSpawn / bonusSpawnStand;
                    StartCoroutine(SpawnParameter(stand, randomLocationStart, randomLocationEnd, prefabToSpawn[i], speed));
                }
            }
        }

        if (jenisStand == 1)
        {
            GameObject randomLocationStart = locationStartStand2[Random.Range(0, locationStartStand1.Length)];
            GameObject randomLocationEnd = locationEndStand2[Random.Range(0, locationEndStand1.Length)];

            for (int i = 0; i < prefabToSpawn.Length; i++)
            {
                customerStatus[i] = prefabToSpawn[i].GetComponent<CustomerMovement>();

                if (customerStatus[i].GetUnlockedStatus(1))
                {
                    float speed = customerStatus[i].GetCustomerSpeed() * bonusUserSpawn * bonusSpawnStand;
                    StartCoroutine(SpawnParameter(stand, randomLocationStart, randomLocationEnd, prefabToSpawn[i], speed));
                }
            }
        }

        if (jenisStand == 2)
        {
            GameObject randomLocationStart = locationStartStand3[Random.Range(0, locationStartStand1.Length)];
            GameObject randomLocationEnd = locationEndStand3[Random.Range(0, locationEndStand1.Length)];

            for (int i = 0; i < prefabToSpawn.Length; i++)
            {
                customerStatus[i] = prefabToSpawn[i].GetComponent<CustomerMovement>();

                if (customerStatus[i].GetUnlockedStatus(2))
                {
                    float speed = customerStatus[i].GetCustomerSpeed() * bonusUserSpawn * bonusSpawnStand;
                    StartCoroutine(SpawnParameter(stand, randomLocationStart, randomLocationEnd, prefabToSpawn[i], speed));
                }
            }
        }
    }

    System.Collections.IEnumerator SpawnParameter(GameObject stand, GameObject spawnPosition, GameObject endPosition, GameObject prefab, float speedSpawn)
    {
        yield return new WaitForSeconds(speedSpawn);

        while (true)
        {
            SpawnIt(stand, spawnPosition, endPosition, prefab);

            // Tunggu 2 detik sebelum memanggil lagi
            yield return new WaitForSeconds(speedSpawn);
        }
    }


    public void SpawnIt(GameObject stand, GameObject spawnPosition, GameObject endPosition, GameObject prefab)
    {

        // Membuat objek prefab di lokasi spawnPosition
        GameObject spawnedObject = Instantiate(prefab, spawnPosition.transform.position, Quaternion.identity);
        spawnedObject.transform.SetParent(tempatSpawn.transform);

        CustomerMovement script = spawnedObject.GetComponent<CustomerMovement>();

        if (script != null)
        {
            // Setel variabel lokasi XYZ dalam skrip
            script.GetTarget(stand, endPosition);
        }
        else
        {
            Debug.LogWarning("Script not found on the spawned object.");
        }
    }

    public void ClickSpawn()
    {
        if(standActive == 1)
        {
            GameObject randomLocationStart = locationStartStand1[Random.Range(0, locationStartStand1.Length)];
            GameObject randomLocationEnd = locationEndStand1[Random.Range(0, locationEndStand1.Length)];

            int terpilih = SelectedPrefab();

            SpawnIt(standObject[0], randomLocationStart, randomLocationEnd, prefabActive[terpilih]);

            OvertimeUI.instance.AddSpawned(prefabActive[terpilih], 0);

        }

        else if(standActive == 2)
        {
            GameObject randomLocationStart = locationStartStand2[Random.Range(0, locationStartStand2.Length)];
            GameObject randomLocationEnd = locationEndStand2[Random.Range(0, locationEndStand2.Length)];

            int terpilih = SelectedPrefab();

            SpawnIt(standObject[1], randomLocationStart, randomLocationEnd, prefabActive[terpilih]);
            OvertimeUI.instance.AddSpawned(prefabActive[terpilih], 1);
        }

        else if(standActive == 3)
        {
            GameObject randomLocationStart = locationStartStand3[Random.Range(0, locationStartStand3.Length)];
            GameObject randomLocationEnd = locationEndStand3[Random.Range(0, locationEndStand3.Length)];

            int terpilih = SelectedPrefab();

            SpawnIt(standObject[2], randomLocationStart, randomLocationEnd, prefabActive[terpilih]);
            OvertimeUI.instance.AddSpawned(prefabActive[terpilih], 2);
        }
    }

    public void SetCustomerPrefabActive(int standNomor)
    {
        prefabActive.Clear();

        for (int i = 0; i < prefabToSpawn.Length; i++)
        {
            // Contoh kondisi: memilih objek yang memiliki komponen Rigidbody
            CustomerMovement customer = prefabToSpawn[i].GetComponent<CustomerMovement>();
            if (customer.GetUnlockStatus(standNomor))
            {
                // Menambahkan objek yang memenuhi kondisi ke dalam list dinamis
                prefabActive.Add(prefabToSpawn[i]);
            }
        }
        int jumlah = prefabActive.Count;
        GetPersentaseSpawn(jumlah);
    }

    public void GetPersentaseSpawn(int jumlahPrefab)
    {
        if (jumlahPrefab == 1) {
            float[] probabilitas = { 1f }; spawnProbabilities = probabilitas; 
        }else if (jumlahPrefab == 2)
        {
            float[] probabilitas = { 0.8f, 0.2f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 3)
        {
            float[] probabilitas = { 0.6f, 0.3f, 0.1f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 4)
        {
            float[] probabilitas = { 0.35f, 0.3f, 0.1f, 0.25f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 5)
        {
            float[] probabilitas = { 0.2f, 0.3f, 0.15f, 0.3f, 0.05f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 6)
        {
            float[] probabilitas = { 0.2f, 0.25f, 0.1f, 0.25f, 0.05f, 0.15f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 7)
        {
            float[] probabilitas = { 0.1f, 0.2f, 0.1f, 0.25f, 0.05f, 0.05f, 0.25f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 8)
        {
            float[] probabilitas = { 0.1f, 0.2f, 0.1f, 0.2f, 0.025f, 0.075f, 0.2f, 0.1f}; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 9)
        {
            float[] probabilitas = { 0.1f, 0.125f, 0.075f, 0.175f, 0.025f, 0.075f, 0.25f, 0.125f, 0.05f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 10)
        {
            float[] probabilitas = { 0.05f, 0.1f, 0.1f, 0.15f, 0.025f, 0.15f, 0.2f, 0.15f, 0.05f, 0.025f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 11)
        {
            float[] probabilitas = { 0.05f, 0.05f, 0.15f, 0.1f, 0.025f, 0.1f, 0.1f, 0.15f, 0.05f, 0.025f, 0.2f }; spawnProbabilities = probabilitas;
        }
        else if (jumlahPrefab == 12)
        {
            float[] probabilitas = { 0.05f, 0.05f, 0.075f, 0.125f, 0.025f, 0.125f, 0.1f, 0.1f, 0.075f, 0.025f, 0.2f, 0.05f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 13)
        {
            float[] probabilitas = { 0.05f, 0.05f, 0.05f, 0.125f, 0.025f, 0.125f, 0.075f, 0.1f, 0.075f, 0.03f, 0.17f, 0.075f, 0.05f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 14)
        {
            float[] probabilitas = { 0.05f, 0.05f, 0.05f, 0.125f, 0.025f, 0.1f, 0.075f, 0.1f, 0.085f, 0.035f, 0.17f, 0.075f, 0.05f, 0.01f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 15)
        {
            float[] probabilitas = { 0.05f, 0.05f, 0.07f, 0.1f, 0.035f, 0.075f, 0.075f, 0.075f, 0.1f, 0.04f, 0.15f, 0.075f, 0.075f, 0.015f, 0.015f }; spawnProbabilities = probabilitas;
        }else if (jumlahPrefab == 16)
        {
            float[] probabilitas = { 0.075f, 0.075f, 0.075f, 0.05f, 0.035f, 0.075f, 0.075f, 0.075f, 0.1f, 0.05f, 0.1f, 0.075f, 0.075f, 0.02f, 0.02f, 0.025f }; spawnProbabilities = probabilitas;
        }

    }

    public int SelectedPrefab()
    {
        float[] cumulativeProbabilities = new float[spawnProbabilities.Length];
        cumulativeProbabilities[0] = spawnProbabilities[0];
        for (int i = 1; i < spawnProbabilities.Length; i++)
        {
            cumulativeProbabilities[i] = cumulativeProbabilities[i - 1] + spawnProbabilities[i];
        }

        // Pilih prefab berdasarkan peluang
        float randomValue = Random.value;
        int chosenPrefabIndex = 0;
        for (int i = 0; i < cumulativeProbabilities.Length; i++)
        {
            if (randomValue <= cumulativeProbabilities[i])
            {
                chosenPrefabIndex = i;
                break;
            }
        }

        return chosenPrefabIndex;
    }

    public void ChangeActiveStand(int active)
    {
        standActive = active;
    }
    
    
}
