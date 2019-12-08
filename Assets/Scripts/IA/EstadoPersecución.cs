using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EstadoPersecución : MonoBehaviour
{
    
    private MaquinaDeEstados maquinaDeEstados;
    private NavMeshAgent agent;
    private ControladorNavMesh controladorNavMesh;
    private ControladorVision controladorVision;
    //public AudioClip musicaPersecucionClip;
    private AudioSource musicaPersecucion;


    void Awake()
    {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<ControladorNavMesh>();
        controladorVision = GetComponent<ControladorVision>();
        musicaPersecucion = GetComponent<AudioSource>();
        
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        RaycastHit hit;
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
}
