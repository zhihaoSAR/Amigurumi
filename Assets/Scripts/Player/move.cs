using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    // mover
    public float speed,runSpeed;
    private CharacterController controller;
    public bool controllable = true;
    string lastAnimation="parado";
    public Animator animator;

    //moverCamara
    public float speedH, speedV, limitUp, limitDown;
    private float yaw = 0, pitch = 0;
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void FixedUpdate()
    {

        //controlar la rotacion de la camara
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        if (pitch < limitDown)
        {
            pitch = limitDown;
        }
        else if (pitch > limitUp)
        {
            pitch = limitUp;
        }

        camera.transform.eulerAngles = new Vector3(pitch, yaw, 0);

        if (controllable)
        {
            //controlar
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            float moveY = 0, m_gravity = 10f;
            moveY -= m_gravity * Time.deltaTime;

            Vector3 movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) *
                            new Vector3(horizontal, moveY, vertical);
            if (!Mathf.Approximately(horizontal, 0) || !Mathf.Approximately(vertical, 0))
            {
                if (Input.GetButton("correr"))
                {
                    setAnimation("caminar");
                    movement *= runSpeed;
                }
                else
                {
                    setAnimation("gatear");
                }

            }
            else
            {
                setAnimation("parado");
            }
            //rotacion del player
            transform.eulerAngles = new Vector3(0, yaw, 0);
            //posicion del player
            controller.Move(movement * speed * Time.deltaTime);
        }
        

    }

    //funcion para controlar la animacion
    public void setAnimation(string animation)
    {
        if(lastAnimation != "parado")
        {
            animator.SetBool(lastAnimation, false);
        }
        if(animation != "parado")
        {
            animator.SetBool(animation, true);
            lastAnimation = animation;
        }
        
    }

    public void OnVerticalChanged(float v)
    {
        speedV = v;
    }
    public void OnHorizontalChanged(float v)
    {
        speedH = v;
    }

}
