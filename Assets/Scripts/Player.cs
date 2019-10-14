using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float cordura,energia;

    public GameObject enemies;
    //valor entre 0-1
    public float hMin, hMax, wMin, wMax;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void FixedUpdate()
    {
        foreach (Transform e in enemies.transform)
        {
            Vector3 pos = camera.WorldToViewportPoint(e.position);
            if (pos.x < wMax && pos.x > wMin && pos.y > hMin && pos.y < hMax)
            {
                recibirDano();
            }

        }
    }

    void recibirDano()
    {
        Debug.Log("mi cordura: " + cordura);
        cordura -= 1;
        if(cordura <= 0)
        {
            Debug.Log("GAME OVER");
        }
    }
}
