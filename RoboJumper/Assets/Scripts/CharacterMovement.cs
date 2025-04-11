using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    public static RobotAnimations CharAnim;
    public static CharacterController CharacterController;
    //Base variables used for basic functions
    static int MaxHitpoints = 3;
    float CurrentVelocity = 0;
    float RunTime = 0; //Used to determine when a character has started running
    public static bool isDead = false;
    public static bool LevelComplete = false;
    [HideInInspector] public Vector3 Direction;
    InputAction add; //Used to instantly add bonus points to curent score
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
    [SerializeField] LayerMask GroundLayer;
    [HideInInspector] public Vector3 Velocity;
    [HideInInspector] public bool JumpPressed = false;
    [HideInInspector] public bool CurrentlyGrounded = false;
    private bool isGrounded;

    //Variables used for getting the bonus points
    public static float ScoreBonus;
    public static float BonusMultiplier = 1.0f;
    int ConsectiveActions = 0;
    [HideInInspector] public static bool RunBonus = false;
    float BonusTimer = 0;

    //Audio Variables
    AudioSource RobotAudio;
    [SerializeField] AudioClip SuccessSound;
    [SerializeField] AudioClip DamageSound;

    private void Awake()
    {
        CharAnim = new RobotAnimations();
    }

    private void OnEnable()
    {
        move = CharAnim.Player.Move;
        jump = CharAnim.Player.Jump;
        add = CharAnim.Player.AddBonus;
        move.Enable();
        jump.Enable();
        add.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        add.Disable();
    }

    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        PlayerCamera = FindFirstObjectByType<CameraMovement>().transform;
        RobotAudio = GetComponent<AudioSource>();
        Time.timeScale = 1;
        isDead = false;
        VariableStorage.CurrentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.15f, GroundLayer);
        if (VariableStorage.Hitpoints < 0)
        {
            isDead = true;
            CharAnim.Disable();
        }

        if (LevelComplete)
        {
            CharAnim.Disable();
            StartCoroutine(LoadResults());
        }

        ApplyGravity();
        ApplyMovement();
        jumpAction();
        AddBonus();
        InstantBonus();

    }

    //Controlling the character movements
    private void ApplyMovement()
    {
        Direction = new Vector3(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y).normalized;
        if (Direction.magnitude > 0.2)
        {
            if (RunTime < 2)
            { //Once the playe have moved for a few seconds he'll start running
                Speed = WalkSpeed;
            }
            else
            {
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
            //ApplyGravity();
        }
    }

    private void ApplyGravity()
    {
        if (isGrounded && Velocity.y < 0.0f)
        {
            Velocity.y = -1.0f;
            CurrentlyGrounded = true;
        }
        else
        {
            Velocity.y += GravityForce * GravityMultiplier * Time.deltaTime;
            CurrentlyGrounded = false;
        }
        CharacterController.Move(Velocity * Time.deltaTime);
    }

    public void jumpAction() {
        if (jump.ReadValue<float>() > 0) {
            if (isGrounded) {
                JumpPressed = true;
                StartCoroutine(Jumped());
            }
        }
    }

    IEnumerator Jumped() {
        yield return new WaitForSeconds(0.25f);
        Velocity.y = Mathf.Sqrt(JumpForce * -2 * GravityForce);
        JumpPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        //When a user collects a coin
        if (other.gameObject.tag == "Coin")
        {
            RobotAudio.PlayOneShot(SuccessSound);
            VariableStorage.CoinsCollected++;
            VariableStorage.Points += 10;
            Destroy(other.gameObject);
            CheckMultiplier();
        }

        if (other.gameObject.tag == "Hazard" || other.gameObject.tag == "Projectile") {
            VariableStorage.Hitpoints -= 1;
            if (RunBonus) { //Cancels score bonus upon taking damage
                ScoreBonus = 0;
                BonusMultiplier = 1;
                ConsectiveActions = 0;
                RunBonus = false;
            }
            RobotAudio.PlayOneShot(DamageSound);
        }

        if (other.gameObject.tag == "Pitt") {
            VariableStorage.Hitpoints = -5;
            if (RunBonus) { //Cancels score bonus upon taking damage
                ScoreBonus = 0;
                BonusMultiplier = 1;
                ConsectiveActions = 0;
                RunBonus = false;
            }
        }
        if (other.gameObject.tag == "Finish") {
            if (RunBonus) {
                BonusTimer = 0;
                AddBonus();
            }
            RobotAudio.PlayOneShot(SuccessSound);
            LevelComplete = true;
        }
    }

    public void CheckMultiplier()
    {
        BonusTimer = 5;
        if (!RunBonus)
        {
            RunBonus = true;
            ScoreBonus += 10;
            ConsectiveActions += 1;
            if (ConsectiveActions >= 5)
            {
                ConsectiveActions = 0;
                BonusMultiplier += 0.05f;
            }
        }
        else if (RunBonus)
        {
            ScoreBonus += 10;
            ConsectiveActions += 1;
            if (ConsectiveActions >= 5)
            {
                ConsectiveActions = 0;
                BonusMultiplier += 0.05f;
            }
        }
    }

    //This is used to add bonis points when a the bonus tally timer runs out
    public void AddBonus()
    {
        if (RunBonus && BonusTimer > 0) {
            BonusTimer -= Time.deltaTime; //Decreases it by 1 second each frame
        }

        else if (RunBonus && BonusTimer <= 0) { //Resets/adds up bonus points when the bonus timer runs out
            ScoreBonus *= BonusMultiplier;
            int TotalBonus = (int)ScoreBonus;
            VariableStorage.Points += TotalBonus;
            ScoreBonus = 0;
            BonusMultiplier = 1;
            ConsectiveActions = 0;
            RunBonus = false;
        }
    }

    public void InstantBonus() {
        if (add.ReadValue<float>() > 0){
            if (RunBonus && BonusTimer > 0) {
                BonusTimer = 0;
                AddBonus();
            }
        }
    }
    IEnumerator LoadResults()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Victory Screen");
    }
}
