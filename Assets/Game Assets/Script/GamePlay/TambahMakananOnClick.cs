using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TambahMakananOnClick : MonoBehaviour
{
    public Stand stand; // Referensi ke skrip Stand yang memiliki jumlahMakanan

    private void OnMouseDown()
    {
        // Tambahkan makanan ke Stand
        if (stand != null)
        {
            stand.TambahMakananPerClick();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
