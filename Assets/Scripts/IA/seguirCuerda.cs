using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seguirCuerda : MonoBehaviour
{
GameObject oso;
 LineRenderer lineRenderer;
private float secondsCounter=0;
   private float secondsToCount=1;
private int c = 10;
    // Start is called before the first frame update
    void Start()
    {
         lineRenderer = this.GetComponent<LineRenderer>();
oso = GameObject.Find("Enemigo_NV1");

    }

    // Update is called once per frame
    void Update()
    {
	secondsCounter += Time.deltaTime;
      if (secondsCounter >= secondsToCount)
      {
         secondsCounter=0;
         lineRenderer.positionCount = c;
        lineRenderer.SetPosition((c++)-1, oso.transform.position);
      }
	
    }
}
