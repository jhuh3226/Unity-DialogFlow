using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Recorder;
public class AndroidUIController : MonoBehaviour
{
    public RecordManager recordManager;
    public void StartVid(){
        recordManager.StartRecord();
    }

    public void SaveVid(){
        recordManager.StopRecord();
    }
}