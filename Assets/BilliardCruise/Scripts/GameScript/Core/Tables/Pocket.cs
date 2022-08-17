using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BilliardCruise.Sava.Scripts
{
    public class Pocket : MonoBehaviour
    {

        public PointGroup path;
        public GameObject cork;

        public float ballVelInPocket = 0.5f;
        public float ballAngVelInPocket = 10;
        public float ballVelInCollector = 0.5f;
        public float ballAngVelInCollector = 10;

        void Start()
        {
            cork.SetActive(false);
        }

        public void CoverPocketWithCork()
        {
            if (!cork.activeSelf)
            {
                StartCoroutine(iCork());
            }
        }

        IEnumerator iCork()
        {
            yield return new WaitForSeconds(0.3f);
            cork.SetActive(true);
            cork.GetComponent<Animator>().SetTrigger("Cork");
            GetComponent<Collider>().enabled = false;
        }

    }
}

