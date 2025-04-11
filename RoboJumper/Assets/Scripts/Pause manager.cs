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
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] GameObject MuteOption;
    [SerializeField] GameObject UnmuteOptions;
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

    //Used to pause and unpaus the gamew
    void Pause(InputAction.CallbackContext context) {
        isPaused = !isPaused;

        if (isPaused) {
            ActivateMenu();
        } else {
            DeactivateMenu();
        }
    }

    public void ActivateMenu() {
        Time.timeScale = 0;
        PauseUI.SetActive(true);
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DeactivateMenu() {
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

    //Loads the pause menus options
    public void loadOptions() {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        if (AudioListener.volume == 0) {
            MuteOption.SetActive(false);
            UnmuteOptions.SetActive(true);
        }

        else {
            MuteOption.SetActive(true);
            UnmuteOptions.SetActive(false);
        }
    }

    //Lets the user rety the game
    public void Retry() {
        VariableStorage.CoinsCollected = 0;
        VariableStorage.Points = 0;
        VariableStorage.Hitpoints = 3;
        CharacterMovement.BonusMultiplier = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Takers the user back to the main menu
    public void ExitGame() {
        VariableStorage.CoinsCollected = 0;
        VariableStorage.Points = 0;
        VariableStorage.Hitpoints = 3;
        CharacterMovement.BonusMultiplier = 1;
        SceneManager.LoadScene("Main Menu");
    }

    //Mutes the games audio
    public void MuteAudio() {
        AudioListener.volume = 0;
        MuteOption.SetActive(false);
        UnmuteOptions.SetActive(true);

    }
    //UnMutes the games audio
    public void unMute() {
        AudioListener.volume = 1;
        MuteOption.SetActive(true);
        UnmuteOptions.SetActive(false);
    }
}
