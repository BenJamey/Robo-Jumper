using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    RobotAnimations CameraControls;
    InputAction look;
    [SerializeField] float CameraSpeedPitch;
    [SerializeField] float CameraSpeedYaw;
    [SerializeField] float CameraSpeedMin;
    [SerializeField] float CameraSpeedMax;
    float Yaw, Pitch;
    Transform Player;
    Vector3 CameraDir;

    private void Awake() {
        CameraControls = new RobotAnimations();
    }

    private void OnEnable() {
        look = CameraControls.Player.Look;
        look.Enable();
    }

    private void OnDisable() {
        look.Disable();
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Player = FindFirstObjectByType<CharacterMovement>().transform;
    }

    
    void Update() {
        CameraDir = new Vector3(look.ReadValue<Vector2>().y, look.ReadValue<Vector2>().x, 0).normalized;
        Pitch += CameraDir.x * Time.deltaTime * CameraSpeedPitch;
        Yaw += CameraDir.y * Time.deltaTime * CameraSpeedYaw;
        Pitch = Mathf.Clamp(Pitch, CameraSpeedMin, CameraSpeedMax);
        transform.position = Player.transform.position + new Vector3(0, 0.69f, 0);
        transform.rotation = Quaternion.Euler(Pitch, Yaw, 0);
    }
}
