using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class silla : MonoBehaviour , Interactuable
{
    public bool subible = false,interactuable = true;
    Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }


    public void OnInteraction()
    {
        
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
