using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeObject : MonoBehaviour
{

    enum Mode {PLAY,PAUSE};
    Mode State;



    Ray ray;
    RaycastHit hit;
    GameObject obj;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        State = Mode.PLAY;
    }

    // Update is called once per frame
    void Update()
    {
        if (State == Mode.PLAY)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit,distance))
            {
                obj = hit.collider.gameObject;

                if (obj.tag == "KeyObject")
                {
                    if(Input.GetButton("coger"))
                    {
                        Destroy(obj);
                    }
                }
            }
        }
    }


}
