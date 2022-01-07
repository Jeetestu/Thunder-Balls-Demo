using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.ThunderAndLightning;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;
    [Header("References")]
    public bool gameOver = false;
    public LightningBoltPrefabScript boltScript;
    public LightningBoltShapeSphereScript sphereScript;
    public Light2D globalLight;
    public Light2D sphereLight;
    public AudioSource audio;


    [Header("Platform References")]
    public Light2D platformLight;
    public CanvasGroup platformScoreCanvasGroup;

    [Header("UI References")]
    public GameObject endGameCanvas;
    public CanvasGroup endGameCanvasGroup;
    public TMP_Text finalScoreText;
    public GameObject emergencyCanvas;
    public CanvasGroup emergencyCanvasGroup;
    public TMP_Text inGameScoreText;

    [Header("Game Over Bar References")]
    public GameObject gameOverBarParent;
    public LightningBoltPrefabScript gameOverBarLightning;
    public Light2D gameOverBarLight;
    public Transform lightningOrigin;
    public Transform lightningDestination;

    [Header("Console References")]
    public Light2D upConsoleLight;
    public Light2D downConsoleLight;

    
    [Header("References for destruction")]
    public List<GameObject> objectsToDestroyAfterBalls;

    [Header("Game Start Settings")]
    public float timeToFadeIn;

    [Header("Game Ending Settings")]
    public float timeToSpawn;
    public float timeToDespawn;
    public float ballDestructionInterval;
    public float objectDestructionInterval;
    public float timeToSpawnMenu;
    public float timeToFadeMenu;

    [Header("Emergency Settings")]
    public float timeToSpawnEmergency;
    public float emergencyCountdownTimer;

    float gameOverBarTargetIntensity;
    float gameOverBarLightningTargetGlowIntensity;
    float gameOverBarTargetLightIntensity;
    float globalLightTargetIntensity;
    float platformLightTargetIntensity;
    float consoleTargetIntensity;
    Vector3 lightningOriginStart;
    Vector3 lightningDestinationStart;

    private void Awake()
    {
        instance = this;
        gameOverBarTargetIntensity = gameOverBarLightning.Intensity;
        gameOverBarLightningTargetGlowIntensity = gameOverBarLightning.GlowIntensity;
        gameOverBarTargetLightIntensity = gameOverBarLight.intensity;
        globalLightTargetIntensity = globalLight.intensity;
        platformLightTargetIntensity = platformLight.intensity;
        consoleTargetIntensity = upConsoleLight.intensity;
        lightningOriginStart = lightningOrigin.localPosition;
        lightningDestinationStart = lightningDestination.localPosition;
        StartCoroutine(startGameRoutine());
    }

    public void ReloadLevel()
    {
        AdsManager.instance.PlayInterstitialAd();
        StartCoroutine(reloadLevelRoutine());
    }

    IEnumerator reloadLevelRoutine()
    {
        float count = 0;
        float progress;
        while (count < timeToFadeMenu)
        {
            count += Time.deltaTime;
            progress = count / timeToFadeMenu;
            endGameCanvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(1);
    }

    IEnumerator startGameRoutine()
    {
        float count = 0;
        float progress;


        while (count < timeToFadeIn)
        {
            count += Time.deltaTime;
            progress = count / timeToFadeIn;
            gameOverBarParent.transform.localScale = new Vector3(Mathf.Clamp(progress, 0f, 1f), 1f, 1f);
            gameOverBarLightning.Intensity = Mathf.Lerp(0f, gameOverBarTargetIntensity, progress);
            gameOverBarLightning.GlowIntensity = Mathf.Lerp(0f, gameOverBarLightningTargetGlowIntensity, progress);
            gameOverBarLight.intensity = Mathf.Lerp(0f, gameOverBarTargetLightIntensity, progress);
            globalLight.intensity = Mathf.Lerp(0f, globalLightTargetIntensity, progress);
            platformLight.intensity = Mathf.Lerp(0f, platformLightTargetIntensity, progress);
            platformScoreCanvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
            upConsoleLight.intensity = Mathf.Lerp(0f, consoleTargetIntensity, progress);
            downConsoleLight.intensity = Mathf.Lerp(0f, consoleTargetIntensity, progress);
            yield return new WaitForEndOfFrame();
        }
    }
    public void GameOverBarHit(Vector2 position)
    {
        gameOver = true;
        if (AdsManager.instance.canUseReward == false)
        {
            emergencyResetNoDecision();
        }
        else
        {
            StartCoroutine(transitionEmergencyMenu());
        }
        transform.position = position;
        StartCoroutine(spawnLightningBall());

    }

    public void emergencyResetYesDecision()
    {
        AdsManager.instance.canUseReward = false;
        AdsManager.instance.PlayRewardedAd();
        PlayerPrefs.SetInt("RewardsWithoutVideo", PlayerPrefs.GetInt("RewardsWithoutVideo") + 1);
        StartCoroutine(emergencyResetRoutine());
        //need to reset the lightning balls and gameOverBar
    }

    IEnumerator emergencyResetRoutine()
    {
        yield return StartCoroutine(transitionEmergencyMenu(false));
        yield return StartCoroutine(destroyAllBallsAboveHeight(transform.position.y - 1, true));
        BarManager.instance.targetYPosition = transform.position.y;
        yield return StartCoroutine(despawnLightningBallForReset());
        gameOver = false;
        BarManager.instance.targetYPosition = BarManager.instance.maxYPosition;
    }

    public void emergencyResetNoDecision()
    {
        StartCoroutine(transitionEmergencyMenu(false));
        endGame();
    }

    public void endGame()
    {
        AdsManager.instance.incrementGameCount();
        AdsManager.instance.canUseReward = true;
        ScoreManager.instance.setupRetryMenuScore();
        StartCoroutine(endGameRoutine());
    }

    IEnumerator spawnLightningBall()
    {
        //Spawns the lightningBall
        float count = 0;
        float progress;
        sphereScript.enabled = true;
        sphereLight.enabled = true;
        gameOverBarLight.intensity = 0f;
        audio.enabled = true;
        audio.Play();
        while (count < timeToSpawn)
        {
            count += Time.deltaTime;
            progress = count / timeToSpawn;
            lightningOrigin.position = Vector3.Lerp(lightningOrigin.position, transform.position, progress);
            lightningDestination.position = Vector3.Lerp(lightningDestination.position, transform.position, progress);
            sphereScript.Intensity = Mathf.Lerp(0f, 3.5f, progress);
            sphereScript.GlowIntensity = Mathf.Lerp(0f, 5f, progress);
            sphereLight.intensity = Mathf.Lerp(0f, 3f, progress);
            yield return new WaitForEndOfFrame();
        }
    }

    //used in response to the reset reward (vs. the fade out during game over which is built into the game over routine)
    IEnumerator despawnLightningBallForReset()
    {
        float count = 0;
        float progress;

        audio.Stop();
        Vector3 lightningBallLocalPosition = lightningOrigin.InverseTransformPoint(transform.position);
        while (count < timeToFadeIn)
        {
            count += Time.deltaTime;
            progress = count / timeToFadeIn;
            lightningOrigin.localPosition = Vector3.Lerp(lightningBallLocalPosition, lightningOriginStart, progress);
            lightningDestination.localPosition = Vector3.Lerp(lightningBallLocalPosition, lightningDestinationStart, progress);
            gameOverBarParent.transform.localScale = new Vector3(Mathf.Clamp(progress, 0f, 1f), 1f, 1f);
            gameOverBarLightning.Intensity = Mathf.Lerp(0f, gameOverBarTargetIntensity, progress);
            gameOverBarLightning.GlowIntensity = Mathf.Lerp(0f, gameOverBarLightningTargetGlowIntensity, progress);
            gameOverBarLight.intensity = Mathf.Lerp(0f, gameOverBarTargetLightIntensity, progress);
            sphereScript.Intensity = Mathf.Lerp(3.5f, 0f, progress);
            sphereScript.GlowIntensity = Mathf.Lerp(5f, 0f, progress);
            yield return new WaitForEndOfFrame();
        }

        sphereScript.enabled = false;
        sphereLight.enabled = false;
    }

    IEnumerator transitionEmergencyMenu(bool enable = true)
    {
        float count;
        float progress;
        emergencyCanvas.SetActive(true);
        count = 0;

        float starting;
        float ending;
        EmergencyCanvas c = emergencyCanvas.GetComponent<EmergencyCanvas>();
        c.advertisementChanceText.text = AdsManager.instance.oddsOfRewardAd() + "% chance of advertisement";
        if (enable)
        {

            c.countdown = emergencyCountdownTimer;
            c.countDownActive = true;
            
            starting = 0f;
            ending = 1f;
        }
        else
        {
            c.countDownActive = false;
            starting = emergencyCanvasGroup.alpha;
            ending = 0f;
        }

        while (count < timeToSpawnEmergency)
        {
            count += Time.deltaTime;
            progress = count / timeToSpawnEmergency;
            emergencyCanvasGroup.alpha = Mathf.Lerp(starting, ending, progress);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator destroyAllBallsAboveHeight(float height = -100f, bool onlyCaughtBalls = false)
    {

        //get list of all balls
        List<BallCollisionLogic> allBalls = new List<BallCollisionLogic>();
        allBalls.AddRange(FindObjectsOfType<BallCollisionLogic>());

        allBalls.Sort(delegate (BallCollisionLogic a, BallCollisionLogic b)
        {
            return Vector2.Distance(this.transform.position, a.transform.position)
            .CompareTo(
                Vector2.Distance(this.transform.position, b.transform.position));
        });

        boltScript.enabled = true;

        while (allBalls.Count > 0)
        {
            if (allBalls[0] == null)
                allBalls.RemoveAt(0);
            else if (allBalls[0].transform.position.y < height || (onlyCaughtBalls && (allBalls[0].gameObject.layer == LayerMask.NameToLayer("Default") || allBalls[0].gameObject.layer == LayerMask.NameToLayer("LaunchingBall"))))
                allBalls.RemoveAt(0);
            else
            {
                GameObject nextTarget = allBalls[0].gameObject;
                boltScript.Destination = nextTarget;
                yield return new WaitForSeconds(ballDestructionInterval);
                if (allBalls[0] != null)
                {
                    boltScript.Destination = null;
                    allBalls[0].releaseBall();
                    allBalls[0].destroyAllConnectedLightning();
                    Destroy(nextTarget);
                }

                allBalls.RemoveAt(0);

            }

        }

        boltScript.enabled = false;

    }

    IEnumerator endGameRoutine()
    {

        float count;
        float progress;

        yield return StartCoroutine(destroyAllBallsAboveHeight());

        boltScript.enabled = true;

        //destroys other objects
        while (objectsToDestroyAfterBalls.Count > 0)
        {
            GameObject nextTarget = objectsToDestroyAfterBalls[0];
            boltScript.Destination = nextTarget;
            yield return new WaitForSeconds(objectDestructionInterval);
            boltScript.Destination = null;
            objectsToDestroyAfterBalls.RemoveAt(0);
            Destroy(nextTarget);
        }

        boltScript.enabled = false;

        //despawns lightning ball and global illumination
        count = 0;

        float initialGlobalLightIntensity = globalLight.intensity;
        SoundManager.i.modifyAudioSource(this.gameObject, timeToDespawn, volume: 0f);
        while (count < timeToDespawn)
        {
            count += Time.deltaTime;
            progress = count / timeToDespawn;
            sphereScript.Intensity = Mathf.Lerp(3.5f, 0f, progress);
            sphereScript.GlowIntensity = Mathf.Lerp(5f, 0f, progress);
            globalLight.intensity = Mathf.Lerp(initialGlobalLightIntensity, 0f, progress);
            sphereLight.intensity = Mathf.Lerp(3f, 0f, progress);
            inGameScoreText.alpha = Mathf.Lerp(1f, 0f, progress);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);
        //spawn endGameMenu

        endGameCanvas.SetActive(true);
        count = 0;

        while (count < timeToSpawnMenu)
        {
            count += Time.deltaTime;
            progress = count / timeToSpawnMenu;
            endGameCanvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
            yield return new WaitForEndOfFrame();
        }
    }
}
