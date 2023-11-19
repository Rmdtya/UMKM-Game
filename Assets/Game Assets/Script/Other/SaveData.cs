using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

[FirestoreData]
public class SaveData
{
    private string userName = "Firdaus";
    private int health = 200;
    private float dificultModifier = 2.3f;

    [FirestoreProperty]
    public string Username
    {
        get => userName;
        set => userName = value;
    }

    [FirestoreProperty]
    public int Health
    {
        get => health;
        set => health = value;
    }

    [FirestoreProperty]
    public float DificultModifier
    {
        get => dificultModifier;
        set => dificultModifier = value;
    }

}
