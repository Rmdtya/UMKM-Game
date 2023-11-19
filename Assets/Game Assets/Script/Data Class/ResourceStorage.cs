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

    public void MinusMakanan(ResepMakanan resep)
    {
        
    }
}
