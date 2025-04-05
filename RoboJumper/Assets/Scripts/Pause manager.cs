using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pausemanager : MonoBehaviour
{
    private RobotAnimations InGameCOntrols;
    private InputAction Menucontrols;

    [SerializeField] private GameObject PauseUI;
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

    private void OnDisable()
    {
        Menucontrols.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void DeactivateMenu() { 
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitGame() {
        SceneManager.LoadScene("Main Menu");
    }
}
