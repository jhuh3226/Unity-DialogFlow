using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseMananger : MonoBehaviour {
    public InputField message;
    private string userID;
    private DatabaseReference dbReference;

    void Start () {
        userID = SystemInfo.deviceUniqueIdentifier;
        // Get the root reference location of the database.
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateUser () {
        User newUser = new User (message.text);
        string json = JsonUtility.ToJson (newUser);
        dbReference.SetRawJsonValueAsync (json);

        // dbReference.Child ("users").Child (userID).SetRawJsonValueAsync (json);
    }
}
