using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public float panSpeed = 30f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 5f;
    public float minY = 10f;
    public float maxY = 80f;
    void Update()
    {

        /*if (GameManage.gameIsOver)
        {
            this.enabled = false;
            return;
        }*/
            

        




        if (Input.GetKey("w"))                   // Kondisi or jika mouse berada 10 pixel di screen paling atas
        {
            // new Vector3(0f, 0f, panSpeed * Time.deltaTime);  The Logic of Translate
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);      //Space.world digunakan agar pergerakan kamera menjadi tidak local (global)
        }

        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);     //Penggunaan back bisa diganti dengan menuliskan (-) -Vector3.forward
        }

        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);

        }

        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);

        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;




        /*  if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)                   // Kondisi or jika mouse berada 10 pixel di screen paling atas
          {
              // new Vector3(0f, 0f, panSpeed * Time.deltaTime);  The Logic of Translate
              transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);      //Space.world digunakan agar pergerakan kamera menjadi tidak local (global)
          }
          if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)                  
          {
              transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);     //Penggunaan back bisa diganti dengan menuliskan (-) -Vector3.forward
          }
          if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)                 
          {
              transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);      
          }
          if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
          {
              transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
          }   */

    }
}
