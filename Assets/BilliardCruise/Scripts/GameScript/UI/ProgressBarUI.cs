using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BilliardCruise.Sava.Scripts
{
    public class ProgressBarUI : MonoBehaviour
    {

        public Image foreground;
        public TMP_Text t_value;

        public float maxValue;
        public float curValue;
        private float _nextValue;
        public float nextValue
        {
            set
            {
                _nextValue = value;
                StartCoroutine(iProgress());
            }
            get
            {
                return _nextValue;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            t_value.text = Mathf.FloorToInt(curValue).ToString() + "/" + Mathf.FloorToInt(maxValue).ToString();
            foreground.GetComponent<RectTransform>().localScale = new Vector3(curValue / maxValue, foreground.GetComponent<RectTransform>().localScale.y, foreground.GetComponent<RectTransform>().localScale.z);
            nextValue = curValue;
        }

        // Update is called once per frame
        void Update()
        {

        }


        IEnumerator iProgress()
        {
            yield return null;
            if (nextValue > curValue)
            {
                while (nextValue > curValue)
                {
                    curValue += Time.deltaTime * 5f;
                    yield return null;
                    foreground.GetComponent<RectTransform>().localScale = new Vector3(curValue / maxValue, foreground.GetComponent<RectTransform>().localScale.y, foreground.GetComponent<RectTransform>().localScale.z);
                }
            }
            else if (nextValue < curValue)
            {
                while (nextValue < curValue)
                {
                    curValue -= Time.deltaTime * 5f;
                    yield return null;
                    foreground.GetComponent<RectTransform>().localScale = new Vector3(curValue / maxValue, foreground.GetComponent<RectTransform>().localScale.y, foreground.GetComponent<RectTransform>().localScale.z);
                }
            }

            t_value.text = Mathf.FloorToInt(curValue).ToString() + "/" + Mathf.FloorToInt(maxValue).ToString();
        }
    }
}

