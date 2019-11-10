using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    //return 1 si ha superado limitMax
    //2 si ha superado limitMin
    //0 esta dentro
    public static int isGreaterOrEqual(Vector3 limitMax,Vector3 limitMin,Vector3 v)
    {
        float longitud = (limitMax - limitMin).magnitude;
        if((limitMin - v).magnitude > longitud)
        {
            return 1;
        }else if((limitMax - v).magnitude > longitud)
        {
            return 2;
        }
        return 0;
                
                
    }
}
