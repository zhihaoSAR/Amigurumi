using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class empujable : MonoBehaviour , Interactuable
{
    Player player;
    public float speed = 2.5f;
    Rigidbody myBody;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        myBody = GetComponent<Rigidbody>();
    }


    public void OnInteraction(Ray ray, RaycastHit hit, int control)
    {
        
        Vector3 dir = ray.direction.normalized;
        Vector3 startPos;

        startPos = hit.point - dir;
        startPos.y = transform.position.y;

        player.empujar(myBody, null, null,speed,startPos);
    }

    bool Interactuable.interactuable(RaycastHit hit)
    {
        return true;
    }

    bool Interactuable.subible(RaycastHit hit)
    {
        return false;
    }
}
