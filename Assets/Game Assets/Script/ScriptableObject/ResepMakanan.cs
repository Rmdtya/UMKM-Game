using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resep", menuName = "ResepMakanan")]
public class ResepMakanan : ScriptableObject
{
    public int levelMakanan;
    public int jenis;
    public double hargaMakanan;
    public float speedPembuatan;

    [Header("Requiriment")]
    public float[] requiriment;
}
