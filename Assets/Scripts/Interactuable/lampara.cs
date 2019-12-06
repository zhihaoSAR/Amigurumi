using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class lampara : MonoBehaviour,Interactuable
{
    public Color color;
    public Light light;
    bool encendido = false;
    public MeshRenderer renderer;
    Material mat;
    MainControl control;
    public Polilla polilla;
    PlayableDirector encenderLamp;

    void Start()
    {
        mat =renderer.GetComponent<Renderer>().material;
        control = GameObject.Find("MainControl").GetComponent<MainControl>();
        encenderLamp = GetComponent<PlayableDirector>();
    }
    IEnumerator encenderLuz(float secondos)
    {
        float time = 0;
        while (time < secondos)
        {
            Color finalColor = color * Mathf.LinearToGammaSpace(light.intensity);
            mat.SetColor("_EmissionColor", finalColor);
            time += Time.deltaTime;
            yield return null;
        }
        
        
        
    }

    public void OnInteraction(Ray ray, RaycastHit hit, int control)
    {
        GetComponent<Animator>().SetBool("LuzEncendida", true);
        light.GetComponent<Animator>().SetBool("EncenderLampara", true);
        encendido = true;
        vuelaPolilla();
        encenderLamp.Play();
        this.control.luzEncendida();
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

    void vuelaPolilla()
    {
        polilla.activarPolilla();

    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        control.acabadoAnimacion();
    }
}
