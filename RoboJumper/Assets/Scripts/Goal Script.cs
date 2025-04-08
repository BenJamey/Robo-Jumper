using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    [SerializeField] float RotationForce;
    Vector3 Turn = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 Rotate = new Vector3(0, 0, 10);
        transform.Rotate(Rotate * RotationForce * Time.deltaTime);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Level Complete");
    //        CharacterMovement.LevelComplete = true;
    //    }
    //}
}
