using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdsManager : MonoBehaviour, IUnityAdsListener
{


public static AdsManager instance;
#if UNITY_IOS
    string gameID = "4519572";
    string platform = "iOS";
#else
    string gameID = "4519573";
    string platform = "Android";
#endif
    public int gamesPlayed;
    string videoAdName;
    string rewardAdName;
    public bool canUseReward;
    // Start is called before the first frame update
    void Start()
    {
        gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
        DontDestroyOnLoad(this.gameObject);
        videoAdName = "Interstitial_" + platform;
        rewardAdName = "Rewarded_" + platform;
        Advertisement.Initialize(gameID);
        Advertisement.AddListener(this);
        canUseReward = true;
    }

    public void incrementGameCount()
    {
        gamesPlayed++;
        PlayerPrefs.SetInt("GamesPlayed", gamesPlayed);
    }

    public bool shouldPlayAd()
    {
        return gamesPlayed % 10 == 0;
    }

    public int oddsOfRewardAd()
    {
        return (int)(((PlayerPrefs.GetInt("RewardsWithoutVideo", 0)) / 4.7f)*100f);
    }


    public void PlayInterstitialAd(bool checkConditions = true)
    {
        if (checkConditions)
        {
            if (!shouldPlayAd())
                return;
        }
        Advertisement.Show(videoAdName);
    }
    

    public void PlayRewardedAd(bool checkConditions = true)
    {
        if (Random.Range(0, 100) < oddsOfRewardAd())
        {
            Advertisement.Show(rewardAdName);
            PlayerPrefs.SetInt("RewardsWithoutVideo", -1);
        }
    }

    /* DEBUG
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
            PlayInterstitialAd();
        if (Input.GetKeyDown(KeyCode.M))
            PlayRewardedAd();
    }
    */
    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        /*
        if (placementId.Contains("Rewarded") && showResult == ShowResult.Finished)
        {
            Debug.Log("Player Should Be rewarded");
        }
        */
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsReady(string placementId)
    {

    }

}
