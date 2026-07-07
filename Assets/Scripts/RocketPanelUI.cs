using TMPro;
using UnityEngine;
public class RocketPanelUI : MonoBehaviour
{
    public GameObject panelGameOver;
    public GameObject CanvasUI;

    [Header("Text")]
    public TMP_Text txtTotalHeight;
    public TMP_Text txtFlightHeight;
    public TMP_Text txtEarnedCoin;
    public TMP_Text txtTotalCoin;

    public void Show(float totalHeight, float flightHeight, int earnedCoin, int totalCoin)
    {
        panelGameOver.SetActive(true);
        CanvasUI.SetActive(false);

        txtTotalHeight.text = $"{(int)totalHeight} m";
        txtFlightHeight.text = $"{(int)flightHeight} m";
        txtEarnedCoin.text = $"+{earnedCoin}";
        txtTotalCoin.text = totalCoin.ToString();
    }
}