using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMovement : MonoBehaviour {

    public GameObject targetStand;
    public float moveSpeed = 4f;
    private GameObject endTarget;

    public SpriteRenderer angryIcon;
    public SpriteRenderer pose1;
    public SpriteRenderer pose2;
    public SpriteRenderer pose3;
    public SpriteRenderer pose4;

    private bool isAngry = false;

    public CustomerStatus customerStatus;

    public float totalDistance = 0f;
    public bool isWaiting = false;
    public float waitTime = 2f;
    public bool isTransaksi = false;

    public bool perjalananMulai = false;
    public bool perjalananPulang = false;

    private Vector3 targetOffset;

    // Start is called before the first frame update
    void Start()
    {
        isAngry = false;
        angryIcon.enabled = false;
        ShowPose1();

        float randomXOffset = Random.Range(-150, 150);
        float randomYOffset = Random.Range(-575, -595);

        // Menetapkan nilai offset ke posisi targetOffset
        targetOffset = new Vector3(randomXOffset, randomYOffset, 0f);

        Application.targetFrameRate = 60;

        isWaiting = false;
        isTransaksi = false;
        perjalananPulang = false;
    }

    public void GetTarget(GameObject stand, GameObject end)
    {
        targetStand = stand;
        endTarget = end;
    }

    private void Update()
    {
        // Hitung pergerakan ke kiri dan ke atas setiap frame

        if (!perjalananMulai)
        {
                float step = moveSpeed * Time.deltaTime;
                Vector3 targetPositionWithOffset = targetOffset + targetStand.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, targetPositionWithOffset, step);

                // Update total jarak yang sudah ditempuh
                totalDistance += step;

                // Jika sudah bergerak sejauh 300f, mulai waktu tunggu
                if (transform.position == targetPositionWithOffset)
                {
                    isWaiting = true;
                    perjalananMulai = true;
                    ShowPose2();
                    waitTime = 2f; // Setel waktu tunggu selama dua detik

                }
        }

        if (isWaiting)
        {
            waitTime -= Time.deltaTime;
           
            
            if (waitTime < 0f)
            {
                isWaiting = false;
                perjalananPulang = true;
            }
        }

        if (perjalananPulang)
        {
            Stand scriptComponent = targetStand.GetComponent<Stand>();

            if (!isTransaksi)
            {
                if (scriptComponent != null)
                {
                    bool transaksi = scriptComponent.Transaction(customerStatus.jumlahBeli, customerStatus.jenisBeli, customerStatus.skill);
                    isTransaksi = true;

                    if (transaksi)
                    {
                        Debug.Log("Berhasil");
                        StartCoroutine(MoveToLastPosition());
                        int jenisStand = scriptComponent.GetJenisStand();

                        if (jenisStand == 1)
                        {
                            ShowPose3();
                        }
                        else if (jenisStand == 2)
                        {
                            ShowPose4();
                        }

                    }
                    else
                    {
                        isAngry = true;
                        ShowAngryIcon();
                        Debug.Log("Makanan Kurang");
                        StartCoroutine(MoveToLastPosition());
                        ShowPose1();
                    }
                }
            }

            float step = moveSpeed * Time.deltaTime;
            Vector3 targetEnd = endTarget.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetEnd, step);

            if(transform.position == targetEnd)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator MoveToLastPosition()
    {
        // Tunggu sampai mencapai posisi Stand terpilih
        float startTime = Time.time;
        /*float journeyLength = Vector3.Distance(transform.position, endLoc);*/

        Debug.Log("bergerak");

        while (transform.position != transform.position)
        {
            Destroy(gameObject);
         
            yield return null;
        }
    }

    public float GetCustomerSpeed()
    {
        return customerStatus.speedCustomer;
    }

    public bool GetUnlockedStatus1()
    {
        return customerStatus.unlockedStand1;
    }

    public bool GetUnlockedStatus2()
    {
        return customerStatus.unlockedStand2;
    }

    public bool GetUnlockedStatus3()
    {
        return customerStatus.unlockedStand3;
    }

    private void ShowAngryIcon()
    {
        angryIcon.enabled = true;
    }

    private void ShowPose1()
    {
        pose1.enabled = true;
        pose2.enabled = false;
        pose3.enabled = false;
        pose4.enabled = false;

    }

    private void ShowPose2()
    {
        pose1.enabled = false;
        pose2.enabled = true;
        pose3.enabled = false;
        pose4.enabled = false;
    }

    private void ShowPose3()
    {
        pose1.enabled = false;
        pose2.enabled = false;
        pose3.enabled = true;
        pose4.enabled = false;
    }

    private void ShowPose4()
    {
        pose1.enabled = false;
        pose2.enabled = false;
        pose3.enabled = false;
        pose4.enabled = true;
    }


}
