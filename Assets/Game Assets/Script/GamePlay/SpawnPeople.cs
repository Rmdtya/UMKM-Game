using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPeople : MonoBehaviour
{
    public GameObject[] prefabToSpawn; // Prefab objek dua dimensi yang akan di-spawn

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
        if (jenisStand == 1)
        {
            GameObject randomLocationStart = locationStartStand1[Random.Range(0, locationStartStand1.Length)];
            GameObject randomLocationEnd = locationEndStand1[Random.Range(0, locationEndStand1.Length)];

            for (int i = 0; i < prefabToSpawn.Length; i++)
            {
                customerStatus[i] = prefabToSpawn[i].GetComponent<CustomerMovement>();

                if (customerStatus[i].GetUnlockedStatus1())
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

            SpawnIt(standObject[0], randomLocationStart, randomLocationEnd, prefabToSpawn[0]);

        }

        else if(standActive == 2)
        {
            
        }

        else if(standActive == 3)
        {
            
        }
    
    }

    public void ChangeActiveStand(int active)
    {
        standActive = active;
    }
}
