using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class puerta : MonoBehaviour,Interactuable
{
    bool abierto = false;
    LevelChangerScript Levelchanger;
    AudioSource sonido;

    public bool interactuable(RaycastHit hit)
    {
        return MainControl.levelComplete && !abierto;
    }

    public void OnInteraction(Ray ray, RaycastHit hit, int control)
    {
        sonido.Play();
        Levelchanger.FadeToLevel(6);
    }

    public bool subible(RaycastHit hit)
    {
        return false;
    }

    public void Start()
    {
        Levelchanger = GameObject.Find("LevelChanged").GetComponent<LevelChangerScript>();
        sonido = GetComponent<AudioSource>();
    }
}
