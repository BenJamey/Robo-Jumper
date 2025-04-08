using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class HUDScript : MonoBehaviour
{
    //public RobotAnimations MenuControls;
    //private CharacterMovement IngameControls;
    //The user chooses to start the game
    [SerializeField] GameObject HudGo;
    [SerializeField] GameObject BonusTally;
    //public CharacterController CharacterController;
    //Variables for the values
    [SerializeField] TextMeshProUGUI LivesText;
    [SerializeField] TextMeshProUGUI CoinsText;
    [SerializeField] TextMeshProUGUI PointsText;
    [SerializeField] TextMeshProUGUI BonusText;
    [SerializeField] TextMeshProUGUI BonusMultipliert;
    void Start()
    {
        HudGo.SetActive(true);
        BonusTally.SetActive(false);
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
}
