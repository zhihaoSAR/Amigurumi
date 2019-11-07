using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Player player;
    Ray ray;
    RaycastHit hit;
    GameObject obj;
    public GameObject Target;
    public NavMeshAgent agent;

    public float distance;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Target.transform.position, transform.position) < distance)
        {
            agent.SetDestination(Target.transform.position);
            agent.speed = 3;
        }
        else
        {
            agent.speed = 0;
        }
    }

    void OnWillRenderObject()
    {
        if (Physics.Raycast(transform.position,player.myPos.position- transform.position
                            , out hit))
        {
            obj = hit.collider.gameObject;
            if (obj.tag == "Player")
            {

                player.recibirDano(transform.position);
            }
        }


    }
}
