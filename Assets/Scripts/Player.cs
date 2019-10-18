using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float cordura,energia;
    public Slider barra_cordura;
    private bool recibiendoDano = false;
    public GameObject enemies;
    //valor entre 0-1
    public float hMin, hMax, wMin, wMax;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void FixedUpdate()
    {
        

        foreach (Transform e in enemies.transform)
        {
            Vector3 pos = camera.WorldToViewportPoint(e.position);
            if (pos.x < wMax && pos.x > wMin && pos.y > hMin && pos.y < hMax && pos.z >=0)
            {
                recibirDano();
            }

        }
        if (!recibiendoDano)
        {
            cordura = cordura >= 995 ? 1000 : cordura+5;
            barra_cordura.value = cordura;
        }
        recibiendoDano = false;
    }

    void recibirDano()
    {
        //Debug.Log("mi cordura: " + cordura);
        cordura -= 5;
        barra_cordura.value = cordura;
        recibiendoDano = true;
        if(cordura <= 0)
        {
            Debug.Log("GAME OVER");
        }
    }
}
