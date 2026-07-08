using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;
using TMPro;
using System;

public class Notification : MonoBehaviour
{
    [SerializeField] private int bonusIntervalSeconds = 300;
    [SerializeField] private int bonusCOinAmount = 20;

    [SerializeField] private GameObject btnDaily;
    [SerializeField] private GameObject panelClaim;
    [SerializeField] private TMP_Text txtCoins;

    private static readonly string NEXT_CLAIM_KEY = "NextClaimUnixTime";
    private static readonly string CHANNEL_ID = "daily_bonus_channel";
    private float checkTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = CHANNEL_ID,
            Name = "Daily Bonus",
            Importance = Importance.Default,
            Description = "Notifikasi saat bonus coin siap diklaim"
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
        RefreshClaimState();
    }

    // Update is called once per frame
    void Update()
    {
        checkTimer += Time.deltaTime;
        if(checkTimer >= 1f)
        {
            checkTimer = 0f;
            RefreshClaimState();
        }
    }
    private long GetCurrentUnixTime()
    {
        return((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
    }
    private long GetNextClaimUnixTime()
    {
        string saved = PlayerPrefs.GetString(NEXT_CLAIM_KEY, "0");
        return long.Parse(saved);
    }
    private void RefreshClaimState()
    {
        long nextClaim = GetNextClaimUnixTime();
        long current = GetCurrentUnixTime();
        bool isReady = current >= nextClaim;
        btnDaily.SetActive(isReady);
    }
    public void ClaimBonus()
    {
        long current = GetCurrentUnixTime();
        long nextClaim = GetNextClaimUnixTime();

        if (current < nextClaim) return;
        UpgradeManager.Instance.AddCoin(bonusCOinAmount);
        long newNextCLaim = current + bonusIntervalSeconds;
        PlayerPrefs.SetString(NEXT_CLAIM_KEY, newNextCLaim.ToString());

        btnDaily.SetActive(false);
        txtCoins.text = bonusCOinAmount.ToString() + "COINS";
        panelClaim.SetActive(true);
    }
    public void sendNotification(string title, string content, int seconds)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = content;
        notification.FireTime = DateTime.Now.AddSeconds(seconds);
        AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
    }

    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            long nextClaim = GetNextClaimUnixTime();
            long current = GetCurrentUnixTime();
            long secondsLeft = nextClaim - current;
            if (secondsLeft > 0)
            {
                sendNotification("Rocket Ignition", "Your claim is here! Ambil bonus coin gratismu sekarang.", (int)secondsLeft);
            }
        }
        else
        {
            RefreshClaimState();
        }
    }
}
