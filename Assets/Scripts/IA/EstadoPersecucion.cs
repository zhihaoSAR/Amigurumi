using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EstadoPersecucion : MonoBehaviour
{
    MainControl mc;
    private MaquinaDeEstados maquinaDeEstados;
    private NavMeshAgent agent;
    private ControladorNavMesh controladorNavMesh;
    private ControladorVision controladorVision;
    private AudioSource musicaPersecucion;


    void Awake()
    {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<ControladorNavMesh>();
        controladorVision = GetComponent<ControladorVision>();
        musicaPersecucion = GetComponent<AudioSource>();
        mc = GameObject.Find("MainControl").GetComponent<MainControl>();
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        RaycastHit hit;
        agent.speed = 6f;
        musicaPersecucion.enabled = true;
        
        //musicaNormal.enabled = false;
        //musicaPersecucion.clip = musicaPersecucionClip;
        //musicaPersecucion.Play();

        if (!controladorVision.PuedeVerAlJugador(out hit, true))
        {
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoAlerta);
            musicaPersecucion.enabled = false;
            
            return;
        }

        controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent();


    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && enabled)
        {
            mc.GameOver();
        }
    }
}
