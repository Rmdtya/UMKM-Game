using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "customerStatus", menuName = "CustomerStatus")]

public class CustomerStatus : ScriptableObject
{
    
    public int lvCustomer;
    public float speedCustomer;
    public int jenisBeli;
    public int jumlahBeli;

    public bool unlockedStand1 = false;
    public bool unlockedStand2 = false;
    public bool unlockedStand3 = false;
    public CustomerSpecialSkill skill;
}

