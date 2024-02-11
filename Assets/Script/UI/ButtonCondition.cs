using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCondition : MonoBehaviour
{
    public UnityEngine.UI.Button testButton;
    
    private void Start()
    {
        string path = Application.persistentDataPath;
        if (AssetDatabase.IsValidFolder(path))//if path exists
        {
            if (Directory.GetFiles(path + "/", "*.sav").Length > 0)//are there any save files?
            {
                //save files found
                testButton.interactable = true;
            }
            else//no save files
            {
                testButton.interactable = false;
            }
        }
        else//path does not exist
        {
            testButton.interactable = false;
        }
        
    }
}
