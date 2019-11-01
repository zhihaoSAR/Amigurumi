using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menupausa : MonoBehaviour
{
    public Canvas menu,ui;
    private bool pausaActivated = true;
    // Start is called before the first frame update
    void Start()
    {
        menu.enabled = false;
        ui.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!menu.enabled)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                pausa();
            }
        }
        

    }

    public void pausa()
    {
        if (pausaActivated)
        {
            menu.enabled = !menu.enabled;
            ui.enabled = !ui.enabled;
            pausaActivated = false;
            if (menu.enabled)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Time.timeScale = 0;
            }
            else
            {

                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }

            StartCoroutine("hacerAnimacion", 1);
        }
       
    }

    private IEnumerator hacerAnimacion(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        pausaActivated = true;
    }
}
