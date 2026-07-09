using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
public class Store : MonoBehaviour
{
    [SerializeField] private GameObject btnRestore;
    [SerializeField] private GameObject panelPurchase;
    [SerializeField] private GameObject panelClaim;
    [SerializeField] private TMP_Text txtCoinsEarn;
    private void Awake()
    {
        if(Application.platform != RuntimePlatform.IPhonePlayer)
        {
            if(btnRestore != null) btnRestore.SetActive(false);
        }
    }
    public void OnPurchaseComplete(Product product)
    {
        Debug.Log("Purchase completed: " + product.definition.id);
        int coinAmount = 0;
        switch (product.definition.id)
        {
            case "com.mgpgroup.rocketignition.coin_small":
                coinAmount = 100;
                break;

            case "com.mgpgroup.rocketignition.coin_medium":
                coinAmount = 550;
                break;

            case "com.mgpgroup.rocketignition.coin_large":
                coinAmount = 1200;
                break;
        }
        if (coinAmount > 0)
        {
            UpgradeManager.Instance.AddCoin(coinAmount);
            if (panelPurchase != null) panelPurchase.SetActive(false);
            if (panelClaim != null && txtCoinsEarn != null)
            {
                txtCoinsEarn.text = $"{coinAmount} COINS";
                panelClaim.SetActive(true);
            }
        }
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogWarning($"Purchase failed on {product.definition.id}, reason {failureDescription}");
    }
}
