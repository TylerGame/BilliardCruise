using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BilliardCruise.Sava.Scripts
{
    public class GameManager : MonoBehaviour
    {


        private const string BALL_PREFIX = "BALL ";

        private static Dictionary<int, Ball> ballsDictionary = new Dictionary<int, Ball>();

        public static int BallsCount
        {
            get
            {
                return ballsDictionary.Count;
            }
        }


        /// <summary>
        /// Billiard Cruise
        /// </summary>

        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }



        public GameData gameData;

        public Animator captain_animator;

        [SerializeField]
        private int booster1, booster2, booster3;

        private int _moves, _goal, _level, _ballCount;

        public bool isTriggerArrowEffect = false;
        public bool isTriggerStrengthEffect = false;
        public bool isTriggerEyeEffect = false;
        public bool isTriggerDiceEffect = false;
        public bool isMoving = false;
        public int moves
        {
            get
            {
                return _moves;
            }

            set
            {
                if (value != gameData.levels[level].moves)
                    GameUI.Instance.DoClickAnim(GameUI.AnimationName.ClickMove);
                _moves = value;

                // if (_moves == 0)
                // {
                //     // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //     GameUI.Instance.ShowLoosePopup();
                // }
            }
        }

        public int goal
        {
            get
            {

                return _goal;
            }
            set
            {
                if (value != 0)
                    GameUI.Instance.DoClickAnim(GameUI.AnimationName.ClickGoal);
                _goal = value;
                // if (_goal == gameData.levels[level].goal)
                // if (_goal == 1)
                // {
                //     GameUI.Instance.ShowRewardPopup();
                // }
            }
        }

        public int level;


        public int ballCount
        {
            get
            {
                return _ballCount;
            }
            set
            {
                _ballCount = value;
            }
        }

        public bool isEndofGame
        {
            get
            {
                return _goal == gameData.levels[level].goal;
            }
        }

        public void ResetBoosters()
        {


            // PoolManager.Instance.CueBall.GetComponent<Ball_Local>().boosterEffectManager.SwitchInvisibleEffect(false);
            // PoolManager.Instance.CueBall.GetComponent<Ball_Local>().boosterEffectManager.SwitchStrengthEffect(false);
            if (isTriggerEyeEffect)
            {
                GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                foreach (GameObject monster in monsters)
                {
                    if (monster.GetComponent<Monster>().monsterType == Monster.MonsterType.Fish || monster.GetComponent<Monster>().monsterType == Monster.MonsterType.Octopus || monster.GetComponent<Monster>().monsterType == Monster.MonsterType.Shark || monster.GetComponent<Monster>().monsterType == Monster.MonsterType.Box)
                    {
                        SpriteRenderer[] sprs = monster.GetComponentsInChildren<SpriteRenderer>();
                        foreach (SpriteRenderer spr in sprs)
                        {
                            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 1f);

                        }

                        Collider[] colliders = monster.GetComponentsInChildren<Collider>();
                        foreach (Collider col in colliders)
                        {
                            col.enabled = true;
                        }
                    }
                }
            }

            isTriggerArrowEffect = false;
            isTriggerDiceEffect = false;
            isTriggerEyeEffect = false;
            isTriggerStrengthEffect = false;

        }

        public static void AddBall(int ballNumber, Ball ball)
        {
            ballsDictionary.Add(ballNumber, ball);

            ball.transform.name = BALL_PREFIX + ballNumber;
        }

        public static void RemoveBall(int ballNumber)
        {
            ballsDictionary.Remove(ballNumber);
        }

        public static Ball GetBall(int ballNumber)
        {
            return ballsDictionary[ballNumber];
        }

        void Awake()
        {

            if (instance != null)
                Destroy(this);
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(this.gameObject);

            moves = gameData.levels[level].moves;
            goal = 0;
            booster1 = gameData.levels[level].boosters[0].count;
            booster2 = gameData.levels[level].boosters[1].count;
            booster3 = gameData.levels[level].boosters[2].count;
        }

        void Start()
        {
            StartCoroutine(iGetBall());
        }

        void Update()
        {

        }

        IEnumerator iGetBall()
        {
            yield return null;
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            ballCount = balls.Length;
        }

        public void UpdateMoves()
        {
            if (moves > 0)
                moves--;
            GameUI.Instance.UpdateTopUI();
        }

        public void UpdateGoal()
        {
            if (goal < gameData.levels[level].goal)
                goal++;
            GameUI.Instance.UpdateTopUI();
        }



    }
}

