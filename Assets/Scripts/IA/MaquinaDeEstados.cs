using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MaquinaDeEstados : MonoBehaviour
{
    public MonoBehaviour EstadoPatrulla;
    public MonoBehaviour EstadoAlerta;
    public MonoBehaviour EstadoPersecucion;
    public MonoBehaviour EstadoInicial;

    private MonoBehaviour estadoActual;

    void Start()
    {
        ActivarEstado(EstadoInicial);
    }

    public void ActivarEstado(MonoBehaviour nuevoEstado)
    {
        if (estadoActual != null)
        {
            estadoActual.enabled = false;
        }
        estadoActual = nuevoEstado;
        estadoActual.enabled = true;
    }

    public void morir()
    {
        estadoActual.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<NavMeshAgent>().enabled = false;
        enabled = false;
    }

}
