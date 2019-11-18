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
    Animator cameraAnimator;
    public Transform myPos;

    // mover
    public float speed, runSpeed;
    float horizontal, vertical, m_gravity = 50f;
    private CharacterController controller;
    public bool controllable = true;
    string lastAnimation = null,lastNivel = "parado";
    public Animator animator;
    bool enPie = false;

    //moverCamara
    public float speedH, speedV, //variable que controla la sensibilidad
                    limitUp, limitDown; // para controlar limite de la rotacion x 
    private float rotationy = 0, rotationx = 0;

    //controlar interaccion
    Ray ray;
    RaycastHit hit;
    GameObject obj;
    public float distance;
    Interactuable func;
    public Text text_Interactuar,text_Subir;
    Vector3 touchPos;

    //variable para controlar empujar
    bool limited;
    Vector3 rail,limitMin,limitMax; 
    float pushSpeed;
    Rigidbody pushObj;

    //variable global para controlar animacion
    float time,seconds;
    bool isAnimating = false;

    //valor entre 0-1
    //si el amigurumi esta dentro de (wMin,hMin) (wMax,hMax) recibe dano
    public float hMin, hMax, wMin, wMax;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraAnimator = camera.transform.parent.GetComponent<Animator>();
    }

    void Update()
    {
        //obtener que ha visto el jugador
        ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, distance))
        {
            obj = hit.collider.gameObject;

            if (obj.CompareTag("KeyObject"))
            {
                if (func == null)
                {
                    func = obj.GetComponent<Interactuable>();
                }

                if (func.interactuable(hit))
                {
                    if (Input.GetButtonDown("interactuar"))
                    {

                    }
                }
                Destroy(obj);

            }
            else if (obj.CompareTag("Interactable"))
            {
                if (func == null)
                {
                    func = obj.GetComponent<Interactuable>();
                }

                if (func.interactuable(hit))
                {
                    text_Interactuar.enabled = true;
                    if (Input.GetButtonDown("interactuar") && state.Equals(Estado.MOVE) && controllable)
                    {
                        //calcular donde empieza a empujar

                        
                        func.OnInteraction(ray,hit,0);
                    }
                }
                else
                {
                    text_Interactuar.enabled = false;
                }
                if (func.subible(hit))
                {
                    text_Subir.enabled = true;
                    if (Input.GetButtonDown("subir") && state.Equals(Estado.MOVE) && controllable)
                    {
                        func.OnInteraction(ray, hit, 1);
                        touchPos = hit.point;
                    }
                }
                else
                {
                    text_Subir.enabled = false;
                }


            }

        }
        else
        {
            func = null;
            text_Interactuar.enabled = false;
            text_Subir.enabled = false;
        }

        //recupera corduro cuando no ve amigurumi
        if (!recibiendoDano)
        {
            cordura = cordura >= 995 ? 1000 : cordura + 5;
            barra_cordura.value = cordura;
        }
        recibiendoDano = false;
        if(enPie)
        {
            energia -= 1;
        }
        else
        {
            energia += 2;
        }

    }

    void FixedUpdate()
    {


        //controlar la rotacion de la camara
        rotationy = speedH * Input.GetAxis("Mouse X");
        rotationx = -speedV * Input.GetAxis("Mouse Y");
        float rotationxNow = camera.transform.eulerAngles.x;
        if (rotationxNow > 60) rotationxNow -= 360;
        if (rotationxNow + rotationx < limitDown)
        {
            Debug.Log(rotationxNow);
            rotationx = 0;
        }
        else if (rotationxNow + rotationx > limitUp)
        {
            Debug.Log(rotationxNow);
            rotationx = 0;
        }

        if (controllable)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            //controlar la gravedad
            float moveY = 0;
            moveY -= m_gravity * Time.deltaTime;


            //comprobar estado
            if (state.Equals(Estado.MOVE))
            {


                Vector3 movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) *
                                new Vector3(horizontal, moveY, vertical);


                if (Input.GetButton("correr") && energia >= 0)
                {
                    if (!enPie)
                    {
                        ponerEnPie(true);
                    }
                    enPie = true;
                }
                else
                {
                    if (enPie)
                    {
                        ponerEnPie(false);
                    }
                    enPie = false;

                }

                

                //si hay movimiento
                if (!Mathf.Approximately(horizontal, 0) || !Mathf.Approximately(vertical, 0))
                {
                    if (enPie)
                    {
                        setAnimation("enPie","caminar");
                        energia -= 1;
                        movement *= runSpeed;
                    }
                    else
                    {
                        setAnimation("parado","gatear");
                    }

                }
                else
                {
                    if (enPie)
                    {
                        setAnimation("enPie", null);
                    }
                    else
                    {
                        setAnimation("parado", null);
                    }
                    
                }
                //rotacion del player
                
                camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x+rotationx,
                                                            transform.eulerAngles.y, 0);
                transform.eulerAngles=new Vector3(0, transform.eulerAngles.y + rotationy, 0);

                //posicion del player
                controller.Move(movement * speed * Time.deltaTime);
            }
            else if(state.Equals(Estado.PUSH))
            {
                camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x + rotationx,
                                                            camera.transform.eulerAngles.y + rotationy, 0);
                if (Input.GetButton("interactuar") && Vector3.Distance(myPos.position,touchPos) <= 5 )
                {
                    Vector3 movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) *
                                new Vector3(0, moveY, vertical);
                    if (Mathf.Approximately(vertical, 0))
                    {
                        animator.speed = 0;
                    }
                    else if (vertical < 0)
                    {
                        setAnimation("enPie","tirar");
                        animator.speed = 1;
                    }
                    else
                    {
                        setAnimation("enPie","empujar");
                        animator.speed = 1;
                    }
                    Vector3 moveStep = movement * pushSpeed * Time.deltaTime;
                    Vector3 objetivo;
                    if (limited)
                    {

                        moveStep = Vector3.Project(moveStep, rail);
                        
                        objetivo = pushObj.position + moveStep;
                        if (Utility.isGreaterOrEqual(limitMax,limitMin,objetivo) == 1)
                        {
                            objetivo = limitMax;
                        }
                        else if (Utility.isGreaterOrEqual(limitMax,limitMin,objetivo) == 2)
                        {
                            objetivo = limitMin;
                        }

                    }
                    else
                    {
                        objetivo = pushObj.position + moveStep;
                        
                    }

                    Vector3 lastPosicion = pushObj.position;
                    pushObj.MovePosition(objetivo);
                    movement = pushObj.position - lastPosicion;
                    movement.y = -m_gravity;
                    controller.Move(movement);
                }
                else
                {
                    controllable = false;
                    //setAnimation("parado",null);
                    animator.speed = 1;
                    state = Estado.MOVE;
                    StartCoroutine(PreparePush(0.2f,transform.position));
                }
                
                
            }
            
        }

        
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
    public void setAnimation(string nivel,string animation)
    {
        if(nivel != lastNivel)
        {
            if (!lastNivel.Equals("parado"))
            {
                animator.SetBool(lastNivel, false);
                cameraAnimator.SetBool(lastNivel, false);
            }
            if (!nivel.Equals("parado"))
            {
                animator.SetBool(nivel, true);
                cameraAnimator.SetBool(nivel, true);
            }
        }
        if (animation != lastAnimation)
        {
            if(lastAnimation != null)
            {
                animator.SetBool(lastAnimation, false);
            }
            if(animation != null)
            {
                animator.SetBool(animation, true);
            }
        }
        lastNivel = nivel;
        lastAnimation = animation;

    }

    public void empujar(Rigidbody obj,Transform limitMin,Transform limitMax,float pushSpeed,Vector3 startPos)
    {
        if(limitMax)
        {
            limited = true;
            rail = limitMax.position - limitMin.position;
            this.limitMax = limitMax.position;
            this.limitMin = limitMin.position;
        }
        else
        {
            limited = false;
        }
        
        
        this.pushSpeed = pushSpeed;
        pushObj = obj;

        if (!enPie)
        {
            enPie = true;
            ponerEnPie(true);
        }
        
        state = Estado.PUSH;
        setAnimation("enPie","empujar");
        StartCoroutine(PreparePush( 0.5f, startPos));
    }
    IEnumerator PreparePush( float seconds, Vector3 pos)
    {
        while(isAnimating)
        {
            yield return new WaitForSeconds(this.seconds - time);
        }
        isAnimating = true;
        time = 0;
        this.seconds = seconds;
        controllable = false;
        Vector3 movement = Vector3.zero;
        movement = pos - transform.position;
        movement.y = -m_gravity;



        while (time < seconds)
        {
            time += Time.deltaTime;
            controller.Move(movement * (Time.deltaTime / seconds));

            yield return null;
        }
        movement = pos - transform.position;
        movement.y = -m_gravity;
        controller.Move(movement);
        controllable = true;
        isAnimating = false;
    }
    public void saltar(Rigidbody obj, Vector3 startPos,Vector3 endPos)
    {

        if (!enPie)
        {
            enPie = true;
            ponerEnPie(true);
        }

        setAnimation("enPie", null);
        StartCoroutine(subir(1f, startPos,endPos));
    }

    IEnumerator subir(float seconds, Vector3 startPos,Vector3 endPos)
    {
        while (isAnimating)
        {
            yield return null;
        }
        isAnimating = true;
        time = 0;
        this.seconds = seconds;
        controllable = false;
        Vector3 movement = Vector3.zero;
        movement = startPos - transform.position;
        movement.y -= 1f;
        animator.SetTrigger("subir");
        while (time < 0.5)
        {
            time += Time.deltaTime;
            controller.Move(movement * (Time.deltaTime / 0.5f));

            yield return null;
        }
        
        movement = Vector3.zero;
        movement = endPos;
        movement.y = 1f;
        time = 0;
        seconds -= 0.5f;
        while (time < seconds)
        {
            time += Time.deltaTime;
            controller.Move(movement * (Time.deltaTime / seconds));

            yield return null;
        }
        //movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) * movement;
        movement.y = 0;
        movement *= 0.3f;
        controller.Move(movement);
        controllable = true;
        isAnimating = false;
    }



    public void OnVerticalChanged(float value)
    {
        speedV = value;
    }
    public void OnHorizontalChanged(float value)
    {
        speedH = value;
    }

    //0: gater 1: en pie
    void setCollider(int forma)
    {
        if(forma == 1)
        {

            
            controller.height = 3.5f;
            controller.center = new Vector3(0, -3f, 0f);

        }
        else
        {
            
            controller.height = 1.7f;
            controller.center = new Vector3(0, -3.8f, 3f);


        }
    }

    //true: cambia la formar de idlegatear a idledepie
    void ponerEnPie(bool enpie)
    {

        if (enpie)
        {
            Vector3 deltaPos = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(0, 0, 1.8f);
            //controller.Move(deltaPos);
            setAnimation("enPie", null);
            StartCoroutine(ponerEnPie(0.5f, 1,deltaPos));
            
   
        }
        else
        {
            Vector3 deltaPos = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(0, 0, -1.8f);
            //controller.Move(deltaPos);
            setAnimation("parado", null);
            StartCoroutine(ponerEnPie(0.5f,0,deltaPos));

    
        }
    }

    IEnumerator ponerEnPie(float seconds,int forma,Vector3 deltaPos)
    {
        while (isAnimating)
        {
            yield return new WaitForSeconds(this.seconds - time);
        }
        isAnimating = true;
        time = 0;
        this.seconds = seconds;
        controllable = false;
        Vector3 now,offset;
        deltaPos.y = -m_gravity;
        //float deltaHeight;
        if(forma == 0)
        {
            controller.height = 1f;
            now = new Vector3(0,1.2f,0.1f);
            offset = new Vector3(0,0.4f,1.8f) - now;
        }
        else
        {
            controller.height = 2.5f;
            now = new Vector3(0, 0.4f, 1.8f);
            offset = new Vector3(0, 1.2f, 0.1f) - now;
        }

        while (time < seconds)
        {
            time += Time.deltaTime;
            float porcion = time / seconds;

            controller.Move(deltaPos * (Time.deltaTime/seconds));
            //controller.height += deltaHeight * Time.deltaTime;
            controller.center = now + offset * porcion;


            yield return null;
        }
        if (forma == 1)
        {
            controller.center = new Vector3(0, 1.2f, 0.1f);
        }
        else
        {
            controller.center = new Vector3(0, 0.4f, 1.8f);
        }

        controllable = true;
        isAnimating = false;
    }
}
