using TMPro;
using UnityEngine;

public class RocketStatsUI : MonoBehaviour
{
    public Rocket rocket;
    public TMP_Text txtSpeed;
    public TMP_Text txtFuel;
    public TMP_Text txtUsage;
    public TMP_Text txtWeight;

    public GameObject panelStats;

    public void UpdateStats()
    {
        float speed = rocket.thrust / (rocket.baseMass + rocket.fuelTankMass);
        txtSpeed.text = $"SPEED: {speed:F1} m/s";
        txtFuel.text = $"FUEL TANK: {rocket.maxFuel:F0} L";
        txtUsage.text = $"FUEL USAGE: {rocket.fuelConsumption:F2} L/s";
        txtWeight.text = $"WEIGHT: {rocket.baseMass + rocket.fuelTankMass:F0} kg";
    }

    public void Toggle()
    {
        panelStats.SetActive(!panelStats.activeSelf);
    }
}
