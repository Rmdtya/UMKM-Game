using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading;
using Firebase.Extensions;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System;
using TMPro;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

public class GooglePlayManager : MonoBehaviour
{
    public string GamePlayscene; // Nama scene yang ingin Anda pindahkan

    public float displayDuration = 3.0f;
    private bool isDisplaying = false;

    private string userID;
    private string username;
    bool isConnected;

    private FirebaseFirestore firestore;
    private string authCode;

    public TextMeshProUGUI textNotification;
    public TextMeshProUGUI firebasetext;
    public GameObject notificationNetwork;
    public GameObject notificationPanel;

    public GameObject buttonStart;
    public GameObject buttonQuit;

    public void Awake()
    {
        firestore = FirebaseFirestore.DefaultInstance;
    }


    public void Start()
    {
        notificationNetwork.SetActive(false);
        buttonStart.SetActive(false);
        buttonQuit.SetActive(false);

        if (!IsInternetAvailable())
        {
            ShowNotification("Tidak Ada Sinyal Internet");
            notificationNetwork.SetActive(true);
        }
        else
        {
            GPSLogin();
        }
    }

 
    public void GPSLogin()
    {
        try
        {
            PlayGamesPlatform.Instance.Authenticate((success) =>
            {
                if (success == SignInStatus.Success)
                {
                    // Continue with Play Games Services

                    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
                    {
                        if (task.Result == Firebase.DependencyStatus.Available)
                        {
                            //No Dependancy issue with firebase :: continue with login
                            ConnectToFirebase();
                            userID = PlayGamesPlatform.Instance.localUser.id;
                            firebasetext.text = PlayGamesPlatform.Instance.GetUserDisplayName() + PlayGamesPlatform.Instance.localUser.id;
                            CekFirebase();

                            buttonStart.SetActive(true);
                            buttonQuit.SetActive(true);
                        }
                        else
                        {
                            ShowNotification("Login Failure");
                        }
                    });
                }
                else
                {

                }
            });
        }
        catch(Exception e)
        {

        }
       
    }

    public void CekFirebase()
    {
        try
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                if (app != null)
                {
                    // Firebase telah terhubung
                    ShowNotification("telah terhubung!");
                }
                else
                {
                    // Firebase tidak terhubung
                    ShowNotification("Tidak terhubung!");
                }
            });
        }
        catch(Exception e)
        {
            ShowNotification("error : " + e.ToString());
        }
    }

    void ConnectToFirebase()
    {
        try
        {
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
            {
                authCode = code;
                Firebase.Auth.FirebaseAuth FBAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                Firebase.Auth.Credential FBCred = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
                FBAuth.SignInWithCredentialAsync(FBCred).ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        firebasetext.text = "Sign In Canceled";
                    }

                    if (task.IsFaulted)
                    {
                        firebasetext.text = "Error : " + task.Result;
                    }

                    Firebase.Auth.FirebaseUser user = FBAuth.CurrentUser;
                    if (user != null)
                    {
                        ShowNotification("Sign in as : " + user.UserId);
                        UserStatus.instance.userDisplayName = user.DisplayName;
                    }
                    else
                    {
                        ShowNotification("new user");
                    }

                    
                });
            });
        }catch (Exception e)
        {
            ShowNotification("error : " + e.ToString());
        }
    }

    public void SaveData()
    {
        try
        {
            SaveData saveData = new();
            firestore.Document($"save_data/0").SetAsync(saveData);
        }
        catch (Exception e)
        {

        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(GamePlayscene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }


    public void ShowNotification(string message)
    {
        textNotification.text = message;
    }

    bool IsInternetAvailable()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public void RestartScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

}
