using TMPro;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Weight")]
    public float baseMass = 100f;
    public float fuelTankMass = 20f;

    [Header("Fuel")]
    public float maxFuel = 100f;
    public float currentFuel = 100f;
    public float fuelConsumption = 1f;

    [Header("Engine")]
    public float thrust = 300f;

    [Header("Height")]
    public float totalHeight;
    public float flightHeight;

    [Header("Physics")]
    public float currentMass;
    public float velocity;


    public TMP_Text txtAltitude, txtFuel;

    public RocketPanelUI RocketUI;

    private bool isFlying = false;

    private void Start()
    {
        totalHeight = PlayerPrefs.GetFloat("TotalHeight", 0);
        UpdateUI();
    }
    void Update()
    {
        if (!isFlying)
        {
            return;
        }
        currentMass = baseMass + fuelTankMass;
        velocity = thrust / currentMass;
        currentFuel -= fuelConsumption * Time.deltaTime;

        if (currentFuel <= 0)
        {
            currentFuel = 0;
            velocity = 0;
            Stop();
            return;
        }

        float distance = velocity * Time.deltaTime;
        flightHeight += distance;
        totalHeight += distance;
        UpdateUI();
    }
    public void UpdateUI()
    {
        txtFuel.text = $"{(int)currentFuel} / {(int)maxFuel}";
        txtAltitude.text = $"{(int)flightHeight} m";
    }

    public void Play()
    {
        isFlying = true;
        HapticManager.Instance.Vibrate();

        Parallax.Instance.Play();

        UpdateUI();
    }

    public void Stop()
    {
        isFlying = false;
        velocity = 0;
        HapticManager.Instance.Vibrate();

        int earnedCoin = Mathf.FloorToInt(flightHeight / 10f);
        UpgradeManager.Instance.AddCoin(earnedCoin);
        PlayerPrefs.SetFloat("TotalHeight", totalHeight);
        PlayerPrefs.Save();

        RocketUI.Show(totalHeight, flightHeight, earnedCoin, UpgradeManager.Instance.coin);

        currentFuel = maxFuel;
        UpdateUI();

        Parallax.Instance.Stop();
        flightHeight = 0;

    }
}