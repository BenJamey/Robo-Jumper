using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    public static RobotAnimations CharAnim;
    public static CharacterController CharacterController;
    //Base variables used for basic functions
    static int MaxHitpoints = 3;
    public static int Hitpoints = 3;
    public static int CoinsCollected;
    public static int Points;
    float CurrentVelocity = 0;
    float RunTime = 0; //Used to determine when a character has started running
    public static bool isDead = false;
    [HideInInspector] public Vector3 Direction;
    //Movement variables
    InputAction move;
    [HideInInspector] public float Speed = 0;
    [SerializeField] public float WalkSpeed;
    [SerializeField] public float RunSpeed;
    [SerializeField] float CharRotationSpeed;
    Transform PlayerCamera;

    //Variables for gravity
    float GravityForce = -9.8f;
    [SerializeField] float GravityMultiplier;

    //Jump variables
    InputAction jump;
    [SerializeField] float JumpForce;
    //[SerializeField] LayerMask GroundLayer;
    public Vector3 Velocity;

    //Variables used for getting the bonus points
    public static float ScoreBonus;
    public static float BonusMultiplier = 1.0f;
    int ConsectiveActions = 0;
    public static bool RunBonus = false;
    float BonusTimer = 0;

    InputAction pause; //Used to help pause the game
    private void Awake() {
        CharAnim = new RobotAnimations();
    }

    private void OnEnable() {
        move = CharAnim.Player.Move;
        jump = CharAnim.Player.Jump;
        move.Enable();
        jump.Enable();
    }

    private void OnDisable() {
        move.Disable();
        jump.Disable();
    }

    void Start() {
        CharacterController = GetComponent<CharacterController>();
        PlayerCamera = FindFirstObjectByType<CameraMovement>().transform;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
        ApplyMovement();
        jumpAction();
        AddBonus();

    }

    //Controlling the character movements
    private void ApplyMovement() {
        Direction = new Vector3(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y).normalized;
        if (Direction.magnitude > 0.2)
        {
            if (RunTime < 2) { //Once the playe have moved for a few seconds he'll start running
                Speed = WalkSpeed;
            }
            else {
                Speed = RunSpeed;
            }

            float TargetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + PlayerCamera.eulerAngles.y;
            float SmoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref CurrentVelocity, CharRotationSpeed);
            Vector3 MoveDir = Quaternion.Euler(0, SmoothAngle, 0) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0, SmoothAngle, 0);
            CharacterController.Move(MoveDir.normalized * Time.deltaTime * Speed);
            if (RunTime < 2)
            { //The player will gradually build up speed and start running after a couple seconds
                RunTime += 0.01f;
            }
        }
        else
        {
            RunTime = 0;
        }
    }

    private void ApplyGravity() {

        if(IsGrounded() && Velocity.y < 0.0f) {
            Velocity.y = -1.0f;
            //Debug.Log("Is grounded");
        }
        else {
            Velocity.y += GravityForce * GravityMultiplier * Time.deltaTime;
            //Debug.Log("Is airborn");
        }
        CharacterController.Move(Velocity * Time.deltaTime);
    }

    public void jumpAction() {
        if(jump.ReadValue<float>() > 0) {
            if (IsGrounded()) {
                Velocity.y = Mathf.Sqrt(JumpForce * -2 * GravityForce);
            }
        }

    }

    private bool IsGrounded() => CharacterController.isGrounded;


    //Used to make the player collect coins
    private void OnTriggerEnter(Collider other) {

        //When a user collects a coin
        if(other.gameObject.tag == "Coin") {
            CoinsCollected++;
            Points += 10;
            Destroy(other.gameObject);
            CheckMultiplier();
        }

        if (other.gameObject.tag == "Hazard") {
            Hitpoints -= 1;
            if (RunBonus) { //Cancels score bonus upon taking damage
                ScoreBonus = 0;
                BonusMultiplier = 1;
                ConsectiveActions = 0;
                RunBonus = false;
                if (Hitpoints < 0) {
                    isDead = true;
                    CharAnim.Disable();
                }
            }
        }
    }

    public void CheckMultiplier() {
        BonusTimer = 5;
        if (!RunBonus) {
            RunBonus = true;
            ScoreBonus += 10;
            ConsectiveActions += 1;
            if (ConsectiveActions == 5) {
                ConsectiveActions = 0;
                BonusMultiplier += 0.05f;
            }
        }
        else if (RunBonus) {
            ScoreBonus += 10;
            ConsectiveActions += 1;
            if (ConsectiveActions == 5) {
                ConsectiveActions = 0;
                BonusMultiplier += 0.05f;
            }
        }
    }

    public void AddBonus()
    {
        if (RunBonus && BonusTimer > 0) {
            BonusTimer -= Time.deltaTime; //Decreases it by 1 second each frame
        }

        else if (RunBonus && BonusTimer <= 0) {
            ScoreBonus *= BonusMultiplier;
            int TotalBonus = (int)ScoreBonus;
            Points += TotalBonus;
            RunBonus = false;
        }



    }
}
