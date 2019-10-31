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

    void FixedUpdate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, distance))
        {
            obj = hit.collider.gameObject;

            if (obj.CompareTag("KeyObject"))
            {
                if (Input.GetButtonDown("interactuar"))
                {
                    Interactuable func = obj.GetComponent<Interactuable>();
                    if (func.interactuable())
                    {

                    }
                    Destroy(obj);
                }
            }
            if (obj.CompareTag("Pushble"))
            {
                if (Input.GetButton("interactuar"))
                {
                    Interactuable func = obj.GetComponent<Interactuable>();
                    func.OnInteraction();
                }
            }
        }
    }


}
