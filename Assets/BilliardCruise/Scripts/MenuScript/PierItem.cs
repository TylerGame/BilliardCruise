using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        }

        void OnEnable()
        {
            t_stars.text = neededStarCount.ToString();
            if (Global.cur_star < 2)
            {
                yesButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                yesButton.GetComponent<Button>().interactable = true;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick_YesButton()
        {

            if (Global.cur_star < 2)
                return;

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

