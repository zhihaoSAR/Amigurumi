using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoPersecución : MonoBehaviour
{
    
    private MaquinaDeEstados maquinaDeEstados;
    private ControladorNavMesh controladorNavMesh;
    private ControladorVision controladorVision;
    
    void Awake()
    {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<ControladorNavMesh>();
        controladorVision = GetComponent<ControladorVision>();
    }

    private void OnEnable()
    {
        
    }

    void Update()
    {
        RaycastHit hit;
        if(!controladorVision.PuedeVerAlJugador(out hit, true))
        {
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoAlerta);
            return;
        }
        controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent();
    }
}
