using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Hierarachy References")] 
    [SerializeField] private Text coinsText;
    [SerializeField] private Text pointsText;
    
    //-------------------------------------------------------------------------
    public void UpdateCoinsAmount(int _coinsCount)
    {
        coinsText.text = _coinsCount.ToString();
    }

    public void UpdatePointsText(int _pointsAmount)
    {
        pointsText.text = _pointsAmount.ToString();
    }
}
