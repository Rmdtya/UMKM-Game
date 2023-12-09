using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "Storage")]
public class Storage : ScriptableObject
{

    [SerializeField]
    private float limitMaksimal;

    public KebutuhanBahan[] penyimpananBahan;

    public int GetSizeBahan()
    {
        return penyimpananBahan.Length;
    }

    public void TambahLimitMakanan(float limit)
    {
        limitMaksimal += limit;
    }

    public KebutuhanBahan[] GetbBahanBahan()
    {
        return penyimpananBahan;
    }

    public float GetLimitMaksimal()
    {
        return limitMaksimal;
    }

    public float GetTotalBahan()
    {
        float jumlahTotal = 0;
        for(int i = 0; i < penyimpananBahan.Length; i++)
        {
            jumlahTotal += penyimpananBahan[i].jumlah;
        }

        return jumlahTotal;
    }

    public void LoadDataStorage(float[] jumlahBahan, float limit)
    {
        for(int i = 0 ; i < jumlahBahan.Length; i++)
        {
            penyimpananBahan[i].jumlah = jumlahBahan[i];
        }

        limitMaksimal = limit;
    }

    public void Resetdata()
    {
        for (int i = 0; i < penyimpananBahan.Length; i++)
        {
            if(i <= 10)
            {
                penyimpananBahan[i].jumlah = 100;
            }
            else
            {
                penyimpananBahan[i].jumlah = 0;
            }
        }

        limitMaksimal = 5000;
    }
}

[System.Serializable]
public struct KebutuhanBahan
{
    public string namaBahan;
    public float jumlah;
    public Sprite spriteBahan;
}

