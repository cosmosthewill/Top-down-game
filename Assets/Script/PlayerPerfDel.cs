using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerfDel : MonoBehaviour
{
    void Start()
    {
        #if !UNITY_EDITOR && UNITY_STANDALONE_WIN  // Only runs in Windows build, not in editor
        if(!PlayerPrefs.HasKey("FirstRun"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("FirstRun", 1);
            PlayerPrefs.Save();
        }
        #endif
    }
}
