using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "customerStatus", menuName = "CustomerStatus")]

public class CustomerStatus : ScriptableObject
{
    public string namaPelanggan;
    public int lvCustomer;
    public float speedCustomer;
    public int jenisBeli;
    public int jumlahBeli;

    [SerializeField]
    private bool[] unlockedStand;
    public CustomerSpecialSkill skill;

    public double syaratPopularity;
    public int syaratLevel;
    public string deskripsiUnlock;


    public bool[] GetAllUnlockStatus() { return unlockedStand;}

    public bool GetUnlockStatus(int index) { return unlockedStand[index]; }

    public void UnlockStatus(int index)
    {
        unlockedStand[index] = true;
    }

    public void LoadData(bool[] statusUnlock)
    {
        for(int i = 0; i< unlockedStand.Length; i++)
        {
            unlockedStand[i] = statusUnlock[i];
        }
    }

    public void ResetData(bool kondisi)
    {
        for(int i = 0; i < unlockedStand.Length; i++)
        {
            unlockedStand[i] = kondisi;
        }
    }
}

