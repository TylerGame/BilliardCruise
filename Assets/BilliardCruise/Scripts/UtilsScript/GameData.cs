using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BilliardCruise.Sava.Scripts
{
    [CreateAssetMenu(fileName = "GameData", menuName = "GameData", order = 1)]
    public class GameData : ScriptableObject
    {
        [Header("Boosters for Each Level")]
        public List<BoostersOfLevel> boostersOfGame;

        [Header("Data Collection of Level")]
        public List<LevelData> levels;
    }


    [Serializable]
    public class BoostersOfLevel
    {
        public List<Booster> boosters;
    }


    [Serializable]
    public class Booster
    {
        public enum BoosterType { Arrow, Muscle, Dice, Eye };
        // [Header("Sort of Booster")]
        public BoosterType booster;
        // [Header("Sprite of Active Booster Icon")]
        public Sprite active_icon;
        // [Header("Sprite of Deactivate Booster Icon")]
        public Sprite deactive_icon;
        // [Header("Number of Booster Usage")]
        public int count = 1;
    }

    [Serializable]
    public class LevelData
    {
        //     [Header("Number of Level")]
        public int level = 1;
        // [Header("Number of Moves")]
        public int moves = 10;

        // [Header("Number of Goal")]
        public int goal = 3;
    }


}

