using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pausemanager : MonoBehaviour
{
    private RobotAnimations InGameCOntrols;
    private InputAction Menucontrols;
    bool LoadDeathMenu = false;

    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private bool isPaused;

    void Awake() {
        InGameCOntrols = new RobotAnimations();
    }

    private void OnEnable()
    {
        Menucontrols = InGameCOntrols.Menu.Escape;
        Menucontrols.Enable();

        Menucontrols.performed += Pause;
    }

    void Start()
    {
        GameOver.SetActive(false);
    }

    private void OnDisable()
    {
        Menucontrols.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterMovement.isDead)
        {
            Menucontrols.Disable();
            StartCoroutine(ActivateGameOverMenu());
        }
    }

    public void Pause(InputAction.CallbackContext context) {
        isPaused = !isPaused;

        if (isPaused) {
            ActivateMenu();
        } else {
            DeactivateMenu();
        }
    }

    void ActivateMenu() {
        Time.timeScale = 0;
        PauseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator ActivateGameOverMenu() {
        yield return new WaitForSeconds(5);
        Time.timeScale = 0;
        GameOver.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DeactivateMenu() { 
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Retry()
    {
        CharacterMovement.CoinsCollected = 0;
        CharacterMovement.Points = 0;
        CharacterMovement.Hitpoints = 3;
        CharacterMovement.BonusMultiplier = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame() {
        CharacterMovement.CoinsCollected = 0;
        CharacterMovement.Points = 0;
        CharacterMovement.Hitpoints = 3;
        CharacterMovement.BonusMultiplier = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
