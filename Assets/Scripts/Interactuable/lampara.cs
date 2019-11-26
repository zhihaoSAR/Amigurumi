using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lampara : MonoBehaviour,Interactuable
{
    public Color color;
    public Light light;
    bool encendido = false;
    public MeshRenderer renderer;
    Material mat;
    MainControl control;

    void Start()
    {
        mat =renderer.GetComponent<Renderer>().material;
        control = GameObject.Find("MainControl").GetComponent<MainControl>();
    }
    IEnumerator encenderLuz(float secondos)
    {
        float time = 0;
        while(time < secondos)
        {
            Color finalColor = color * Mathf.LinearToGammaSpace(light.intensity);
            mat.SetColor("_EmissionColor", finalColor);
            time += Time.deltaTime;
            yield return null;
        }
        control.luzEncendida();
        
        
    }

    public void OnInteraction(Ray ray, RaycastHit hit, int control)
    {
        GetComponent<Animator>().SetBool("LuzEncendida", true);
        light.GetComponent<Animator>().SetBool("EncenderLampara", true);
        encendido = true;
        StartCoroutine("encenderLuz", 5);
    }

    public bool subible(RaycastHit hit)
    {
        return false;
    }

    public bool interactuable(RaycastHit hit)
    {
        return !encendido;
    }
}
