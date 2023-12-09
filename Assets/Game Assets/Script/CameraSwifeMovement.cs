using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwifeMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float swipeSpeed = 1.0f;
    public float moveThreshold = 200f;

    public Vector3 initialPosition;
    public Vector3 targetPosition;

    public Vector3 stand1;
    public Vector3 stand2;
    public Vector3 stand3;
    public float moveSpeed = 5f;

    [SerializeField]
    public bool isDragging = false;
    public bool isWaiting = false;
    [SerializeField]
    private int standActive;

    public float swipeDistance;

    private void Start()
    {
        SpawnPeople.instance.ChangeActiveStand(1);
        SpawnPeople.instance.SetCustomerPrefabActive(0);
    }

    private void Update()
    {
        if (GameManager.instance.popupActive)
            return;


            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                standActive = GameManager.instance.GetStandActive();

                if (touch.phase == TouchPhase.Began)
                {
                    isDragging = true;
                   
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = touch.deltaPosition.x * swipeSpeed;
                    targetPosition.x -= deltaX;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    isDragging = false;

                    swipeDistance = targetPosition.x - initialPosition.x;
                    float swipePoint = Mathf.Abs(targetPosition.x - initialPosition.x);

                    if (swipePoint < moveThreshold)
                    {
                        targetPosition = initialPosition;
                    }
                    else
                    {
                        standActive = GameManager.instance.GetStandActive();

                        if (standActive == 1)
                        {
                            if (swipeDistance < -300f)
                            {
                                targetPosition = stand2;
                                GameManager.instance.UpdateStandActive(2);
                                SpawnPeople.instance.ChangeActiveStand(2);
                                SpawnPeople.instance.SetCustomerPrefabActive(1);
                            }
                            else if (swipeDistance > 300)
                            {
                                targetPosition = stand3;
                                GameManager.instance.UpdateStandActive(3);
                                SpawnPeople.instance.ChangeActiveStand(3);
                                SpawnPeople.instance.SetCustomerPrefabActive(2);
                            }
                            initialPosition = targetPosition;

                        }
                        else if (standActive == 2)
                        {
                            if (swipeDistance < -300f)
                            {
                                targetPosition = initialPosition;
                            }
                            else if (swipeDistance > 300)
                            {
                                targetPosition = stand1;
                                GameManager.instance.UpdateStandActive(1);
                                SpawnPeople.instance.ChangeActiveStand(1);
                                SpawnPeople.instance.SetCustomerPrefabActive(0);
                            }

                            initialPosition = targetPosition;
                        }
                        else if (standActive == 3)
                        {
                            if (swipeDistance < -300f)
                            {
                                targetPosition = stand1;
                                GameManager.instance.UpdateStandActive(1);
                                SpawnPeople.instance.ChangeActiveStand(1);
                                SpawnPeople.instance.SetCustomerPrefabActive(0);
                            }
                            else if (swipeDistance > 300)
                            {
                                targetPosition = initialPosition;
                            }

                            initialPosition = targetPosition;
                        }

                    }

                }
            }
            
        if (!isDragging)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime *5);
        }
        else
        {
            mainCamera.transform.position = targetPosition;
        }
    }

    public void MoveCameraToStand(int standNomor)
    {
        StartCoroutine(MoveToStandLocation(standNomor));
        GameManager.instance.popupActive = true;
    }


    IEnumerator MoveToStandLocation(int standNomor)
    {
        if(standNomor == 2)
        {
            
            while (Vector3.Distance(transform.position, stand2) > 0.1f)
            {
                
                // Menggunakan Lerp untuk pergerakan yang halus
                transform.position = Vector3.Lerp(mainCamera.transform.position, stand2, Time.deltaTime * 5);

                yield return null;
                Debug.Log("Pergerakan Masih Berlangsung!");
            }

            // Pergerakan selesai ketika mencapai titik target
            Debug.Log("Pergerakan selesai!");
            GameManager.instance.UpdateStandActive(2);
            SpawnPeople.instance.ChangeActiveStand(2);
            GameManager.instance.popupActive = false;
            targetPosition = stand2;
            initialPosition = targetPosition;
        }
        
        
        if(standNomor == 3)
        {
            while (Vector3.Distance(transform.position, stand3) > 0.1f)
            {
               
                // Menggunakan Lerp untuk pergerakan yang halus
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, stand3, Time.deltaTime * 5);

                yield return null;
            }

            Debug.Log("Pergerakan selesai!");
            GameManager.instance.UpdateStandActive(3);
            SpawnPeople.instance.ChangeActiveStand(3);
            GameManager.instance.popupActive = false;
            targetPosition = stand3;
            initialPosition = targetPosition;
        }
        
    }
}
