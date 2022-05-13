using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Firebase;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseMananger : MonoBehaviour {
    // public InputField message;
    private string userID;

    public string name = "default name";
    public string petitionMessage = "default petition message";
    string cultureName = "en-US";
    long epochTime;
    private DatabaseReference dbReference;

    void Start () {
        userID = SystemInfo.deviceUniqueIdentifier;
        // Get the root reference location of the database.
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void TimeStamp () {
        DateTime utcDate = DateTime.UtcNow;
        var culture = new CultureInfo ("en-US");
        epochTime = DateTimeOffset.Now.ToUnixTimeSeconds ();
        // Debug.Log ("epoc: ", epochTime);
        Debug.Log (DateTimeOffset.Now.ToUnixTimeSeconds ());
        // Debug.Log("{0}, {1:G}");
        // Debug.Log(utcDate.ToString (culture));
        // Debug.Log(utcDate.Kind);
        // Console.WriteLine ("{0}, {1:G}", utcDate.ToString (culture), utcDate.Kind);
    }

    public void CreateUser () {
        TimeStamp ();
        DateTime utcDate = DateTime.UtcNow;
        var culture = new CultureInfo ("en-US");

        User newUser = new User (name, petitionMessage, epochTime);
        string json = JsonUtility.ToJson (newUser);
        dbReference.Child (userID).SetRawJsonValueAsync (json);

        // dbReference.SetRawJsonValueAsync (json);
        // dbReference.Child ("users").Child (userID).SetRawJsonValueAsync (json);
    }
}