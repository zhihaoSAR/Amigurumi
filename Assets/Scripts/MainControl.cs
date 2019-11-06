using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MainControl : MonoBehaviour
{
    Dictionary<string, string> button = new Dictionary<string, string>();
    public Text text_Interacturar;
    void Awake()
    {
        var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
        SerializedObject obj = new SerializedObject(inputManager);
        SerializedProperty axisArray = obj.FindProperty("m_Axes");
        if (axisArray.arraySize == 0)
            Debug.Log("No Axes");

        for (int i = 0; i < 5; ++i)
        {
            var axis = axisArray.GetArrayElementAtIndex(i);
            var name = axis.displayName;      //axis.displayName  "Horizontal"  string
            axis.Next(true);   //axis.displayName      "Name"  string
            axis.Next(false);      //axis.displayName  "Descriptive Name"  string
            axis.Next(false);      //axis.displayName  "Descriptive Negative Name" string
            axis.Next(false);      //axis.displayName  "Negative Button"   string
            axis.Next(false);      //axis.displayName  "Positive Button"   string
            var value = axis.stringValue;  //"right"

            if(i > 1)
            {
                button.Add(name, value);
            }
        }

    }
    void Start()
    {
        text_Interacturar.text = button["interactuar"].ToUpper() + " Interactuar";

    }

}
