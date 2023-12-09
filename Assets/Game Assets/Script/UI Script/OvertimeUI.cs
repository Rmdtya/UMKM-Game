using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OvertimeUI : MonoBehaviour
{
    public static OvertimeUI instance;

    [SerializeField] private ResepMakanan[] resepMakanan;
    [SerializeField] private CustomerStatus[] customer;

    public TextMeshProUGUI textOvertime;

    public int intervalOvertime = 5;

    public List<CustomerStatus> customerSpawn = new List<CustomerStatus>();
    public List<int> position = new List<int>();

    public double valueOvertime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Coroutine());
    }

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(intervalOvertime);

        while (true)
        {
            SetUI();

            // Tunggu 2 detik sebelum memanggil lagi
            yield return new WaitForSeconds(intervalOvertime);
        }
    }

    public void SetUI()
    {
        valueOvertime = 0;

        for(int i = 0; i < customer.Length; i++)
        {
            if (customer[i].GetUnlockStatus(0))
            {
                double jumlah = 0;
                if(customer[i].jenisBeli == 0)
                {
                    jumlah = customer[i].speedCustomer / intervalOvertime * customer[i].jumlahBeli * resepMakanan[0].hargaMakanan;
                }
                else if(customer[i].jenisBeli == 1)
                {
                    jumlah = customer[i].speedCustomer / intervalOvertime * customer[i].jumlahBeli * resepMakanan[1].hargaMakanan;
                }
                else if(customer[i].jenisBeli == 2)
                {
                    jumlah = customer[i].speedCustomer / intervalOvertime * customer[i].jumlahBeli * resepMakanan[2].hargaMakanan;
                }

                valueOvertime += jumlah;
            }

            if (customer[i].GetUnlockStatus(1))
            {
                double jumlah = 0;
                if (customer[i].jenisBeli == 0)
                {
                    jumlah = customer[i].speedCustomer / intervalOvertime * customer[i].jumlahBeli * resepMakanan[3].hargaMakanan;
                }
                else if (customer[i].jenisBeli == 1)
                {
                    jumlah = customer[i].speedCustomer / intervalOvertime * customer[i].jumlahBeli * resepMakanan[4].hargaMakanan;
                }
                else if (customer[i].jenisBeli == 2)
                {
                    jumlah = customer[i].speedCustomer / intervalOvertime * customer[i].jumlahBeli * resepMakanan[5].hargaMakanan;
                }

                valueOvertime += jumlah;
            }

            if (customer[i].GetUnlockStatus(2))
            {
                double jumlah = 0;
                if (customer[i].jenisBeli == 0)
                {
                    jumlah = customer[i].speedCustomer / intervalOvertime * customer[i].jumlahBeli * resepMakanan[6].hargaMakanan;
                }
                else if (customer[i].jenisBeli == 1)
                {
                    jumlah = customer[i].speedCustomer / intervalOvertime * customer[i].jumlahBeli * resepMakanan[7].hargaMakanan;
                }
                else if (customer[i].jenisBeli == 2)
                {
                    jumlah = customer[i].speedCustomer / intervalOvertime * customer[i].jumlahBeli * resepMakanan[8].hargaMakanan;
                }

                valueOvertime += jumlah;
            }
        }

        SetSpawned();
    }

    public void SetSpawned()
    {
        for(int i = 0; i< customerSpawn.Count; i++)
        {
            if (position[i] == 0)
            {
                double jumlah = 0;
                if (customerSpawn[i].jenisBeli == 0)
                {
                    jumlah = customerSpawn[i].speedCustomer / intervalOvertime * customerSpawn[i].jumlahBeli * resepMakanan[0].hargaMakanan;
                }
                else if (customerSpawn[i].jenisBeli == 1)
                {
                    jumlah = customerSpawn[i].speedCustomer / intervalOvertime * customerSpawn[i].jumlahBeli * resepMakanan[1].hargaMakanan;
                }
                else if (customerSpawn[i].jenisBeli == 2)
                {
                    jumlah = customerSpawn[i].speedCustomer / intervalOvertime * customerSpawn[i].jumlahBeli * resepMakanan[2].hargaMakanan;
                }

                valueOvertime += jumlah;
            }else if (position[i] == 1)
            {
                double jumlah = 0;
                if (customerSpawn[i].jenisBeli == 0)
                {
                    jumlah = customerSpawn[i].speedCustomer / intervalOvertime * customerSpawn[i].jumlahBeli * resepMakanan[3].hargaMakanan;
                }
                else if (customerSpawn[i].jenisBeli == 1)
                {
                    jumlah = customerSpawn[i].speedCustomer / intervalOvertime * customerSpawn[i].jumlahBeli * resepMakanan[4].hargaMakanan;
                }
                else if (customerSpawn[i].jenisBeli == 2)
                {
                    jumlah = customerSpawn[i].speedCustomer / intervalOvertime * customerSpawn[i].jumlahBeli * resepMakanan[5].hargaMakanan;
                }

                valueOvertime += jumlah;
            }else if (position[i] == 2)
            {
                double jumlah = 0;
                if (customerSpawn[i].jenisBeli == 0)
                {
                    jumlah = customerSpawn[i].speedCustomer / intervalOvertime * customerSpawn[i].jumlahBeli * resepMakanan[6].hargaMakanan;
                }
                else if (customerSpawn[i].jenisBeli == 1)
                {
                    jumlah = customerSpawn[i].speedCustomer / intervalOvertime * customerSpawn[i].jumlahBeli * resepMakanan[7].hargaMakanan;
                }
                else if (customerSpawn[i].jenisBeli == 2)
                {
                    jumlah = customerSpawn[i].speedCustomer / intervalOvertime * customerSpawn[i].jumlahBeli * resepMakanan[8].hargaMakanan;
                }

                valueOvertime += jumlah;
            }
        }

        textOvertime.text = FormatOvertime(valueOvertime);

        customerSpawn.Clear();
        position.Clear();
    }

    public void AddSpawned(GameObject obj, int standNomor)
    {
        CustomerMovement script = obj.GetComponent<CustomerMovement>();
        CustomerStatus status = script.GetCustomerScript();

        customerSpawn.Add(status);
        position.Add(standNomor);
    }

    string FormatOvertime(double value)
    {
        if (value < 1000)
        {
            return value.ToString("F0");
        }
        else if (value < 1000000)
        {
            double valueInK = value / 1000;
            return valueInK.ToString("F1") + "k";
        }
        else
        {
            double valueInM = value / 1000000;
            return valueInM.ToString("F1") + "jt";
        }
    }
}
