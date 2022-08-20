using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BilliardCruise.Sava.Scripts
{
    public class Ball : MonoBehaviour
    {

        [SerializeField] protected int ballNumber;
        [SerializeField] protected float stopThreshold;
        [SerializeField] protected float stopThresholdAngular;
        [SerializeField] protected float railDamping;
        [SerializeField] protected float maxAngularVelocity;

        [SerializeField] protected Texture ballTexture;

        [SerializeField] protected AudioClip ballCollisionSound;
        [SerializeField] protected AudioClip railCollisionSound;
        [SerializeField] protected AudioClip pocketSound;
        [SerializeField] protected AudioClip ballCollectorSound;

        protected PoolManager poolManager;
        protected Rigidbody rb;
        protected MeshRenderer meshRenderer;
        protected AudioSource audioSrc;

        protected BallCollector ballCollector;

        public BoosterEffectManager boosterEffectManager;
        public GameObject prefabOfBoosterEffect;
        public GameObject prefabOfUtilsforEffects;

        public float Radius
        {
            get;
            private set;
        }

        public int BallNumber
        {
            get
            {
                return ballNumber;
            }

            set
            {
                ballNumber = value;
            }
        }


        public Texture BallTexture
        {
            get
            {
                return ballTexture;
            }
            set
            {
                ballTexture = value;
            }
        }

        public bool IsAtRest
        {
            get
            {
                return rb.velocity.magnitude <= stopThreshold &&
                rb.angularVelocity.magnitude <= stopThresholdAngular;
            }
        }

        public bool IsPocketed
        {
            get
            {
                return poolManager.IsBallPocketed(this);
            }
        }

        public BallType Type
        {
            get
            {
                if (BallNumber == 0)
                {
                    return BallType.CUE;
                }

                if (BallNumber == 8)
                {
                    return BallType.EIGHT;
                }

                if (BallNumber >= 1 && BallNumber <= 7)
                {
                    return BallType.SOLID;
                }

                if (BallNumber >= 9 && BallNumber <= 15)
                {
                    return BallType.STRIPED;
                }

                return BallType.UNKNOWN;
            }
        }

        protected virtual void Awake()
        {
            poolManager = GameObject.FindObjectOfType<PoolManager>();

            rb = GetComponent<Rigidbody>();
            rb.maxAngularVelocity = maxAngularVelocity;

            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.mainTexture = ballTexture;

            audioSrc = GetComponent<AudioSource>();

            Radius = GetComponent<Collider>().bounds.size.x / 2.0f;

            ballCollector = GameObject.FindObjectOfType<BallCollector>();




        }


        protected virtual void Start()
        {
            GameObject obj = Instantiate(prefabOfBoosterEffect);
            boosterEffectManager = obj.GetComponent<BoosterEffectManager>();
            boosterEffectManager.owner = gameObject;
        }

        protected virtual void Update()
        {
            if (rb.velocity.y > 0)
            {
                Vector3 velocity = rb.velocity;
                velocity.y = 0;
                rb.velocity = velocity;
            }

            if (rb.velocity.magnitude <= stopThreshold)
            {
                rb.velocity = Vector3.zero;

            }

            if (rb.angularVelocity.magnitude <= stopThresholdAngular)
            {
                rb.angularVelocity = Vector3.zero;
            }

            if (!boosterEffectManager.isState && !IsAtRest)
            {
                if (GameManager.Instance.isTriggerArrowEffect)
                {

                }
                if (GameManager.Instance.isTriggerDiceEffect)
                {

                }
                if (GameManager.Instance.isTriggerEyeEffect)
                {
                    boosterEffectManager.SwitchInvisibleEffect(true);
                }
                if (GameManager.Instance.isTriggerStrengthEffect)
                {
                    boosterEffectManager.SwitchStrengthEffect(true);
                }
            }
        }

        protected void HandleCollisionEnter(Collision col, bool isServer)
        {
            if (col.collider.CompareTag("Rail") || col.collider.CompareTag("Monster"))
            {
                PlayRailCollisionSound();

                if (isServer)
                {
                    rb.velocity *= railDamping;

                    poolManager.CurrentTurn.AddToCushionedBalls(this);
                    if (poolManager.CurrentTurn.BallsHitByCueBall.Count > 0)
                    {
                        poolManager.CurrentTurn.BallsHitRailsAfterContact = true;
                    }
                }

                if (col.collider.CompareTag("Monster") && GameManager.Instance.isTriggerStrengthEffect)
                {
                    GameObject uEffects = Instantiate(prefabOfUtilsforEffects, col.collider.gameObject.transform.position, Quaternion.Euler(90f, 0f, 0f));
                    uEffects.GetComponent<UtilsForEffects>().UseCrackEffect();
                }
            }

            if (col.collider.CompareTag("Ball"))
            {
                PlayBallCollisionSound();
                if (isServer)
                {
                    if (BallNumber == 0)
                    {
                        Ball otherBall = col.collider.GetComponent<Ball>();
                        if (otherBall != null)
                        {
                            if (poolManager.CurrentTurn != null)
                            {
                                poolManager.CurrentTurn.AddToBallsHitByCueBall(otherBall);
                            }
                        }
                    }
                }
            }
        }

        protected void HandleTriggerEnter(Collider col, bool isServer)
        {
            if (col.CompareTag("Pocket"))
            {
                Pocket pocket = col.GetComponent<Pocket>();
                PlayPocketSound();
                pocket.CoverPocketWithCork();
                // if (isServer)
                // {
                if (ballNumber != 0)
                    GameManager.Instance.UpdateGoal();
                poolManager.CurrentTurn.AddToPocketedBalls(this);
                boosterEffectManager.SwitchInvisibleEffect(false);
                boosterEffectManager.SwitchStrengthEffect(false);
                StartCoroutine(PocketCo(pocket));
                // }
            }
            else if (col.CompareTag("BottleNeck"))
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Collider>().enabled = false;
                col.gameObject.transform.parent.GetComponent<Monster>().ballIn = gameObject;
                col.gameObject.GetComponent<Collider>().isTrigger = false;
                StartCoroutine(iBottleNeckCo(col.gameObject));
            }
        }

        IEnumerator iBottleNeckCo(GameObject tar)
        {
            yield return null;
            Debug.Log(tar.name);
            tar.GetComponent<Collider>().enabled = false;
            tar.transform.parent.gameObject.GetComponent<Collider>().enabled = false;
            yield return new WaitForEndOfFrame();
            transform.position = new Vector3(tar.transform.parent.position.x, transform.position.y, tar.transform.parent.position.z);


            if (BallNumber == 0)
            {
                transform.position = new Vector3(1000, 1000, 1000);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                GameUI.Instance.ShowLoosePopup();
                yield break;
            }

            tar.GetComponent<Collider>().enabled = true;
            tar.transform.parent.gameObject.GetComponent<Collider>().enabled = true;
            // while (Vector3.Distance(transform.position, tar.transform.position) > 0.001f)
            // {
            //     Vector3.MoveTowards(transform.position, tar.transform.position, Time.deltaTime);
            //     yield return null;
            // }

        }

        public override string ToString()
        {
            return BallNumber.ToString();
        }

        public void Stop()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        protected IEnumerator PocketCo(Pocket pocket)
        {
            SetLayer(0);
            GetComponent<SphereCollider>().enabled = false;
            SetGravity(false);
            float speed = rb.velocity.magnitude;
            Stop();
            yield return null;

            // transform.position = pocket.transform.world;
            // while (transform.localScale.x > 0.2f)
            // {
            //     // Vector3 direction = (currentNode.position - transform.position).normalized;
            //     // rb.velocity = direction * pocket.ballVelInPocket;
            //     // rb.angularVelocity = rb.angularVelocity.normalized * pocket.ballAngVelInPocket;
            //     transform.localScale *= Mathf.Clamp(speed * 30, 0.3f, 0.9f);
            //     yield return new WaitForSeconds(0.01f);
            // }


            for (int i = 0; i < pocket.path.nodes.Count; i++)
            {
                Transform currentNode = pocket.path.nodes[i];
                Vector3 dir = (currentNode.position - transform.position).normalized;
                transform.position = currentNode.position;
                while (transform.localScale.x > 0.2f)
                {
                    // Vector3 direction = (currentNode.position - transform.position).normalized;
                    // rb.velocity = direction * pocket.ballVelInPocket;
                    // rb.angularVelocity = rb.angularVelocity.normalized * pocket.ballAngVelInPocket;
                    transform.localScale *= Mathf.Clamp(speed * 10, 0.3f, 0.9f);
                    yield return new WaitForSeconds(0.01f);
                }
            }

            if (BallNumber == 0)
            {
                transform.position = new Vector3(1000, 1000, 1000);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                if (!GameManager.Instance.isGameEnding)
                {
                    GameManager.Instance.isGameEnding = true;
                    GameUI.Instance.ShowLoosePopup();
                }
                yield break;
            }


            // GameManager.RemoveBall(ballNumber);
            // poolManager.UpdateBalls();
            // Destroy(gameObject);
            gameObject.SetActive(false);

            // yield break;


            // StartCollectorSound();

            // transform.position = poolManager.BallCollectorNodes[0].position;
            // transform.up = Vector3.left;

            // for (int i = 1; i < poolManager.BallCollectorNodes.Count; i++)
            // {
            //     Transform currentNode = poolManager.BallCollectorNodes[i];

            //     while (Vector3.Distance(currentNode.position, transform.position) > 0.01f)
            //     {
            //         Vector3 direction = (currentNode.position - transform.position).normalized;
            //         rb.velocity = direction * pocket.ballVelInCollector;
            //         rb.angularVelocity = Vector3.left * pocket.ballAngVelInCollector;

            //         yield return new WaitForEndOfFrame();
            //     }

            //     transform.position = currentNode.position;
            // }

            // Stop();

            // Sleep();

            // PlayBallReachedSound();

            // Transform lastNode = poolManager.BallCollectorNodes[poolManager.BallCollectorNodes.Count - 1];
            // Vector3 lastNodePos = lastNode.position;

            // float deltaZ = Radius * 2;
            // deltaZ = deltaZ + (deltaZ * 0.05f);

            // lastNodePos.z += deltaZ;
            // lastNode.position = lastNodePos;

            // StopCollectorSound();
        }

        protected void PlayBallCollisionSound()
        {
            audioSrc.volume = 1.0f;
            audioSrc.clip = ballCollisionSound;
            audioSrc.Play();
        }

        protected void PlayRailCollisionSound()
        {
            audioSrc.volume = 0.5f;
            audioSrc.clip = railCollisionSound;
            audioSrc.Play();
        }

        protected void PlayPocketSound()
        {
            audioSrc.volume = 1.0f;
            audioSrc.clip = pocketSound;
            audioSrc.Play();
        }

        protected virtual void PlayBallReachedSound()
        {
            PlayBallCollisionSound();
        }

        protected virtual void SetLayer(int layerNumber)
        {
            this.gameObject.layer = layerNumber;
        }

        protected virtual void SetGravity(bool gravityState)
        {
            rb.useGravity = gravityState;
        }

        protected virtual void Sleep()
        {
            rb.isKinematic = true;
            rb.Sleep();
        }

        protected virtual void StartCollectorSound()
        {
            ballCollector.StartCollectorSound();
        }

        protected virtual void StopCollectorSound()
        {
            ballCollector.StopCollectorSound();
        }

    }
}

