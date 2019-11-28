using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainControl : MonoBehaviour
{
    Dictionary<string, string> button = new Dictionary<string, string>();
    public Text text_Interacturar,text_Subir;
    public Slider barra_cordura , barra_energia;
    Image corduraFondo,energiaFondo;
    static bool bajaCordura = false,bajaEnergia = false;
    static Sprite HighCordura,LowCordura,HighEnergia,LowEnergia;
    public LevelChangerScript levelChanger;
    private ControladorNavMesh controladorNavMesh;
    public GameObject polilla;



    void Awake()
    {
        controladorNavMesh = GetComponent<ControladorNavMesh>();
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

        HighCordura = Resources.Load<Sprite>("fondo_cordura_high");
        LowCordura = Resources.Load<Sprite>("fondo_cordura_low");
        HighEnergia = Resources.Load<Sprite>("fondo_energia_high");
        LowEnergia = Resources.Load<Sprite>("fondo_energia_low");
    }
    void Start()
    {
        text_Interacturar.text = button["interactuar"].ToUpper() + " Interactuar";
        text_Subir.text = button["subir"].ToUpper() + " Subir";
        corduraFondo = barra_cordura.transform.GetChild(0).GetComponent<Image>();
        energiaFondo = barra_energia.transform.GetChild(0).GetComponent<Image>();
        
    }

    public void setInteractuarVisible(bool visible)
    {
        text_Interacturar.enabled = visible;
    }
    public void setSubirVisible(bool visible)
    {
        text_Subir.enabled = visible;
    }

    public void UpdateCordura(float value)
    {
        if(value > 0.3 && bajaCordura)
        {
            corduraFondo.sprite = HighCordura;
            bajaCordura = false;
        }
        else if(value <= 0.3 && !bajaCordura)
        {
            corduraFondo.sprite = LowCordura;
            bajaCordura = true;
        }
        float v = value * 0.9f;
        barra_cordura.value = v; 
    }

    public void UpdateEnergia(float value)
    {
        if (value > 0.3 && bajaEnergia)
        {
            energiaFondo.sprite = HighEnergia;
            bajaEnergia = false;
        }
        else if (value <= 0.3 && !bajaEnergia)
        {
            energiaFondo.sprite = LowEnergia;
            bajaEnergia = true;
        }
        float v = value * 0.9f;
        barra_energia.value = v;
    }


    public void luzEncendida()
    {
        controladorNavMesh.DeteberNavMeshAgent();
        polilla.SetActive(true);
    }

    public void GameOver()
    {
        levelChanger.FadeToLevel(2);    
    }

}
