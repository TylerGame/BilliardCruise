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

                if (_moves == 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
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
                if (_goal == gameData.levels[level].goal)
                {
                    GameUI.Instance.ShowRewardPopup();
                }
            }
        }

        public int level
        {
            get
            {
                return _level;
            }
            set
            {

                _level = value;
            }
        }


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

            level = 0;

            moves = gameData.levels[level].moves;
            goal = 0;
            booster1 = gameData.boostersOfGame[level].boosters[0].count;
            booster2 = gameData.boostersOfGame[level].boosters[1].count;
            booster3 = gameData.boostersOfGame[level].boosters[2].count;
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

