using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class HUDScript : MonoBehaviour
{
    //public RobotAnimations MenuControls;
    //public static CharacterController CharacterController;
    //The user chooses to start the game
    [SerializeField] GameObject HudGo;
    [SerializeField] GameObject BonusTally;
    //[SerializeField] GameObject PauseMenu;
    //InputAction pause;
    //public CharacterController CharacterController;
    //Variables for the values
    [SerializeField] TextMeshProUGUI LivesText;
    [SerializeField] TextMeshProUGUI CoinsText;
    [SerializeField] TextMeshProUGUI PointsText;
    [SerializeField] TextMeshProUGUI BonusText;
    [SerializeField] TextMeshProUGUI BonusMultipliert;

    //private void Awake()
    //{
    //    MenuControls = new RobotAnimations();
    //}

    //private void OnEnable()
    //{
    //    pause = MenuControls.Player.Pause;
    //    pause.Enable();
    //}

    //private void OnDisable()
    //{
    //    pause.Disable();
    //}
    void Start()
    {
        //CharacterController = GetComponent<CharacterController>();
        HudGo.SetActive(true);
        BonusTally.SetActive(false);
        //PauseMenu.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        LivesText.text = VariableStorage.Hitpoints.ToString();
        if (VariableStorage.Hitpoints < 0) {
            LivesText.text = "0";
        }
        CoinsText.text = VariableStorage.CoinsCollected.ToString("000");
        PointsText.text = VariableStorage.Points.ToString("000000000");

        if (CharacterMovement.RunBonus)
        {
            BonusTally.SetActive(true);
            BonusText.text = CharacterMovement.ScoreBonus.ToString("000000000");
            BonusMultipliert.text = CharacterMovement.BonusMultiplier.ToString("F2");
        }
        else {
            BonusTally.SetActive(false);
        }

        

    }

    //public void PauseController()
    //{
    //    if (pause.ReadValue<float>() > 0) {
    //        Debug.Log("Pause button pressed");
    //        ActivateMenu();
    //    }
    //}

    //void ActivateMenu() {
    //    Time.timeScale = 0;
    //    PauseMenu.SetActive(true);
    //    Cursor.lockState = CursorLockMode.None;
    //    Cursor.visible = true;
    //}

    //public void ResumeGame() {
    //    Time.timeScale = 1;
    //    PauseMenu.SetActive(false);
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //}
}
