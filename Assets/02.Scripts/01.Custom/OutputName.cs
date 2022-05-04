using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class OutputName : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textMeshPro;
    public string name;
    TextInfo textInfo; // https://docs.microsoft.com/en-us/dotnet/api/system.globalization.textinfo.totitlecase?view=net-6.0
    void Start () {
        textInfo = new CultureInfo ("en-US", false).TextInfo;

        name = "";
    }

    // get the petition data and upload the string
    void Update () {
        if (textMeshPro.text != null) OutputNameText ();
    }

    public void OutputNameText () {
        string output = name + "'s petition";
        textMeshPro.text = textInfo.ToTitleCase (output);
    }
}