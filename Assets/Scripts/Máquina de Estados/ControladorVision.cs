using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorVision : MonoBehaviour
{

    public Transform DetectorBebe;
    public float rangoVision = 20f;
    public Vector3 offset = new Vector3(0f, 0.75f, 0f);

    private ControladorNavMesh controladorNavMesh;

    private void Awake()
    {
        controladorNavMesh = GetComponent<ControladorNavMesh>();
    }

    public bool PuedeVerAlJugador(out RaycastHit hit, bool mirarHaciaElJugador = false)
    {
        Vector3 vectorDireccion;
        if (mirarHaciaElJugador)
        {
            vectorDireccion = (controladorNavMesh.perseguirObjetivo.position + offset) - DetectorBebe.position;
        }
        else
        {
            vectorDireccion = DetectorBebe.forward;
        }
        return Physics.Raycast(DetectorBebe.position, vectorDireccion, out hit, rangoVision) && hit.collider.CompareTag("Player");
    }
}
