using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    public ResourceBahan[] bahan; // Prefab objek dua dimensi yang akan di-spawn
    public static ResourceStorage instance;

    [SerializeField]
    private Storage storage;

    private void Awake()
    {
        instance = this;
    }

    public void TambahLimitMaksimal(float limit)
    {
        storage.TambahLimitMakanan(limit);
    }

    public void TambahJumlah(int index, float jumlah)
    {
        storage.penyimpananBahan[index].jumlah += jumlah;
    }

    public void UnlockStand2(float jumlah)
    {
        for (int i = 11; i <= 19; i++)
        {
            storage.penyimpananBahan[i].jumlah += jumlah;
        }
    }
}
