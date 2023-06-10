using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    // Update is called once per frame
    void Update()
    {
       // allows camera to allways move with the player
        transform.position = cameraPosition.position;
    }
}
