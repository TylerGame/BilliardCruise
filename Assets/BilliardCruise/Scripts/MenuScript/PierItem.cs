using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BilliardCruise.Sava.Scripts
{
    public class PierItem : MonoBehaviour
    {
        public int order;
        public Transform container;
        public GameObject yesButton;
        public TMP_Text t_stars;
        public int neededStarCount;
        // Start is called before the first frame update
        void Start()
        {
            t_stars.text = neededStarCount.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick_YesButton()
        {
            GameObject[] piers = GameObject.FindGameObjectsWithTag("PierItem");
            switch (piers.Length)
            {
                case 0:

                    break;
                case 1:
                    MenuManager.Instance.pierYesButton = yesButton;
                    MenuManager.Instance.pierCurOption = gameObject;
                    MenuManager.Instance.pierWay = 0;
                    break;
                case 2:
                    MenuManager.Instance.pierYesButton = yesButton;
                    MenuManager.Instance.pierCurOption = gameObject;
                    MenuManager.Instance.pierWay = order;
                    break;
            }

            MenuManager.Instance.startPierStarEffect(neededStarCount);
        }
    }
}

