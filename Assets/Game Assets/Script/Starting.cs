using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Starting : MonoBehaviour
{
    private const string FirstRunKey = "FirstRun";

    [SerializeField]
    private string fileName = "SaveData.txt";
    // Start is called before the first frame update

    public StartingData startingData;


    void Awake()
    {

        Debug.Log("Nilai Prefs 2" + PlayerPrefs.HasKey(FirstRunKey));
        if (PlayerPrefs.GetInt(FirstRunKey, 1) == 1)
        {
            // Jika belum pernah dijalankan, jalankan fungsi pertama kali
            DeleteSaveData();

            //ResetStatus.instance.ResetData();

            startingData.isStarting = false;
            Debug.Log("Fungsi Starting Dijalankan");

            // Set PlayerPrefs agar tidak menjalankan fungsi ini lagi di masa depan
            PlayerPrefs.SetInt(FirstRunKey, 0);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Fungsi Tidak Dijalankan");
            startingData.isStarting = true;
        }
    }

    void DeleteSaveData()
    {

#if UNITY_EDITOR
        string directory = Application.dataPath + "/hehe";
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        string directory = Application.persistentDataPath + "/Temporary/";
#endif

        var path = directory + "/" + fileName;

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("File berhasil dihapus: " + path);
        }
        else
        {
            Debug.Log("File tidak ditemukan: " + path);
        }
    }
}
