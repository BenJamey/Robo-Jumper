using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    //The user chooses to start the game
    [SerializeField] GameObject MainMenuGo;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] GameObject MuteOption;
    [SerializeField] GameObject UnmuteOptions;
    [SerializeField] GameObject DisablRanks;
    [SerializeField] GameObject EnableRanks;
    //The 2 variables below explain what the mute andrank buttons do, and change depending on whats set
    [SerializeField] TextMeshProUGUI MuteDescriptor;
    [SerializeField] TextMeshProUGUI RankDescriptor;
    void Start() {
        MainMenuGo.SetActive(true);
        OptionsMenu.SetActive(false);
        //Checks weather or not the audio is muted and weather or not ranks are disabled and changes the buttons accoredingly
        if (AudioListener.volume == 0) {
            MuteOption.SetActive(false);
            UnmuteOptions.SetActive(true);
            MuteDescriptor.text = "Enables the games sound";
        }

        else {
            MuteOption.SetActive(true);
            UnmuteOptions.SetActive(false);

            MuteDescriptor.text = "Disables the games sound";
        }

        if (VariableStorage.ShowRank) {
            EnableRanks.SetActive(false);
            DisablRanks.SetActive(true);
            RankDescriptor.text = "Disables ranks at the end of levels";
        }

        else if (!VariableStorage.ShowRank)
        {
            EnableRanks.SetActive(true);
            DisablRanks.SetActive(false);
            RankDescriptor.text = "Enables ranks at the end of levels";
        }
    }

    // Update is called once per frame
    public void OpenTestLevel() {
        SceneManager.LoadScene("Test Level");
    }

    //Used to load the options menu
    public void LoadOptions() {
        MainMenuGo.SetActive(false);
        OptionsMenu.SetActive(true);

    }
    
    //Mutes the games audio
    public void MuteAudio() {
        AudioListener.volume = 0;
        MuteOption.SetActive(false);
        UnmuteOptions.SetActive(true);
        MuteDescriptor.text = "Enables the games sound";

    }
    //Unmutes the games audio
    public void unMute() {
        AudioListener.volume = 1;
        MuteOption.SetActive(true);
        UnmuteOptions.SetActive(false);
        MuteDescriptor.text = "Disables the games sound";
    }

    //Takes the user back to the main menu options
    public void LoadMain() {
        OptionsMenu.SetActive(false);
        MainMenuGo.SetActive(true);
    }

    //Disables end level ranks
    public void DisableRanks() {
        EnableRanks.SetActive(true);
        DisablRanks.SetActive(false);
        VariableStorage.ShowRank = false;
        RankDescriptor.text = "Enables ranks at the end of levels";
    }

    //Enables end level ranks
    public void ShowRanks() {
        EnableRanks.SetActive(false);
        DisablRanks.SetActive(true);
        VariableStorage.ShowRank = true;
        RankDescriptor.text = "Disables ranks at the end of levels";
    }

    public void EndGame() {
        Debug.Log("GameEnded");
        Application.Quit();

    }
}
