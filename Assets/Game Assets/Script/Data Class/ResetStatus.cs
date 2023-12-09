using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStatus : MonoBehaviour
{
    public static ResetStatus instance;

    public UserData userData;
    public StandStatus[] standStatus;
    public ResepMakanan[] resepMakanan;
    public Storage storage;
    public CustomerStatus[] customer;
    // Start is called before the first frame update
    public void ResetData()
    {
        userData.ResetStatus();
        for(int i =0; i<standStatus.Length; i++)
        {
            standStatus[i].ResetData();
        }

        for(int j = 0; j < resepMakanan.Length; j++)
        {
            if(j == 0)
            {
                resepMakanan[j].ResetData(true);
            }
            else
            {
                resepMakanan[j].ResetData(false);
            }
        }

        storage.Resetdata();

        for (int j = 0; j < customer.Length; j++)
        {
            if (j == 0)
            {
                customer[j].ResetData(true);
            }
            else
            {
                customer[j].ResetData(false);
            }
        }

        Debug.Log("Reset Berhasil");
    }

    // Update is called once per frame
    private void Awake()
    {
        instance = this;
    }

    public void AddDiamond()
    {
        UserStatus.instance.CallAddDiamond(100);
    }
}
