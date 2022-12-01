using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBasedCheck : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Wallbased Collision detected");
    }

}
