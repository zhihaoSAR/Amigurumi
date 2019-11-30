using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    Player player;
    Ray ray;
    RaycastHit hit;
    GameObject obj;
    public bool realizaDano = true;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }


    void OnWillRenderObject()
    {
        if (Physics.Raycast(transform.position,player.myPos.position- transform.position
                            , out hit))
        {
            Debug.DrawLine(transform.position, (player.myPos.position - transform.position) * hit.distance, Color.yellow);
            obj = hit.collider.gameObject;
            if (obj.tag == "Player")
            {
                player.recibirDano(transform.position);
            }
        }


    }
}
