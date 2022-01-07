using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.ThunderAndLightning;
using JUtils;

public class BallCollisionLogic : MonoBehaviour
{

    public Rigidbody2D rb;
    public CircleCollider2D col;
   // public HashSet<BallCollisionLogic> currentChain;
    public BallVisualLogic visualLogic;
    public List<BallCollisionLogic> connectedBalls;
    public bool caught;
    public float breakage = 0f;
    private Dictionary<GameObject, GameObject> lightningReferences;
    public LightningSphereMovement lightningSphereMovementScript;
    public Transform ballSpriteTransform;
    public AudioSource audio;

    private void Awake()
    {
        lightningReferences = new Dictionary<GameObject, GameObject>();
        connectedBalls = new List<BallCollisionLogic>();
    }

    public void setBreakagePercent(float percent)
    {
        breakage = percent;
        visualLogic.SetBreakageVisual(percent);
        if (percent >= 1f)
            destroyBallNegativeCause();
    }

    private void spawnLightning(GameObject target)
    {
        if (lightningReferences.ContainsKey(target))
            return;
        GameObject newLightning = Instantiate(GameAssets.i.connectedBallLightningPrefab, Vector3.zero, Quaternion.identity);
        LightningBoltPrefabScript lightningScript = newLightning.GetComponent<LightningBoltPrefabScript>();
        lightningScript.Source = this.gameObject;
        lightningScript.Destination = target;
        lightningScript.GlowTintColor = visualLogic.data.visualColor;
        lightningScript.LightningTintColor = visualLogic.data.visualColor;

        lightningReferences.Add(target, newLightning);
    }

    private void destroyLightning(GameObject target)
    {
        if (!lightningReferences.ContainsKey(target))
            return;
        GameObject lightning = lightningReferences[target];
        lightningReferences.Remove(target);
        Destroy(lightning);
    }

    public void destroyAllConnectedLightning()
    {
        List<GameObject> toDestroy = new List<GameObject>();
        toDestroy.AddRange(lightningReferences.Values);
        
        while (toDestroy.Count > 0)
        {
            GameObject destroying = toDestroy[0];
            toDestroy.RemoveAt(0);
            Destroy(destroying);
        }
        lightningReferences = new Dictionary<GameObject, GameObject>();

    }

    
    private List<BallCollisionLogic> GetCurrentChain()
    {
        List<BallCollisionLogic> visited = new List<BallCollisionLogic>();
        List<BallCollisionLogic> toVisit = new List<BallCollisionLogic>();

        toVisit.Add(this);

        while (toVisit.Count > 0)
        {
            visited.Add(toVisit[0]);
            foreach (BallCollisionLogic b in toVisit[0].connectedBalls)
                if (!visited.Contains(b) && !toVisit.Contains(b) && b.caught)
                    toVisit.Add(b);

            toVisit.RemoveAt(0);
        }

        return visited;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LevelManager.instance.gameOver)
            return;
        BallCollisionLogic otherBall = other.gameObject.GetComponent<BallCollisionLogic>();
        if (otherBall != null)
            if (otherBall.visualLogic.data.colorEnum == visualLogic.data.colorEnum)
            {
                spawnLightning(other.gameObject);
                connectedBalls.Add(otherBall);
            }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if (LevelManager.instance.gameOver)
       //     return;
        BallCollisionLogic otherBall = other.gameObject.GetComponent<BallCollisionLogic>();
        if (otherBall != null)
            if (connectedBalls.Contains(otherBall))
            {
                destroyLightning(other.gameObject);
                connectedBalls.Remove(otherBall);
            }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") || collision.gameObject.layer == LayerMask.NameToLayer("CaughtBall"))
        if (collision.transform.GetComponentInParent<PlatformController>() != null && !caught)
        {
            catchBall();
            //fixes the scaling
            ballSpriteTransform.localEulerAngles = transform.localEulerAngles;
            transform.SetParent(collision.transform, true);
            if (transform.parent == PlatformController.instance.transform)
                transform.localScale = new Vector3(0.399f, 0.638f, 1f);
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);

            //transform.SetParent(PlatformController.platform.transform, true);
        }
    }

    private void catchBall()
    {
        caught = true;
        rb.isKinematic = true;
        //rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;

        gameObject.layer = LayerMask.NameToLayer("CaughtBall");
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlatformController.instance.updateEntireBounds();

        List<BallCollisionLogic> currentChain = GetCurrentChain();
        if (currentChain.Count >= 4)
            foreach (BallCollisionLogic b in currentChain)
                b.destroyBallPositiveCause();
    }

    //un-catches this ball and all of its children
    public void releaseBall()
    {
        caught = false;
        transform.parent = null;
        rb.isKinematic = false;
        if (gameObject.layer == LayerMask.NameToLayer("CaughtBall"))
            gameObject.layer = LayerMask.NameToLayer("LaunchedBall");
        rb.constraints = RigidbodyConstraints2D.None;

        foreach (BallCollisionLogic ball in GetComponentsInChildren<BallCollisionLogic>())
        {
            if (ball!=this)
                ball.releaseBall();
        }
        /*
        foreach (Transform child in transform)
        {
            BallCollisionLogic childLogic = child.GetComponent<BallCollisionLogic>();
            if (childLogic != null)
                childLogic.releaseBall();
        }
        */
        setBreakagePercent(0f);
        PlatformController.instance.updateEntireBounds();
    }

    public void destroyBallPositiveCause()
    {
        StartCoroutine(destroyBallReleaseLightning(true));
        ScoreManager.instance.addScore(1);
        SoundManager.i.PlayAudioSource(audio);
    }

    public void destroyBallNegativeCause()
    {
        StartCoroutine(destroyBallReleaseLightning(false));
        SoundManager.i.PlayAudioSource(audio);
    }


    IEnumerator destroyBallReleaseLightning(bool good)
    {
        releaseBall();

        destroyAllConnectedLightning();
        if (good)
        {
            this.gameObject.layer = LayerMask.NameToLayer("LightningSphereGood");
            lightningSphereMovementScript.moveTicks = 1;
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("LightningSphereBad");
            lightningSphereMovementScript.moveTicks = -1;
        }
        visualLogic.lightning.enabled = true;
        visualLogic.breakBallVisual();
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        rb.isKinematic = false;
        rb.gravityScale = 0f;
        //LightningSphereMovement.spawnLightningSphere(transform.position, -1, colorLogic.data.visualColor, JUtilsClass.closestTransformToPoint(transform.position, PitDestruction.instance.downLeft, PitDestruction.instance.downRight));
    }

}
