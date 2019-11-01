﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class push : MonoBehaviour
{
    public float power;
    private CharacterController contro;
    // Start is called before the first frame update
    void Start()
    {
        contro = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }
        if(hit.gameObject.CompareTag("Pushable"))
        {
            Vector3 powerDir = hit.moveDirection.normalized;
            body.MovePosition(body.transform.position + powerDir * Time.deltaTime);
        }
        


    }
}
