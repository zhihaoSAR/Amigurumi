using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactuable 
{
    void OnInteraction(Ray ray, RaycastHit hit,int control);
    bool subible(RaycastHit hit);
    bool interactuable(RaycastHit hit);
}
