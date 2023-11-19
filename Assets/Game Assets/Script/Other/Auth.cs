using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;
using System;

public class Auth : MonoBehaviour
{

    public TextMeshProUGUI textlogin;
    public TextMeshProUGUI textatas;


    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.Instance.Authenticate((status) =>
        {
            if (status == SignInStatus.Success)
            {
                try
                {
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                    {
                        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

                        Credential credential = PlayGamesAuthProvider.GetCredential(code);

                        StartCoroutine(AuthGet());

                        IEnumerator AuthGet()
                        {
                            System.Threading.Tasks.Task<FirebaseUser> task = auth.SignInWithCredentialAsync(credential);

                            while (!task.IsCompleted) yield return null;

                            if (task.IsCanceled)
                            {
                                textatas.text = "Auth Canceled";
                            }
                            else if (task.IsFaulted)
                            {
                                textatas.text = "Faulted " + task.Exception;
                            }
                            else
                            {
                                FirebaseUser newUser = task.Result;
                                textatas.text = "Fully auth user";
                            }
                        }
                    });
                }
                catch (Exception e)
                {
                    textatas.text = "error : " + e.ToString();
                }
            }
            else
            {
                textatas.text = "PGP AUTH FAIL";
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
