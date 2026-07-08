using UnityEngine;
using UnityEngine.Advertisements;
using Unity.Services.LevelPlay;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager instance;

    private string appKey = "27141a56d";
    public string rewardedAdUnitId = "txpresm8q2hfilpy";

    private LevelPlayRewardedAd rewardedAd;
    private bool isSdkReady = false;
    private Action onRewardEarnedCallback;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    void Start()
    {
        LevelPlay.OnInitSuccess += OnInitSuccess;
        LevelPlay.OnInitFailed += OnInitFailed;
        LevelPlay.Init(appKey);
    }
    void OnInitSuccess(LevelPlayConfiguration config)
    {
        Debug.Log("LevelPlay initialization complete.");
        isSdkReady = true;
        rewardedAd = new LevelPlayRewardedAd(rewardedAdUnitId);
        rewardedAd.OnAdLoaded += OnAdLoaded;
        rewardedAd.OnAdLoadFailed += OnAdLoadFailed;
        rewardedAd.OnAdRewarded += OnAdRewarded;
        rewardedAd.OnAdClosed += OnAdClosed;
        LoadAd();
    }
    void OnInitFailed(LevelPlayInitError error)
    {
        Debug.Log("LevelPlay Initialization Failed: " + error);
    }
    public void LoadAd()
    {
        if (!isSdkReady) return;
        rewardedAd.LoadAd();
    }
    void OnAdLoaded(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Rewarded ad loaded.");
    }
    void OnAdLoadFailed(LevelPlayAdError adError)
    {
        Debug.Log("Error loading rewarded ad: " + adError);
    }
    public void ShowRewardedAd(Action onRewardEarn)
    {
        if (isSdkReady && rewardedAd != null && rewardedAd.IsAdReady()) 
        {
            onRewardEarnedCallback = onRewardEarn;
            rewardedAd.ShowAd();
        }
        else
        {
            Debug.Log("Ad is not ready, try load again...");
            LoadAd();
        }
    }
    void OnAdRewarded(LevelPlayAdInfo adInfo, LevelPlayReward reward)
    {
        onRewardEarnedCallback?.Invoke();
        onRewardEarnedCallback = null;
    }
    void OnAdClosed(LevelPlayAdInfo adInfo)
    {
        LoadAd();
    }
}
