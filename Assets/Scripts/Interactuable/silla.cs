using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class silla : MonoBehaviour, Interactuable
{
    Player player;
    public Transform endPos,altura;//altura es la altura que permite a subir

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public bool interactuable(RaycastHit hit)
    {
        return false;
    }

    public void OnInteraction(Ray ray, RaycastHit hit, int control)
    {
        Vector3 end = ray.direction.normalized;
        end.y = endPos.position.y;
        Vector3 start = hit.point - ray.direction.normalized;
        start.y = endPos.position.y;
        player.saltar( start, end);
    }

    public bool subible(RaycastHit hit)
    {
        return player.myPos.position.y > altura.position.y;
    }
}
