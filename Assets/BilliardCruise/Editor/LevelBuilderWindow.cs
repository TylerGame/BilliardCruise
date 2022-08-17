using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace BilliardCruise.Sava.Scripts
{
    public class LevelBuilderWindow : EditorWindow
    {
        private static LevelBuilderWindow Instance;
        private GUISkin skin;

        private GameData gameData;
        static GameObject gameLogicObjects;
        static GameObject gameUI;
        static GameObject poolManager_Local;
        static GameObject tableObjects;

        static GameManager gameManager;
        static List<GameObject> balls;
        static List<BallColor> ballColors;
        private Vector2 viewScrollPosition;
        private Vector2 viewScrollPosition1;

        static List<Object> monsters;
        static List<GameObject> monstersFromGame;
        static List<GameObject> fishFromGame;
        static List<GameObject> boxFromGame;
        static List<GameObject> sharkFromGame;
        static List<GameObject> batteryFromGame;
        static List<GameObject> octupusFromGame;

        static GameObject table;
        static GameObject pocket_parent;



        static List<GameObject> pocket_list;
        enum BallColor
        {
            White,
            Red,
            Blue,
            Green,
            Yellow,
            Brown,
            Orange,
            Cyan
        }


        [MenuItem("BilliardCruise/NewLevel", false)]
        public static void CreateNewLevel()
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            gameLogicObjects = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/BilliardCruise/Resources/GameLogicObjects.prefab"));
            gameLogicObjects.name = "GameLogicObjects";
            gameUI = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/BilliardCruise/Resources/GameUI.prefab"));
            gameUI.name = "GameUI";
            poolManager_Local = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/BilliardCruise/Resources/PoolManager_Local.prefab"));
            poolManager_Local.name = "PoolManager_Local";

            tableObjects = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/BilliardCruise/Resources/TableObjects.prefab"));
            tableObjects.name = "TableObjects";

            GameObject white_ball = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/BilliardCruise/Resources/Ball.prefab"));
            white_ball.name = "Ball-White";
            white_ball.GetComponent<Ball_Local>().BallNumber = 0;
            white_ball.GetComponent<Ball_Local>().BallTexture = Resources.Load("BallTex/Ball0Tex") as Texture;
            // balls.Add(white_ball);

            GameObject directionLight = new GameObject("Directional Light");
            directionLight.AddComponent<Light>();
            directionLight.GetComponent<Light>().type = LightType.Directional;
            directionLight.GetComponent<Light>().color = Color.white;
            directionLight.transform.rotation = Quaternion.Euler(120f, 40f, 0f);
            directionLight.transform.position = Vector3.up * 5f;
            RenderSettings.ambientLight = Color.white;
            OpenLevelBuilder();
        }
        [MenuItem("BilliardCruise/LevelBuilder", false)]
        public static void OpenLevelBuilder()
        {
            var window = (LevelBuilderWindow)GetWindow(typeof(LevelBuilderWindow), false, "Level Builder");
            window.minSize = new Vector2(1280, 800);
            window.maxSize = new Vector2(1208, 800);

            GUI.contentColor = Color.white;
            window.Show();
            Instance = window;
        }


        private void OnEnable()
        {
            Instance = this;
            skin = Resources.Load<GUISkin>("GUIStyle/LevelEditor");
            gameData = Resources.Load<GameData>("Games/GameData");
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            balls = GameObject.FindGameObjectsWithTag("Ball").ToList();
            ballColors = new List<BallColor>();

            if (gameData.levels[gameManager.level].boosters.Count != 3)
            {
                gameData.levels[gameManager.level].boosters.Clear();
                gameData.levels[gameManager.level].boosters.Add(new Booster());
                gameData.levels[gameManager.level].boosters.Add(new Booster());
                gameData.levels[gameManager.level].boosters.Add(new Booster());
            }
            for (int i = 0; i < balls.Count - 1; i++)
            {
                for (int j = i; j < balls.Count; j++)
                {
                    if (balls[i].GetComponent<Ball_Local>().BallNumber > balls[j].GetComponent<Ball_Local>().BallNumber)
                    {
                        GameObject temp = balls[i];
                        balls[i] = balls[j];
                        balls[j] = temp;
                    }
                }
            }
            foreach (GameObject b in balls)
            {
                ballColors.Add(getBallColor(b.GetComponent<Ball_Local>().BallTexture.name));
            }
            monsters = Resources.LoadAll("Monsters/").ToList();
            monstersFromGame = GameObject.FindGameObjectsWithTag("Monster").ToList();
            batteryFromGame = new List<GameObject>();
            boxFromGame = new List<GameObject>();
            fishFromGame = new List<GameObject>();
            octupusFromGame = new List<GameObject>();
            sharkFromGame = new List<GameObject>();
            foreach (GameObject o in monstersFromGame)
            {
                switch (o.GetComponent<Monster>().monsterType)
                {
                    case Monster.MonsterType.Battery:
                        batteryFromGame.Add(o);
                        break;
                    case Monster.MonsterType.Box:
                        boxFromGame.Add(o);
                        break;
                    case Monster.MonsterType.Fish:
                        fishFromGame.Add(o);
                        break;
                    case Monster.MonsterType.Octopus:
                        octupusFromGame.Add(o);
                        break;
                    case Monster.MonsterType.Shark:
                        sharkFromGame.Add(o);
                        break;
                }
            }
            table = GameObject.Find("table");
            pocket_parent = GameObject.Find("Pocket");
            pocket_list = GameObject.FindGameObjectsWithTag("PocketHole").ToList();
        }


        BallColor getBallColor(string texName)
        {
            BallColor ret = BallColor.White;
            switch (texName)
            {
                case "Ball0Tex":
                    ret = BallColor.White;
                    break;
                case "Ball3Tex":
                    ret = BallColor.Red;
                    break;
                case "Ball2Tex":
                    ret = BallColor.Blue;
                    break;
                case "Ball7Tex":
                    ret = BallColor.Brown;
                    break;
                case "Ball4Tex":
                    ret = BallColor.Cyan;
                    break;
                case "Ball6Tex":
                    ret = BallColor.Green;
                    break;
                case "Ball5Tex":
                    ret = BallColor.Orange;
                    break;
                case "Ball1Tex":
                    ret = BallColor.Yellow;
                    break;
            }

            return ret;

        }
        private void OnInspectorUpdate()
        {
            // Repaint();
        }


        private void OnGUI()
        {
            Repaint();
            DrawLevelInfo();
            DrawBoosterInfo();
            DrawBallInfo();
            DrawMonstersInfo();
            DrawPoolInfo();
            DrawButtons();
        }

        private void DrawLevelInfo()
        {
            GUILayout.BeginArea(new Rect(50, 0, 500, 500));

            GUILayout.Space(10);
            GUILayout.Label("Level Info", skin.GetStyle("ViewTitle"), GUILayout.Width(450), GUILayout.Height(40));
            GUILayout.Space(10);

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            GUILayout.Label("level: ", GUILayout.Width(100), GUILayout.Height(15));
            int.TryParse(GUILayout.TextField(gameManager.level.ToString(), GUILayout.Width(100), GUILayout.Height(15)), out gameManager.level);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Moves: ", GUILayout.Width(100), GUILayout.Height(15));

            int.TryParse(GUILayout.TextField(gameData.levels[gameManager.level].moves.ToString(), GUILayout.Width(100), GUILayout.Height(15)), out gameData.levels[gameManager.level].moves);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Goal: ", GUILayout.Width(100), GUILayout.Height(15));

            int.TryParse(GUILayout.TextField(gameData.levels[gameManager.level].goal.ToString(), GUILayout.Width(100), GUILayout.Height(15)), out gameData.levels[gameManager.level].goal);

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndArea();
        }

        void DrawBoosterInfo()
        {
            GUILayout.BeginArea(new Rect(50, 100, 500, 400));
            GUILayout.Space(10);
            GUILayout.Label("Booster Info", skin.GetStyle("ViewTitle"), GUILayout.Width(450), GUILayout.Height(40));
            // GUILayout.Space(10);
            // if (gameData.levels[gameManager.level].boosters.Count < 3 && GUILayout.Button("+ Add Booster", skin.GetStyle("AddButton"), GUILayout.Width(440), GUILayout.Height(25)))
            //     gameData.levels[gameManager.level].boosters.Add(new Booster());
            for (var i = 0; i < gameData.levels[gameManager.level].boosters.Count; i++)
            {
                GUILayout.Space(10);
                var disNumber = i + 1;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("" + disNumber + ":", GUILayout.Width(25));
                gameData.levels[gameManager.level].boosters[i].booster = (Booster.BoosterType)EditorGUILayout.EnumPopup(gameData.levels[gameManager.level].boosters[i].booster, GUILayout.Width(250));
                switch (gameData.levels[gameManager.level].boosters[i].booster)
                {
                    case Booster.BoosterType.Arrow:
                        gameData.levels[gameManager.level].boosters[i].active_icon = Resources.Load<Sprite>("BoosterTex/arrowpan") as Sprite;
                        gameData.levels[gameManager.level].boosters[i].deactive_icon = Resources.Load<Sprite>("BoosterTex/arrowpan-gray") as Sprite;
                        break;
                    case Booster.BoosterType.Dice:
                        gameData.levels[gameManager.level].boosters[i].active_icon = Resources.Load<Sprite>("BoosterTex/dicepan") as Sprite;
                        gameData.levels[gameManager.level].boosters[i].deactive_icon = Resources.Load<Sprite>("BoosterTex/dicepan-gray") as Sprite;
                        break;
                    case Booster.BoosterType.Muscle:
                        gameData.levels[gameManager.level].boosters[i].active_icon = Resources.Load<Sprite>("BoosterTex/strongarmpan") as Sprite;
                        gameData.levels[gameManager.level].boosters[i].deactive_icon = Resources.Load<Sprite>("BoosterTex/strongarmpan-gray") as Sprite;
                        break;
                    case Booster.BoosterType.Eye:
                        gameData.levels[gameManager.level].boosters[i].active_icon = Resources.Load<Sprite>("BoosterTex/eye") as Sprite;
                        gameData.levels[gameManager.level].boosters[i].deactive_icon = Resources.Load<Sprite>("BoosterTex/eye-gray") as Sprite;
                        break;
                }
                int.TryParse(GUILayout.TextField(gameData.levels[gameManager.level].boosters[i].count.ToString(), GUILayout.Width(100), GUILayout.Height(15)), out gameData.levels[gameManager.level].boosters[i].count);
                GUILayout.Space(10);
                // if (GUILayout.Button("X", skin.GetStyle("RemoveButton"), GUILayout.Width(20), GUILayout.Height(20)))
                // {
                //     gameData.levels[gameManager.level].boosters.RemoveAt(i);
                // }


                EditorGUILayout.EndHorizontal();
            }



            GUILayout.EndArea();
        }

        void DrawBallInfo()
        {
            GUILayout.BeginArea(new Rect(50, 230, 500, 200));

            GUILayout.Space(10);
            GUILayout.Label("Ball Info", skin.GetStyle("ViewTitle"), GUILayout.Width(450), GUILayout.Height(40));

            if (GUILayout.Button("+ Add Ball", skin.GetStyle("AddBall"), GUILayout.Width(440), GUILayout.Height(25)))
            {
                GameObject ball = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/BilliardCruise/Resources/Ball.prefab"));
                ball.GetComponent<Ball_Local>().BallNumber = balls.Count;
                ball.transform.position = new Vector3(Random.Range(-3f, 3f), 0.4f, Random.Range(-7f, 7f));
                if (ball.GetComponent<Ball_Local>().BallNumber == 0)
                {
                    ball.name = "Ball_White";

                    ballColors.Add(BallColor.White);
                }
                else
                {
                    ball.name = "Ball-" + ball.GetComponent<Ball_Local>().BallNumber;
                    BallColor bcolor = (BallColor)Random.Range(1, 8);
                    ballColors.Add(bcolor);
                }
                balls.Add(ball);
            }

            viewScrollPosition = GUILayout.BeginScrollView(viewScrollPosition);

            for (int i = 0; i < balls.Count; i++)
            {
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Ball Number : " + balls[i].GetComponent<Ball_Local>().BallNumber, GUILayout.Width(150));
                if (balls[i].GetComponent<Ball_Local>().BallNumber == 0)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    ballColors[i] = (BallColor)EditorGUILayout.EnumPopup(ballColors[i], GUILayout.Width(250));
                    DrawBall(i, ballColors[i]);
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    ballColors[i] = (BallColor)EditorGUILayout.EnumPopup(ballColors[i], GUILayout.Width(250));
                    DrawBall(i, ballColors[i]);
                    if (GUILayout.Button("X", skin.GetStyle("RemoveButton"), GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        DestroyImmediate(balls[i]);
                        balls.RemoveAt(i);
                        ballColors.RemoveAt(i);
                        SortBallNumber();
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }


        void SortBallNumber()
        {
            int index = 1;
            foreach (GameObject b in balls)
            {
                if (b.GetComponent<Ball_Local>().BallNumber != 0)
                {
                    b.GetComponent<Ball_Local>().BallNumber = index;
                    b.name = "Ball-" + index;
                    index++;
                }
            }

            ballColors.Clear();
            foreach (GameObject b in balls)
            {
                ballColors.Add(getBallColor(b.GetComponent<Ball_Local>().BallTexture.name));
            }
        }
        void DrawBall(int i, BallColor color)
        {
            Texture tx = null;
            switch (color)
            {
                case BallColor.White:
                    tx = Resources.Load("BallTex/Ball0Tex") as Texture;

                    break;
                case BallColor.Red:
                    tx = Resources.Load("BallTex/Ball3Tex") as Texture;

                    break;
                case BallColor.Blue:
                    tx = Resources.Load("BallTex/Ball2Tex") as Texture;

                    break;
                case BallColor.Brown:
                    tx = Resources.Load("BallTex/Ball7Tex") as Texture;

                    break;
                case BallColor.Cyan:
                    tx = Resources.Load("BallTex/Ball4Tex") as Texture;

                    break;
                case BallColor.Green:
                    tx = Resources.Load("BallTex/Ball6Tex") as Texture;

                    break;
                case BallColor.Orange:
                    tx = Resources.Load("BallTex/Ball5Tex") as Texture;

                    break;
                case BallColor.Yellow:
                    tx = Resources.Load("BallTex/Ball1Tex") as Texture;

                    break;
            }

            balls[i].GetComponent<MeshRenderer>().material.mainTexture = tx;
            balls[i].GetComponent<Ball_Local>().BallTexture = tx;
        }

        void DrawMonstersInfo()
        {
            GUILayout.BeginArea(new Rect(50, 430, 500, 500));

            GUILayout.Space(10);
            GUILayout.Label("Monsters & Obstacles", skin.GetStyle("ViewTitle"), GUILayout.Width(450), GUILayout.Height(40));
            viewScrollPosition1 = GUILayout.BeginScrollView(viewScrollPosition1);
            foreach (Object o in monsters)
            {
                EditorGUILayout.BeginHorizontal();
                var cont = new GUIContent();
                cont.image = Resources.Load("MonsterTex/" + o.name) as Texture;
                cont.text = o.name;
                GUILayout.Button(cont, skin.GetStyle("ModuleButton"), GUILayout.Width(100), GUILayout.Height(50));
                // EditorGUILayout.LabelField(o.name, GUILayout.Width(100));
                if (GUILayout.Button("-", skin.GetStyle("RemoveButton"), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    switch (o.name)
                    {
                        case "Fish":
                            if (fishFromGame.Count != 0)
                            {

                                DestroyImmediate(fishFromGame[fishFromGame.Count - 1]);
                                fishFromGame.RemoveAt(fishFromGame.Count - 1);
                            }

                            break;
                        case "Box":
                            if (boxFromGame.Count != 0)
                            {
                                DestroyImmediate(boxFromGame[boxFromGame.Count - 1]);
                                boxFromGame.RemoveAt(boxFromGame.Count - 1);
                            }

                            break;
                        case "Shark":
                            if (sharkFromGame.Count != 0)
                            {
                                DestroyImmediate(sharkFromGame[sharkFromGame.Count - 1]);
                                sharkFromGame.RemoveAt(sharkFromGame.Count - 1);
                            }

                            break;
                        case "Battery":
                            if (batteryFromGame.Count != 0)
                            {
                                DestroyImmediate(batteryFromGame[batteryFromGame.Count - 1]);
                                batteryFromGame.RemoveAt(batteryFromGame.Count - 1);
                            }

                            break;
                        case "Octupus":
                            if (octupusFromGame.Count != 0)
                            {
                                DestroyImmediate(octupusFromGame[octupusFromGame.Count - 1]);
                                octupusFromGame.RemoveAt(octupusFromGame.Count - 1);
                            }

                            break;
                    }
                }
                int count = 0;
                switch (o.name)
                {
                    case "Fish":
                        count = fishFromGame.Count;
                        break;
                    case "Box":
                        count = boxFromGame.Count;
                        break;
                    case "Shark":
                        count = sharkFromGame.Count;
                        break;
                    case "Battery":
                        count = batteryFromGame.Count;
                        break;
                    case "Octupus":
                        count = octupusFromGame.Count;
                        break;
                }
                EditorGUILayout.LabelField(count.ToString(), GUILayout.Width(50));
                if (GUILayout.Button("+", skin.GetStyle("RemoveButton"), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    GameObject monster = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/BilliardCruise/Resources/Monsters/" + o.name + ".prefab"));
                    monster.transform.position = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-7f, 7f));
                    switch (o.name)
                    {
                        case "Fish":
                            fishFromGame.Add(monster);
                            monster.name = "Fish-" + fishFromGame.Count;
                            break;
                        case "Box":
                            boxFromGame.Add(monster);
                            monster.name = "Box-" + boxFromGame.Count;
                            break;
                        case "Shark":
                            sharkFromGame.Add(monster);
                            monster.name = "Shark-" + sharkFromGame.Count;
                            break;
                        case "Battery":
                            batteryFromGame.Add(monster);
                            monster.name = "Battery-" + batteryFromGame.Count;
                            break;
                        case "Octupus":
                            octupusFromGame.Add(monster);
                            monster.name = "Octupus-" + octupusFromGame.Count;
                            break;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        void DrawPoolInfo()
        {
            GUILayout.BeginArea(new Rect(500, 0, 500, 500));
            GUILayout.Space(10);
            GUILayout.Label("Table Info", skin.GetStyle("ViewTitle"), GUILayout.Width(450), GUILayout.Height(40));
            GUILayout.BeginHorizontal();
            table.GetComponent<SpriteRenderer>().sprite = (Sprite)EditorGUILayout.ObjectField(table.GetComponent<SpriteRenderer>().sprite, typeof(Sprite), false, GUILayout.Width(100), GUILayout.Height(160));
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Pockets", GUILayout.Width(150));
            if (GUILayout.Button("-", skin.GetStyle("RemoveButton"), GUILayout.Width(20), GUILayout.Height(20)))
            {
                if (pocket_list.Count != 0)
                {
                    DestroyImmediate(pocket_list[pocket_list.Count - 1]);
                    pocket_list.RemoveAt(pocket_list.Count - 1);
                }
            }
            EditorGUILayout.LabelField(pocket_list.Count.ToString(), GUILayout.Width(150));
            if (GUILayout.Button("+", skin.GetStyle("RemoveButton"), GUILayout.Width(20), GUILayout.Height(20)))
            {
                GameObject pocketHole = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/BilliardCruise/Resources/PocketHole.prefab"));
                pocketHole.transform.SetParent(pocket_parent.transform);
                pocketHole.transform.position = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-7f, 7f));
                pocketHole.name = "PocketHole-" + pocket_list.Count;
                pocket_list.Add(pocketHole);
            }


            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        void DrawButtons()
        {
            GUILayout.BeginArea(new Rect(1000, 700, 500, 200));
            GUILayout.Space(10);
            if (GUILayout.Button("Save", skin.GetStyle("AddBall"), GUILayout.Width(200), GUILayout.Height(50)))
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/BilliardCruise/Scenes/Level0" + gameManager.level.ToString() + ".unity");
            }
            GUILayout.EndArea();
        }

        void OnDisable()
        {

        }

        private void OnLostFocus()
        {

        }
    }

}