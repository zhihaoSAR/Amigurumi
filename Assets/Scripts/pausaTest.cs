using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pausaTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.timeScale != 0)
            {
                Debug.Log("pausa");
            }
            
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            Time.timeScale = 1;
        }
    }
}
