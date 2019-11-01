using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class takeObject : MonoBehaviour
{

    enum Mode {PLAY,PAUSE};
    Mode State;



    

    // Start is called before the first frame update
    void Start()
    {
        State = Mode.PLAY;
    }

    void FixedUpdate()
    {
        
    }


}
