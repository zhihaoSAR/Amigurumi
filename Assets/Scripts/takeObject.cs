using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeObject : MonoBehaviour
{
    public GameObject player;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDrag()
    {
        transform.position = player.transform.position+ 
                            Quaternion.Euler(0, player.transform.eulerAngles.y, 0) * (new Vector3(0,-0.5f,distance));
    }
}
