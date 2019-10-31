using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Estado state = Estado.MOVE;

    //player
    public float cordura,energia;
    public Slider barra_cordura; // slider del ui para mostrar la cantidad de cordura
    private bool recibiendoDano = false;
    public Camera camera;

    // mover
    public float speed, runSpeed;
    private CharacterController controller;
    public bool controllable = true;
    string lastAnimation = "parado";
    public Animator animator;

    //moverCamara
    public float speedH, speedV, //variable que controla la sensibilidad
                    limitUp, limitDown; // para controlar limite de la rotacion x 
    private float yaw = 0, pitch = 0;



    //valor entre 0-1
    //si el amigurumi esta dentro de (wMin,hMin) (wMax,hMax) recibe dano
    public float hMin, hMax, wMin, wMax;



    void Start()
    {
        controller = GetComponent<CharacterController>();
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
            if(state.Equals(Estado.MOVE))
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                //controlar la gravedad
                float moveY = 0, m_gravity = 10f;
                moveY -= m_gravity * Time.deltaTime;

                Vector3 movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) *
                                new Vector3(horizontal, moveY, vertical);
                //si hay movimiento
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

        //recupera corduro cuando no ve amigurumi
        if (!recibiendoDano)
        {
            cordura = cordura >= 995 ? 1000 : cordura+5;
            barra_cordura.value = cordura;
        }
        recibiendoDano = false;
    }

    public void recibirDano(Vector3 ePos)
    {
        Vector3 pos = camera.WorldToViewportPoint(ePos);
        if (pos.x < wMax && pos.x > wMin && pos.y > hMin && pos.y < hMax && pos.z >= 0)
        {
            cordura -= 150* Time.deltaTime;
            barra_cordura.value = cordura;
            recibiendoDano = true;
            if (cordura <= 0)
            {
                Debug.Log("GAME OVER");
            }
        }

    }

    //funcion para controlar la animacion
    public void setAnimation(string animation)
    {
        if (lastAnimation != "parado")
        {
            animator.SetBool(lastAnimation, false);
        }
        if (animation != "parado")
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
