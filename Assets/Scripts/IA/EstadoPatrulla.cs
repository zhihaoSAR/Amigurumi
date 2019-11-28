using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EstadoPatrulla : MonoBehaviour
{
    public Transform[] WayPoints;

    private ControladorNavMesh controladorNavMesh;
    private int siguienteWayPoint;
    private MaquinaDeEstados maquinaDeEstados;
    private ControladorVision controladorVision;
    private NavMeshAgent agent;
    public AudioClip clip;
    private AudioSource source;

    void Awake()
    {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<ControladorNavMesh>();
        controladorVision = GetComponent<ControladorVision>();
        source = GetComponent<AudioSource>();
    }


    void Update()
    {
        //Ve al jugador??
        RaycastHit hit;
        if(controladorVision.PuedeVerAlJugador(out hit))
        {
            source.clip = clip;
            source.Play();

            controladorNavMesh.perseguirObjetivo = hit.transform;
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPersecucion);
            return;
        }

        if (controladorNavMesh.HemosLlegado())
        {
            siguienteWayPoint = (siguienteWayPoint + 1) % WayPoints.Length;
            ActualizarWayPointDestino();

        }
    }

    void OnEnable()
    {
        controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent(WayPoints[siguienteWayPoint].position);
    }

    void ActualizarWayPointDestino()
    {
        controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent(WayPoints[siguienteWayPoint].position);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && enabled)
        {
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoAlerta);
        }
    }
}
