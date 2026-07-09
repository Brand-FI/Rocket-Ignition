using UnityEngine;

public class HapticManager : MonoBehaviour
{
    public static HapticManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Vibrate()
    {
        #if UNITY_ANDROID || UNITY_IOS
                Handheld.Vibrate();
        #endif
    }
}