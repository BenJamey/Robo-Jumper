using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryScript : MonoBehaviour
{
    //Variables for getting the text to store the results
    [SerializeField] TextMeshProUGUI PointsText;
    [SerializeField] TextMeshProUGUI LivesText;
    [SerializeField] TextMeshProUGUI LivesBonus;
    [SerializeField] TextMeshProUGUI CoinsText;
    [SerializeField] TextMeshProUGUI CoinsBonus;
    [SerializeField] TextMeshProUGUI TotalScore;
    //Variables for the medals
    [SerializeField] GameObject Gold;
    [SerializeField] GameObject Silver;
    [SerializeField] GameObject Bronze;
    int FinalScore = 0;
    int LivesBonusPoints = 0;
    int CoinBonusPoints = 0;
    // Start is called before the first frame update
    void Start()
    {
        Gold.SetActive(false);
        Silver.SetActive(false);
        Bronze.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PointsText.text = VariableStorage.Points.ToString("000000000");
        CoinsText.text = VariableStorage.CoinsCollected.ToString("000");
        CoinBonusPoints = 150 * VariableStorage.CoinsCollected;
        CoinsBonus.text = CoinBonusPoints.ToString("000000");
        LivesText.text = VariableStorage.Hitpoints.ToString();
        LivesBonusPoints = 1000 * VariableStorage.Hitpoints;
        LivesBonus.text = LivesBonusPoints.ToString("0000");
        FinalScore = VariableStorage.Points + CoinBonusPoints + LivesBonusPoints;
        TotalScore.text = FinalScore.ToString("000000000");
        if (FinalScore < 1000)
        {
            Bronze.SetActive(true);
        }
        else if (FinalScore >= 1000 && FinalScore < 2000)
        {
            Silver.SetActive(true);
        }
        else if (FinalScore >= 2000)
        {
            Gold.SetActive(true);
        }
        //StartCoroutine(ShowRank());
    }

    IEnumerator ShowRank() {
        yield return new WaitForSeconds(2);
        if (FinalScore < 1000) {
            Bronze.SetActive(true);
        }
        else if (FinalScore >= 1000 && FinalScore < 2000) {
            Silver.SetActive(true);
        }
        else if (FinalScore >= 2000) {
            Gold.SetActive(true);
        }
    }
}
