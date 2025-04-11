using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableStorage : MonoBehaviour
{
    //These variables will be saved/reused across all the different functions
    public static int Hitpoints = 3;
    //VariableStorage.Hitpoints     This is used as reference for copying and pasting
    public static int CoinsCollected;
    //VariableStorage.CoinsCollected
    public static int Points;
    //VariableStorage.Points
    public static int CurrentLevel; //Will be used in future for loading the levels
    //VariableStorage.CurrentLevel
    public static bool ShowRank = true; //Used tod etermine weather or not a rank is shown at the end of the stage
    //VariableStorage.ShowRank
    //public static bool isMuted = false;
}
