using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polilla : MonoBehaviour
{
    public bool polillaLuz = false;
    private GameObject lampara, amigurumi;
    private Vector3 diff;
    private float t;
    private float vel;
    private float secs = 5;
    // Start is called before the first frame update
    void Start()
    {
        //transform.localScale = new Vector3(0, 0, 0);
        lampara = GameObject.Find("lampara_fina");
        amigurumi = GameObject.Find("Enemigo_NV1");
        transform.position = amigurumi.transform.position;
        diff = lampara.transform.position - amigurumi.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(polillaLuz)
        {
            if (t <= secs)
            {
                t += Time.deltaTime;
                vel = t / 10;
                transform.position = amigurumi.transform.position + diff * vel;
            }
        }
    }
}
