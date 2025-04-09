using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pausemanager : MonoBehaviour
{
    private RobotAnimations InGameControls;
    private InputAction Menucontrols;
    bool LoadDeathMenu = false;

    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private bool isPaused;

    void Awake() {
        InGameControls = new RobotAnimations();
    }

    private void OnEnable()
    {
        Menucontrols = InGameControls.Menu.Escape;
        Menucontrols.Enable();

        Menucontrols.performed += Pause;
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

    void Pause(InputAction.CallbackContext context) {
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

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    IEnumerator ActivateGameOverMenu() {
        yield return new WaitForSeconds(3);
        Time.timeScale = 0;
        GameOver.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        VariableStorage.CoinsCollected = 0;
        VariableStorage.Points = 0;
        VariableStorage.Hitpoints = 3;
        CharacterMovement.BonusMultiplier = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame() {
        VariableStorage.CoinsCollected = 0;
        VariableStorage.Points = 0;
        VariableStorage.Hitpoints = 3;
        CharacterMovement.BonusMultiplier = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
