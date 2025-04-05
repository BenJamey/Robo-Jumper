using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    //The user chooses to start the game
    [SerializeField] GameObject MainMenuGo;
    void Start()
    {
        MainMenuGo.SetActive(true);
    }

    // Update is called once per frame
    public void OpenTestLevel()
    {
        SceneManager.LoadScene("Test Level");
    }
}
