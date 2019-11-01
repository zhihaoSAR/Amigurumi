using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class empujable : MonoBehaviour , Interactuable
{
    public bool subible = false,interactuable = true;
    Player player;
    public Transform limitMin,limitMax;
    public float speed = 2.5f;
    Rigidbody myBody;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        myBody = GetComponent<Rigidbody>();
    }


    public void OnInteraction()
    {
        player.empujar(myBody, limitMin, limitMax,speed);
    }

    bool Interactuable.interactuable()
    {
        return interactuable;
    }

    bool Interactuable.subible()
    {
        return subible;
    }
}
