using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace BilliardCruise.Sava.Scripts
{
    public class WinPopupUI : MonoBehaviour
    {
        public GameObject coin;
        public GameObject star;
        public GameObject key;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartEffect()
        {
            coin.transform.localPosition = Vector3.one * 1.5f;
            star.transform.localPosition = Vector3.one * 1.5f;
            key.transform.localPosition = Vector3.one * 1.5f;


            coin.transform.DOScale(Vector3.one, 0.5f);
            star.transform.DOScale(Vector3.one, 0.5f);
            key.transform.DOScale(Vector3.one, 0.5f);
        }
    }
}

