using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class OfflineActivity : MonoBehaviour
{
    private const string PlayerPrefsKey = "SavedDateTime";

    public DateTime localDateTime, UTCdateTime;

    public static OfflineActivity instance;
    public bool isStart;

    private void Awake()
    {
        isStart = false;
        instance = this;
    }
    public struct ServerDateTime
    {
        public string datetime;
        public string utc_datetime;
    }

    private void Start()
    {
        StartCoroutine(GetTimeServer());
    }

    public void SetWorldTime()
    {
        DateTime timeNow = WorldTimeAPI.Instance.GetCurrentDateTime();
        Debug.Log("WorldTimeAPI : " + timeNow.ToString());
    }

    public IEnumerator GetTimeServer()
    {

        UnityWebRequest request = UnityWebRequest.Get("http://worldtimeapi.org/api/ip");
        yield return request.SendWebRequest();

            ServerDateTime serverDateTime = JsonUtility.FromJson<ServerDateTime>(request.downloadHandler.text);
            localDateTime = parseToDateTime(serverDateTime.datetime);
            UTCdateTime = parseToDateTime(serverDateTime.utc_datetime);
            Debug.Log("waktu Local : " + localDateTime.ToString());
            Debug.Log("UTC : " + UTCdateTime.ToString());

            if (PlayerPrefs.HasKey(PlayerPrefsKey))
            {
                // Mengambil data DateTime dari PlayerPrefs dan mengonversinya ke dalam DateTime
                
                string savedDateTimeString = PlayerPrefs.GetString(PlayerPrefsKey);
                DateTime savedDateTime = DateTime.Parse(savedDateTimeString);

                Debug.Log("Last Active Prefs Ada : " + savedDateTime.ToString());

                CheckLastDateTime(savedDateTime, localDateTime);
            }
            else
            {
                // Jika PlayerPrefs tidak memiliki data DateTime sebelumnya, simpan waktu saat ini
                Debug.Log("Last Active Prefs Tidak Ada");
                SaveCurrentDateTime(localDateTime);
                InvokeRepeating("SetLastActive", 60f, 60f);
        }

        
    }

    public void CheckLastDateTime(DateTime lastActive, DateTime timeNow)
    {
        TimeSpan timeDifference = timeNow - lastActive;
        float minutesDifference = (float)timeDifference.TotalMinutes;
        int secondsDifference = Mathf.RoundToInt((float)timeDifference.TotalMinutes * 60);

        if(secondsDifference >= 43200)
        {
            secondsDifference = 43200;
        }

        // Menampilkan informasi perbedaan waktu
        Debug.Log("Perbedaan waktu dengan saat ini: " + minutesDifference + " menit");
        Debug.Log("Perbedaan waktu dengan saat ini: " + secondsDifference + " Detik");

        if(secondsDifference >= 60)
        {
            LoadSaveData.instance.GetOfflinePoint(secondsDifference);
            Debug.Log("GetOffline Bonus");
        }

        SaveCurrentDateTime(timeNow);
        InvokeRepeating("SetLastActive", 60f, 60f);
        isStart = true;
    }

    DateTime parseToDateTime(string value)
    {
        string date = Regex.Match(value, @"^\d{4}-\d{2}-\d{2}").Value;

        string time = Regex.Match(value, @"\d{2}:\d{2}:\d{2}").Value;

        return DateTime.Parse(string.Format("{0} {1}", date, time));
    }

    void SaveCurrentDateTime(DateTime date)
    {

        // Menyimpan DateTime ke dalam PlayerPrefs
        PlayerPrefs.SetString(PlayerPrefsKey, date.ToString());
        PlayerPrefs.Save();

        Debug.Log("Waktu saat ini disimpan: " + date.ToString());
    }

    public IEnumerator SetLastActive()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://worldtimeapi.org/api/ip");
        yield return request.SendWebRequest();

        try
        {
            ServerDateTime serverDateTime = JsonUtility.FromJson<ServerDateTime>(request.downloadHandler.text);
            DateTime lastTime = parseToDateTime(serverDateTime.datetime);

            Debug.Log("Last Active Saved: " + localDateTime + " Detik");
            SaveCurrentDateTime(lastTime);
        }catch (Exception e)
        {
            Debug.Log("Error Last Active Saved: " + e.Message);
        }
    }

    private void OnApplicationPause()
    {
        if (isStart)
        {
            try
            {
                StartCoroutine(SetLastActive());
            }
            catch (Exception e)
            {
                Debug.Log("Error Set Last Active Data : " + e.Message);
                StartCoroutine(SetLastActive());
            }
        }
    }
}
