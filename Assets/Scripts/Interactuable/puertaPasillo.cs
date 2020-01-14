using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class puertaPasillo : MonoBehaviour, Interactuable
{
    bool abierto = false;
    LevelChangerScript Levelchanger;
    AudioSource sonido;

    public bool interactuable(RaycastHit hit)
    {
        return !abierto;
    }

    public void OnInteraction(Ray ray, RaycastHit hit, int control)
    {
        sonido.Play();
        Levelchanger.FadeToLevel(5);
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
