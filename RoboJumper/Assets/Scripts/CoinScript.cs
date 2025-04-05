using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] float RotationForce;
    Vector3 Turn = Vector3.zero;
    
    
    void Update()
    {
        //Makes the coin rotate
        Vector3 Rotate = new Vector3(0, 0, 10);
        transform.Rotate(Rotate * RotationForce * Time.deltaTime);
    }
}
