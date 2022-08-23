using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BilliardCruise.Sava.Scripts
{
    public class PierItem : MonoBehaviour
    {
        public int order;
        public Transform container;
        public GameObject yesButton;
        // Start is called before the first frame update
        void Start()
        {

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
                    MenuManager.Instance.pierWay = 0;
                    break;
                case 2:
                    MenuManager.Instance.pierYesButton = yesButton;
                    MenuManager.Instance.pierWay = order;
                    break;
            }

            MenuManager.Instance.startPierStarEffect();
        }
    }
}

