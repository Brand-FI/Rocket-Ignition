using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RocketPanelUI : MonoBehaviour
{
    public GameObject panelGameOver;
    public GameObject CanvasUI;

    [Header("Text")]
    public TMP_Text txtTotalHeight;
    public TMP_Text txtFlightHeight;
    public TMP_Text txtEarnedCoin;
    public TMP_Text txtTotalCoin;

    public Button btnAd;
    private int lastEarnedCoin;
    private bool adRewardClaimed;

    public void Show(float totalHeight, float flightHeight, int earnedCoin, int totalCoin)
    {
        panelGameOver.SetActive(true);
        CanvasUI.SetActive(false);

        txtTotalHeight.text = $"{(int)totalHeight} m";
        txtFlightHeight.text = $"{(int)flightHeight} m";
        txtEarnedCoin.text = $"+{earnedCoin}";
        txtTotalCoin.text = totalCoin.ToString();

        lastEarnedCoin = earnedCoin;
        adRewardClaimed = false;
        if (btnAd != null) btnAd.gameObject.SetActive(true);
    }
    public void OnWatchAdButtonClicked()
    {
        if (adRewardClaimed) return;
        AdManager.instance.ShowRewardedAd(() =>
        {
            adRewardClaimed = true;
            UpgradeManager.Instance.AddCoin(lastEarnedCoin);
            HapticManager.Instance.Vibrate();

            txtEarnedCoin.text = $"+{lastEarnedCoin * 2}";
            txtTotalCoin.text = UpgradeManager.Instance.coin.ToString();
            if(btnAd != null) btnAd.gameObject.SetActive(false);
        });
    }
}