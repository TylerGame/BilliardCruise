using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace BilliardCruise.Sava.Scripts
{
    public class SplashManager : MonoBehaviour
    {
        [SerializeField]
        public Image c_LoadingBarForeground;
        [SerializeField]
        string str_NextSceneName;

        [SerializeField] private Image c_text1, c_text2, c_text3, c_text4;



        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(iLoading());
            StartCoroutine(iDisplayingText());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator iDisplayingText()
        {
            yield return null;
            while (true)
            {
                c_text1.GetComponent<RectTransform>().DOAnchorPosX(0, 0.2f, false);
                yield return new WaitForSeconds(2f);
                c_text1.GetComponent<RectTransform>().DOAnchorPosX(4654, 0.5f, false).OnComplete(() =>
                {
                    c_text1.GetComponent<RectTransform>().SetLeft(-4654f);
                    c_text1.GetComponent<RectTransform>().SetRight(4654f);
                });


                c_text2.GetComponent<RectTransform>().DOAnchorPosX(0, 0.2f, false);
                yield return new WaitForSeconds(2f);
                c_text2.GetComponent<RectTransform>().DOAnchorPosX(4654, 0.5f, false).OnComplete(() =>
                {
                    c_text2.GetComponent<RectTransform>().SetLeft(-4654f);
                    c_text2.GetComponent<RectTransform>().SetRight(4654f);
                });


                c_text3.GetComponent<RectTransform>().DOAnchorPosX(0, 0.2f, false);
                yield return new WaitForSeconds(2f);
                c_text3.GetComponent<RectTransform>().DOAnchorPosX(4654, 0.5f, false).OnComplete(() =>
                {
                    c_text3.GetComponent<RectTransform>().SetLeft(-4654f);
                    c_text3.GetComponent<RectTransform>().SetRight(4654f);
                });


                c_text4.GetComponent<RectTransform>().DOAnchorPosX(0, 0.2f, false);
                yield return new WaitForSeconds(2f);
                c_text4.GetComponent<RectTransform>().DOAnchorPosX(4654, 0.5f, false).OnComplete(() =>
                {
                    c_text4.GetComponent<RectTransform>().SetLeft(-4654f);
                    c_text4.GetComponent<RectTransform>().SetRight(4654f);
                });

            }
        }

        IEnumerator iLoading()
        {
            float loadingTime = 0f;
            c_LoadingBarForeground.fillAmount = 0f;
            while (true)
            {
                loadingTime += (Time.deltaTime * 5f);
                yield return new WaitForSeconds(0.1f);
                c_LoadingBarForeground.fillAmount = (loadingTime / 3f);
                if (loadingTime >= 3f)
                    break;
            }
            SceneManager.LoadScene(str_NextSceneName);
        }
    }
}

