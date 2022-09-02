using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BilliardCruise.Sava.Scripts
{
    [Serializable]
    public class UndoPocketInfo
    {
        public bool isClosed;
    }
    public class Pocket : MonoBehaviour
    {

        public PointGroup path;
        public GameObject cork;
        public float ballVelInPocket = 0.5f;
        public float ballAngVelInPocket = 10;
        public float ballVelInCollector = 0.5f;
        public float ballAngVelInCollector = 10;

        bool isClosed = false;
        List<UndoPocketInfo> undos = new List<UndoPocketInfo>();
        void Start()
        {
            cork.SetActive(false);
            isClosed = false;
            SaveUndo();
        }

        public void SaveUndo()
        {
            UndoPocketInfo undo = new UndoPocketInfo();
            undo.isClosed = isClosed;
            undos.Add(undo);
        }

        public void DoUndo()
        {
            if (undos.Count >= 2)
            {
                UndoPocketInfo undo = undos[undos.Count - 2];
                isClosed = undo.isClosed;
                if (!isClosed)
                {
                    GetComponent<Collider>().enabled = true;
                    cork.GetComponent<Animator>().enabled = false;
                    cork.SetActive(false);
                }
                undos.RemoveAt(undos.Count - 1);
                undos.RemoveAt(undos.Count - 1);
                SaveUndo();
            }
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
            cork.GetComponent<Animator>().enabled = true;
            cork.GetComponent<Animator>().SetTrigger("Cork");
            GetComponent<Collider>().enabled = false;
        }
    }
}

