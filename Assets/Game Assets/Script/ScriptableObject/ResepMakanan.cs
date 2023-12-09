using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resep", menuName = "ResepMakanan")]
public class ResepMakanan : ScriptableObject
{

    public string namaMakanan;
    [SerializeField]
    private int levelMakanan;
    public int jenis;
    public double hargaMakanan;
    public float speedPembuatan;
    public string deskripsiMakanan;
    public double hargaUpgrade;
    public Sprite gambarMakanan;
    [SerializeField]
    private bool unlocked;
    public string deskripsiUnlock;
    public Sprite gambarLocked;

    [Header("Requiriment")]
    public float[] requiriment;
    public Sprite[] spriteRequiriment;

    public float[] kelipatanHarga;
    public float[] bonusUpgrade;
    public double totalPeningkatanHargaMakanan;
    public int flatBonus;

    

    public int GetLevelMakanan()
    {
        return levelMakanan;
    }

    public Sprite GetSpriteMakanan()
    {
        return gambarMakanan;
    }

    public Sprite[] GetSpriteBahan()
    {
        return spriteRequiriment;
    }

    public bool GetUnlockStatus()
    {
        return unlocked;
    }

    public void UnlockMakanan()
    {
        unlocked = true;
    }

    public void UpgradeMakanan(int levelStand)
    {
        levelMakanan++;
        hargaMakanan += bonusUpgrade[levelStand];
        hargaUpgrade = hargaUpgrade * kelipatanHarga[levelStand];

        totalPeningkatanHargaMakanan += hargaMakanan;
    }

    public void LoadDataResep(int level, double makanan, double upgrade, bool unlock, double totalpeningkatan)
    {
        levelMakanan = level;
        hargaMakanan = makanan;
        hargaUpgrade = upgrade;
        unlocked = unlock;
        totalPeningkatanHargaMakanan = totalpeningkatan;
    }

    public void ResetData(bool condition)
    {
        unlocked = condition;
        levelMakanan = 1;
    }
}
