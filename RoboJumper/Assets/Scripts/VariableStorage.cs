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
}
