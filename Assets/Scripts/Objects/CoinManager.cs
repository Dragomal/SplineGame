using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CoinManager : MonoBehaviour
{
    private int coinCount;
    private Text coinText;
    [SerializeField] private GameObject _canvasPrefab;

    public void AddCoin(){
        coinCount++;

        TextMeshProUGUI coinCountText = _canvasPrefab.GetComponentInChildren<TextMeshProUGUI>();

        coinCountText.text = ": " + coinCount.ToString();
    }
}