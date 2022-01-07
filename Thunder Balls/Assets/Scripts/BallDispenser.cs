using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDispenser : MonoBehaviour
{

    public static BallDispenser instance;

    [Header("References")]
    public GameObject ballPrefab;
    public Transform ballSpawnPoint;
    public Transform ballLaunchPoint;
    public LauncherVisualController launchVisual;
    public AudioSource launchAudioSource;
    [Header("Launch Settings")]
    public float minLaunchForce;
    public float maxLaunchForce;
    public float startingFrequency;
    public float endingFrequency;
    public float timeToReachEndingFrequency;
    public int totalBalls;

    public float launchFrequency;

    private Queue<Rigidbody2D> ballQueue;
    private Rigidbody2D nextBall;
    private int ballCount;
    private float countDown;
    private bool ballsSpawned;

    private float timer;


    private void Awake()
    {
        instance = this;
        ballQueue = new Queue<Rigidbody2D>();
    }

    private void Start()
    {
        spawnBall();
        ballCount = 1;
        countDown = 1f;
        timer = 0f;
    }
    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Space))
         //   launchBall();
      //  if (Input.GetKeyDown(KeyCode.B))
        //    SoundManager.PlaySound("Test");
        if (LevelManager.instance.gameOver)
            return;
        if (!ballsSpawned)
        {
            countDown = countDown - Time.deltaTime;
            if (countDown <=0)
            {
                spawnBall();
                ballCount++;
                countDown = 1f;
            }

            if (ballCount == totalBalls)
            {
                ballsSpawned = true;
                countDown = launchFrequency;
            }
        }

        if (ballsSpawned)
        {
            timer += Time.deltaTime;
            if (countDown <= 0)
            {
                launchBall();
                launchFrequency = Mathf.Lerp(startingFrequency, endingFrequency, timer / timeToReachEndingFrequency);
                countDown = launchFrequency;
            }
            else
                countDown = countDown - Time.deltaTime;
        }

    }

    public void spawnBall()
    {
        GameObject newBall = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
        ballQueue.Enqueue(newBall.GetComponent<Rigidbody2D>());
        newBall.GetComponent<BallVisualLogic>().SetRandomBallColor();
        //newBall.GetComponent<BallColorLogic>().SetBallColor(BallColorLogic.BALLCOLOR.BLUE);
        if (nextBall == null)
        {
            nextBall = ballQueue.Dequeue();
            //nextBall.gameObject.layer = LayerMask.NameToLayer("LoadedBall");
        }
    }

    public void launchBall()
    {

        spawnBall();
        nextBall.transform.position = ballLaunchPoint.transform.position;
        nextBall.AddForce(Vector2.up * Random.Range(minLaunchForce, maxLaunchForce));
        nextBall.gameObject.layer = LayerMask.NameToLayer("LaunchingBall");
        nextBall = ballQueue.Dequeue();
        //nextBall.gameObject.layer = LayerMask.NameToLayer("LoadedBall");
        //visual component
        launchVisual.reloadTime = Mathf.Max(0.1f,launchFrequency - 0.8f);
        launchVisual.runLaunchVisual();
        SoundManager.i.PlayAudioSource(launchAudioSource);
    }


}
