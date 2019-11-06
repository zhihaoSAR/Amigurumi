using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Player player;
    Ray ray;
    RaycastHit hit;
    GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnWillRenderObject()
    {
        if (Physics.Raycast(transform.position,player.transform.GetChild(0).GetChild(1).position- transform.position
                            , out hit))
        {
            obj = hit.collider.gameObject;
            Debug.Log(obj);
            if (obj.tag == "Player")
            {
                
                player.recibirDano(transform.position);
            }
        }

        
    }
}
