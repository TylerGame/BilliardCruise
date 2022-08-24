using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BilliardCruise.Sava.Scripts
{
    public class PierUIComponent : MonoBehaviour
    {
        public TMP_Text txt_currentStar;

        // Start is called before the first frame update
        void Start()
        {
            // txt_currentStar.text = MenuManager.Instance.currentStar.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            txt_currentStar.text = MenuManager.Instance.currentStar.ToString();
        }
    }
}

