using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPManager : Singleton<IAPManager>
{
    public GameObject purchasePanel, iapConfirmPanel, sucessImage, failureImage, watchAdButton, iapButton, reviveButton;

    private bool isNoAdsAvailable;
    private void Start()
    {
        isNoAdsAvailable = PlayerPrefs.GetString("NoAds") == "true" ? true : false;
    }

    [ContextMenu("Delete IAP Ownership")]
    private void TestDeleteKey()
    {
        isNoAdsAvailable = false;
        PlayerPrefs.DeleteKey("NoAds");
    }

    public void OnAdsPurchaseComplete()
    {
        isNoAdsAvailable = true;
        PlayerPrefs.SetString("NoAds", "true");

        Invoke("WaitToCallAfterAdsPurchaseComplete", .1f);
    }

    public void WaitToCallAfterAdsPurchaseComplete()
    {
        iapConfirmPanel.SetActive(false);
        failureImage.SetActive(false);
        watchAdButton.SetActive(false);
        iapButton.SetActive(false);
        purchasePanel.SetActive(true);
        sucessImage.SetActive(true);
        reviveButton.SetActive(true);
    }

    public void OnAdsPurchaseFailed()
    {
        iapConfirmPanel.SetActive(false);
        sucessImage.SetActive(false);
        purchasePanel.SetActive(true);
        failureImage.SetActive(true);
    }

    public void DeadPlayer()
    {
        if (isNoAdsAvailable)
        {
            iapConfirmPanel.SetActive(false);
            failureImage.SetActive(false);
            watchAdButton.SetActive(false);
            iapButton.SetActive(false);
            purchasePanel.SetActive(true);
            sucessImage.SetActive(true);
            reviveButton.SetActive(true);
        }
    }
}
