using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Reference")]
    public Rocket rocket;
    public RocketStatsUI rocketStatsUI;

    [Header("Currency")]
    public int coin;

    [Header("Fuel Tank")]
    public int fuelLevel;
    public int fuelBasePrice = 50;
    public float fuelPriceMultiplier = 1.18f;

    [Header("Speed")]
    public int speedLevel;
    public int speedBasePrice = 100;
    public float speedPriceMultiplier = 1.25f;

    [Header("Fuel Efficiency")]
    public int efficiencyLevel;
    public int efficiencyBasePrice = 75;
    public float efficiencyPriceMultiplier = 1.20f;

    [Header("UI")]
    public TMP_Text txtCoin;

    public TMP_Text txtFuelPrice;
    public TMP_Text txtSpeedPrice;
    public TMP_Text txtEfficiencyPrice;

    public TMP_Text txtFuelLevel;
    public TMP_Text txtSpeedLevel;
    public TMP_Text txtEfficiencyLevel;

    //Baseline
    public float baseFuel = 100f;
    public float baseTankMass = 20f;
    public float baseThrust = 300f;
    public float baseConsumption = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        coin = PlayerPrefs.GetInt("Coin", 0);

        fuelLevel = PlayerPrefs.GetInt("FuelLevel", 0);
        speedLevel = PlayerPrefs.GetInt("SpeedLevel", 0);
        efficiencyLevel = PlayerPrefs.GetInt("EfficiencyLevel", 0);

        ApplyUpgrade();
        UpdateUI();
    }

    void ApplyUpgrade()
    {
        // Fuel Tank
        rocket.maxFuel = baseFuel + fuelLevel * 10;
        rocket.currentFuel = rocket.maxFuel;
        rocket.fuelTankMass = baseTankMass + fuelLevel;

        // Speed
        rocket.thrust = baseThrust + speedLevel * 15;

        // Efficiency
        rocket.fuelConsumption = Mathf.Max(0.1f, baseConsumption - efficiencyLevel * 0.02f);

        rocket.UpdateUI();
        rocketStatsUI.UpdateStats();
    }

    public void AddCoin(int amount)
    {
        coin += amount;
        PlayerPrefs.SetInt("Coin", coin);
        UpdateUI();
    }

    public void BuyFuel()
    {
        int price = GetFuelPrice();

        if (coin < price)
            return;

        coin -= price;
        fuelLevel++;
        HapticManager.Instance.Vibrate();

        Save();
    }

    public void BuySpeed()
    {
        int price = GetSpeedPrice();

        if (coin < price)
            return;

        coin -= price;
        speedLevel++;
        HapticManager.Instance.Vibrate();

        Save();
    }

    public void BuyEfficiency()
    {
        int price = GetEfficiencyPrice();

        if (coin < price)
            return;

        coin -= price;
        efficiencyLevel++;
        HapticManager.Instance.Vibrate();

        Save();
    }

    void Save()
    {
        PlayerPrefs.SetInt("Coin", coin);

        PlayerPrefs.SetInt("FuelLevel", fuelLevel);
        PlayerPrefs.SetInt("SpeedLevel", speedLevel);
        PlayerPrefs.SetInt("EfficiencyLevel", efficiencyLevel);

        PlayerPrefs.Save();

        ApplyUpgrade();
        UpdateUI();
    }

    public int GetFuelPrice()
    {
        return Mathf.RoundToInt(fuelBasePrice * Mathf.Pow(fuelPriceMultiplier, fuelLevel));
    }

    public int GetSpeedPrice()
    {
        return Mathf.RoundToInt(speedBasePrice * Mathf.Pow(speedPriceMultiplier, speedLevel));
    }

    public int GetEfficiencyPrice()
    {
        return Mathf.RoundToInt(efficiencyBasePrice * Mathf.Pow(efficiencyPriceMultiplier, efficiencyLevel));
    }

    void UpdateUI()
    {
        txtCoin.text = coin.ToString();

        txtFuelPrice.text = GetFuelPrice().ToString();
        txtSpeedPrice.text = GetSpeedPrice().ToString();
        txtEfficiencyPrice.text = GetEfficiencyPrice().ToString();

        txtFuelLevel.text = "LEVEL " + fuelLevel;
        txtSpeedLevel.text = "LEVEL " + speedLevel;
        txtEfficiencyLevel.text = "LEVEL " + efficiencyLevel;
    }
}