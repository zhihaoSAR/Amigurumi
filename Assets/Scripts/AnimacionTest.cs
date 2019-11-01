using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionTest : MonoBehaviour
{
    public moveCamera maincamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnAnimatorMove()
    {
        Animator animator = GetComponent<Animator>();
        //maincamera.moveCameraWithBaby(animator.bodyPosition);
        Debug.Log(animator.rootPosition);
    }
}
