using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polilla : MonoBehaviour
{
    public bool polillaLuz = false;
    private bool empezarB = true;
    private GameObject lampara, amigurumi;
    private Vector3 diff;
    private float t, t2;
    private float vel;
    private float secs = 5;
    private float posY;
    private float diffY;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);
        lampara = GameObject.Find("lampara_fina");
        amigurumi = GameObject.Find("Enemigo_NV1");
        diff = lampara.transform.position - amigurumi.transform.position;
        posY = lampara.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(polillaLuz)
        {
            if (empezarB)
            {
                empezar();
                empezarB = false;
            }
            if (t <= secs)
            {
                t += Time.deltaTime;
                vel = t / secs;
                float y = amigurumi.transform.position.y + diff.y * vel;
                //transform.position = amigurumi.transform.position + diff * vel;
                if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(lampara.transform.position.x, 0, lampara.transform.position.z)) < 38)
                {
                    t2 += Time.deltaTime;
                    y = posY + diffY * (t2 / 1);
                } else
                {
                    posY = transform.position.y;
                    diffY = lampara.transform.position.y - transform.position.y;
                }

                if (y >= lampara.transform.position.y) y = lampara.transform.position.y;
                transform.position = new Vector3(
                    amigurumi.transform.position.x + diff.x * vel,
                    y,
                    amigurumi.transform.position.z + diff.z * vel
                );
            }
        }
    }

    private void empezar()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = amigurumi.transform.position;
    }
}