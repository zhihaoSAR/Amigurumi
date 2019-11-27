using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoPersecución : MonoBehaviour
{
    
    private MaquinaDeEstados maquinaDeEstados;
    private ControladorNavMesh controladorNavMesh;
    private ControladorVision controladorVision;
    private AudioClip musicaPersecucionClip;
    public AudioSource musicaPersecucion;

    
    void Awake()
    {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<ControladorNavMesh>();
        controladorVision = GetComponent<ControladorVision>();
        //musicaPersecucion = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        
    }

    void Update()
    {
        musicaPersecucion.clip = musicaPersecucionClip;
        musicaPersecucion.Play();
        RaycastHit hit;
        if(!controladorVision.PuedeVerAlJugador(out hit, true))
        {
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoAlerta);
            return;
        }
        controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent();
    }
}
