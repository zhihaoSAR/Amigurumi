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
    //private AudioSource musicaPersecucion;
    private float distanciaMaxima;

    void Awake()
    {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<ControladorNavMesh>();
        controladorVision = GetComponent<ControladorVision>();
        //musicaPersecucion = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        distanciaMaxima = 20f;

    }

    private void OnEnable()
    {
        
    }

    void Update()
    {
        RaycastHit hit;

        //float distanciaActual = Vector3.Distance(agent.transform.position, transform.position);
        //if (distanciaActual > distanciaMaxima)
        //{
        //
        //    maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoAlerta);
        //
        //}

        if (!controladorVision.PuedeVerAlJugador(out hit, true))
        {
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoAlerta);
            return;
        }

        controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent();
    }
}
