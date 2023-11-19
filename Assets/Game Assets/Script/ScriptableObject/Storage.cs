using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "Storage")]
public class Storage : ScriptableObject
{
    [System.Serializable]
    public struct KebutuhanBahan
    {
        public string namaBahan;
        public float jumlah;
    }

    public int jenis;
    public double hargaMakanan;

    public KebutuhanBahan[] penyimpananBahan;
}


