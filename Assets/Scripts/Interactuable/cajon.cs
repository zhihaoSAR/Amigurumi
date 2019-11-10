using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cajon : MonoBehaviour,Interactuable
{
    public bool subible = false, interactuable = true;
    Player player;
    public Transform limitMin, limitMax;
    public float speed = 2.5f;
    Rigidbody myBody;
    BoxCollider myCollider;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        myBody = GetComponent<Rigidbody>();
        myCollider = GetComponent<BoxCollider>();
    }


    public void OnInteraction()
    {
        player.empujar(myBody, limitMin, limitMax, speed);
    }

    bool Interactuable.interactuable(RaycastHit hit)
    {
        float producto = Vector3.Dot(hit.normal, Quaternion.Euler(0, transform.eulerAngles.y, 0) * transform.right);

        return producto == 0;
    }

    bool Interactuable.subible()
    {
        return subible;
    }
}
