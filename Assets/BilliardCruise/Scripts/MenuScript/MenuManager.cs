using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using TMPro;
using DG.Tweening;
using Krivodeling.UI.Effects;
namespace BilliardCruise.Sava.Scripts
{


    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        GameObject obj_PlayGameUI, obj_LoadingUI;

        [SerializeField]
        Image obj_loadingBarForeground;
        [SerializeField]
        string str_nextSceneName;
        [SerializeField]
        GameObject[] obj_windows;
        [SerializeField]
        GameObject[] obj_buttons;
        [SerializeField]
        TMP_Text[] obj_titles;

        private GameObject cur_window;
        private int cur_window_id = 2;
        // private InputManager inputManager;
        private SwipeDetector swipeDetector;
        private bool isAcceptableSwipe = false;


        [SerializeField] private Image c_text1, c_text2, c_text3, c_text4;

        [SerializeField] GameObject coinModel, starModel, keyModel;
        public Waypoints starWays, coinWays, keyWays;

        [SerializeField] GameObject starDisplay, coinDisplay, keyDisplay;
        [SerializeField] GameObject starEffect, coinEffect, keyEffect;
        [SerializeField] GameObject starNum, coinNum, keyNum;


        [SerializeField] GameObject zoneBanner;

        // Start is called before the first frame update
        void Start()
        {
            obj_loadingBarForeground.fillAmount = 0f;
            cur_window_id = 2;
            cur_window = obj_windows[cur_window_id];
            swipeDetector = GameObject.FindObjectOfType<SwipeDetector>();
            isAcceptableSwipe = true;

            starEffect.SetActive(false);
            coinEffect.SetActive(false);
            keyEffect.SetActive(false);

            starModel.SetActive(false);
            coinModel.SetActive(false);
            keyModel.SetActive(false);

            starNum.SetActive(false);
            coinNum.SetActive(false);
            keyNum.SetActive(false);

            zoneBanner.SetActive(false);
            // uiBluer.BuildFlipMode = FlipMode.Y;
        }

        // Update is called once per frame
        void Update()
        {



        }



        void OnEnable()
        {
            SwipeDetector._OnSwipeLeft += OnSwipeLeft;
            SwipeDetector._OnSwipeRight += OnSwipeRight;
            SwipeDetector._OnSwipeDown += OnSwipeDown;
            SwipeDetector._OnSwipeUp += OnSwipeUp;
        }

        public void StartAnimCoin_Star()
        {
            starModel.transform.localScale = Vector3.zero;
            starModel.SetActive(true);
            starNum.SetActive(true);


            starModel.transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
            {
                starNum.transform.DOLocalMoveY(250f, 1f).OnComplete(() =>
                   {
                       starNum.GetComponent<RectTransform>().localPosition = new Vector3(173f, 0f, 0);

                   });

                StartCoroutine(iFadeColor(starNum.GetComponent<TMP_Text>()));
                // starNum.GetComponent<TMP_Text>().material.DOColor(new Color(starNum.GetComponent<TMP_Text>().material.color.r, starNum.GetComponent<TMP_Text>().material.color.g, starNum.GetComponent<TMP_Text>().material.color.b, 0.5f), 1f).OnComplete(() =>
                // {
                //     starNum.GetComponent<TMP_Text>().material.color = new Color(starNum.GetComponent<TMP_Text>().material.color.r, starNum.GetComponent<TMP_Text>().material.color.g, starNum.GetComponent<TMP_Text>().material.color.b, 1f);
                //     starNum.SetActive(false);
                // });


                starModel.transform.DOPath(starWays.waypoints.ToArray(), 1f, PathType.Linear, PathMode.TopDown2D).OnComplete(() =>
                {
                    starModel.GetComponent<RectTransform>().localPosition = new Vector3(173f, 0f, 0f);
                });
                starModel.transform.DOScale(Vector3.one * 0.7f, 1f).OnComplete(() =>
                {
                    starModel.SetActive(false);
                    starModel.GetComponent<RectTransform>().localScale = Vector3.one;
                    starEffect.SetActive(true);
                    starEffect.GetComponent<Animator>().SetTrigger("Glitter");
                    starDisplay.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 5).OnComplete(() =>
                    {
                        starEffect.SetActive(false);
                    });
                });
            });


        }

        IEnumerator iFadeColor(TMP_Text tp)
        {
            yield return null;
            float alpha = 1f;
            while (alpha > 0.5f)
            {
                tp.color = new Color(tp.color.r, tp.color.g, tp.color.b, alpha);
                alpha -= Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            tp.gameObject.SetActive(false);
            tp.color = new Color(tp.color.r, tp.color.g, tp.color.b, 1f);

        }
        public void StartAnimation_Coin()
        {
            coinModel.transform.localScale = Vector3.zero;
            coinModel.SetActive(true);

            coinNum.SetActive(true);

            coinModel.transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
            {
                coinNum.transform.DOLocalMoveY(250f, 1f).OnComplete(() =>
                {
                    coinNum.GetComponent<RectTransform>().localPosition = new Vector3(-173f, 0f, 0);
                    coinNum.SetActive(false);
                });

                StartCoroutine(iFadeColor(coinNum.GetComponent<TMP_Text>()));
                // starNum.GetComponent<TMP_Text>().material.DOFade(0.5f, 1f).OnComplete(() =>
                // {
                //     starNum.GetComponent<TMP_Text>().material.color = new Color(starNum.GetComponent<TMP_Text>().material.color.r, starNum.GetComponent<TMP_Text>().material.color.g, starNum.GetComponent<TMP_Text>().material.color.b, 1f);
                //     starNum.SetActive(false);
                // });


                coinModel.transform.DOPath(coinWays.waypoints.ToArray(), 1f, PathType.Linear, PathMode.TopDown2D).OnComplete(() =>
                {
                    coinModel.GetComponent<RectTransform>().localPosition = new Vector3(-173f, 0f, 0f);
                });
                coinModel.transform.DOScale(Vector3.one * 0.7f, 1f).OnComplete(() =>
                {
                    coinModel.SetActive(false);
                    coinModel.GetComponent<RectTransform>().localScale = Vector3.one;
                    coinEffect.SetActive(true);
                    coinEffect.GetComponent<Animator>().SetTrigger("Glitter");
                    coinDisplay.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 5).OnComplete(() =>
                    {
                        coinEffect.SetActive(false);
                    });

                    StartAnimation_Key();
                });
            });


        }


        public void StartAnimation_Key()
        {
            keyModel.transform.localScale = Vector3.zero;
            keyModel.SetActive(true);

            keyNum.SetActive(true);

            keyModel.transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
            {
                keyNum.transform.DOLocalMoveY(250f, 1f).OnComplete(() =>
                {
                    keyNum.GetComponent<RectTransform>().localPosition = new Vector3(5f, 0f, 0);
                    keyNum.SetActive(false);
                });

                StartCoroutine(iFadeColor(keyNum.GetComponent<TMP_Text>()));
                // starNum.GetComponent<TMP_Text>().material.DOFade(0.5f, 1f).OnComplete(() =>
                // {
                //     starNum.GetComponent<TMP_Text>().material.color = new Color(starNum.GetComponent<TMP_Text>().material.color.r, starNum.GetComponent<TMP_Text>().material.color.g, starNum.GetComponent<TMP_Text>().material.color.b, 1f);
                //     starNum.SetActive(false);
                // });


                keyModel.transform.DOPath(keyWays.waypoints.ToArray(), 1f, PathType.Linear, PathMode.TopDown2D).OnComplete(() =>
                {
                    keyModel.GetComponent<RectTransform>().localPosition = new Vector3(5f, 0f, 0f);
                });
                keyModel.transform.DOScale(Vector3.one * 0.7f, 1f).OnComplete(() =>
                {
                    keyModel.SetActive(false);
                    keyModel.GetComponent<RectTransform>().localScale = Vector3.one;
                    keyEffect.SetActive(true);
                    keyEffect.GetComponent<Animator>().SetTrigger("Glitter");
                    keyDisplay.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 5).OnComplete(() =>
                    {
                        keyEffect.SetActive(false);
                        StartAnimation_ZoneBanner();
                    });
                });
            });
        }

        public void StartAnimation_ZoneBanner()
        {
            zoneBanner.transform.localScale = Vector3.zero;
            zoneBanner.SetActive(true);
            zoneBanner.transform.DOScale(Vector3.one, 0.2f);
        }

        void OnDisable()
        {
            SwipeDetector._OnSwipeLeft -= OnSwipeLeft;
            SwipeDetector._OnSwipeRight -= OnSwipeRight;
            SwipeDetector._OnSwipeDown -= OnSwipeDown;
            SwipeDetector._OnSwipeUp -= OnSwipeUp;
        }

        public void OnClick_PlayGameUI()
        {

            StartAnimCoin_Star();
            StartAnimation_Coin();

            return;

            obj_PlayGameUI.SetActive(true);
            isAcceptableSwipe = false;
            //    uiBluer = GameObject.FindObjectOfType<UIBlur>();
            // obj_PlayGameUI.GetComponent<Krivodeling.UI.Effects.UIBlur>().EditorFlipMode = FlipMode.Y;
            StartCoroutine(iRevertFlip());
        }

        IEnumerator iRevertFlip()
        {
            yield return null;
            obj_PlayGameUI.GetComponent<Krivodeling.UI.Effects.UIBlur>().BuildFlipMode = FlipMode.Y;
        }
        public void OnClose_PlayGameUI()
        {
            obj_PlayGameUI.SetActive(false);
            isAcceptableSwipe = true;
        }

        public void OnClick_PlayLevelButton()
        {
            obj_PlayGameUI.SetActive(false);
            obj_LoadingUI.SetActive(true);
            StartCoroutine(iLoading());
            StartCoroutine(iDisplayingText());
        }

        private void OnSwipeLeft()
        {
            if (!isAcceptableSwipe)
                return;
            obj_windows[cur_window_id].GetComponent<RectTransform>().DOAnchorPosX(-2000f, 0.2f, false);
            Vector2 origin_size = obj_buttons[cur_window_id].GetComponent<RectTransform>().sizeDelta;

            obj_buttons[cur_window_id].GetComponent<RectTransform>().DOSizeDelta(new Vector2(300f, origin_size.y), 0.2f, false);
            obj_buttons[cur_window_id].GetComponent<Image>().color = new Color(182f / 255f, 206f / 255f, 1f, 1f);
            obj_buttons[cur_window_id].GetComponentsInChildren<RectTransform>()[1].DOScale(Vector3.one, 0.2f);
            obj_titles[cur_window_id].enabled = false;
            if (cur_window_id == (obj_windows.Length - 1))
            {
                obj_windows[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(2000f, 0f);
                obj_windows[0].GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f, false).OnComplete(() =>
                {
                    cur_window_id = 0;
                });
                obj_buttons[0].GetComponent<RectTransform>().DOSizeDelta(new Vector2(559, origin_size.y), 0.2f, false);
                obj_buttons[0].GetComponent<Image>().color = Color.white;
                obj_buttons[0].GetComponentsInChildren<RectTransform>()[1].DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f);
                obj_titles[0].enabled = true;
            }
            else
            {
                obj_windows[cur_window_id + 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(2000f, 0f);
                obj_buttons[cur_window_id + 1].GetComponent<RectTransform>().DOSizeDelta(new Vector2(559f, origin_size.y), 0.2f, false);
                obj_buttons[cur_window_id + 1].GetComponent<Image>().color = Color.white;
                obj_buttons[cur_window_id + 1].GetComponentsInChildren<RectTransform>()[1].DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f);
                obj_titles[cur_window_id + 1].enabled = true;
                obj_windows[cur_window_id + 1].GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f, false).OnComplete(() =>
                {
                    cur_window_id += 1;
                });
            }
        }

        private void OnSwipeRight()
        {

            if (!isAcceptableSwipe)
                return;
            obj_windows[cur_window_id].GetComponent<RectTransform>().DOAnchorPosX(2000f, 0.2f, false);

            Vector2 origin_size = obj_buttons[cur_window_id].GetComponent<RectTransform>().sizeDelta;

            obj_buttons[cur_window_id].GetComponent<RectTransform>().DOSizeDelta(new Vector2(300f, origin_size.y), 0.2f, false);
            obj_buttons[cur_window_id].GetComponent<Image>().color = new Color(182f / 255f, 206f / 255f, 1f, 1f);
            obj_buttons[cur_window_id].GetComponentsInChildren<RectTransform>()[1].DOScale(Vector3.one, 0.2f);
            obj_titles[cur_window_id].enabled = false;

            if (cur_window_id == 0)
            {
                obj_windows[obj_windows.Length - 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000f, 0f);
                obj_buttons[obj_windows.Length - 1].GetComponent<RectTransform>().DOSizeDelta(new Vector2(559, origin_size.y), 0.2f, false);
                obj_buttons[obj_windows.Length - 1].GetComponent<Image>().color = Color.white;
                obj_buttons[obj_windows.Length - 1].GetComponentsInChildren<RectTransform>()[1].DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f);
                obj_titles[obj_windows.Length - 1].enabled = true;
                obj_windows[obj_windows.Length - 1].GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f, false).OnComplete(() =>
                {
                    cur_window_id = obj_windows.Length - 1;
                });
            }
            else
            {
                obj_windows[cur_window_id - 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000f, 0f);
                obj_buttons[cur_window_id - 1].GetComponent<RectTransform>().DOSizeDelta(new Vector2(559f, origin_size.y), 0.2f, false);
                obj_buttons[cur_window_id - 1].GetComponent<Image>().color = Color.white;
                obj_buttons[cur_window_id - 1].GetComponentsInChildren<RectTransform>()[1].DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f);
                obj_titles[cur_window_id - 1].enabled = true;
                obj_windows[cur_window_id - 1].GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f, false).OnComplete(() =>
                {
                    cur_window_id -= 1;
                });
            }
        }

        private void OnSwipeUp()
        {

        }


        private void OnSwipeDown()
        {

        }


        public void OnClick_Shop()
        {

            if (cur_window_id == 0)
                return;
            obj_windows[cur_window_id].GetComponent<RectTransform>().DOAnchorPosX(-2000f, 0.2f, false);
            Vector2 origin_size = obj_buttons[cur_window_id].GetComponent<RectTransform>().sizeDelta;

            obj_buttons[cur_window_id].GetComponent<RectTransform>().DOSizeDelta(new Vector2(300f, origin_size.y), 0.2f, false);
            obj_buttons[cur_window_id].GetComponent<Image>().color = new Color(182f / 255f, 206f / 255f, 1f, 1f);
            obj_buttons[cur_window_id].GetComponentsInChildren<RectTransform>()[1].DOScale(Vector3.one, 0.2f);
            obj_titles[cur_window_id].enabled = false;



            obj_windows[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(2000f, 0f);
            obj_windows[0].GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f, false).OnComplete(() =>
            {
                // cur_window_id = 0;
            });
            obj_buttons[0].GetComponent<RectTransform>().DOSizeDelta(new Vector2(559, origin_size.y), 0.2f, false);
            obj_buttons[0].GetComponent<Image>().color = Color.white;
            obj_buttons[0].GetComponentsInChildren<RectTransform>()[1].DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f);
            obj_titles[0].enabled = true;
            cur_window_id = 0;

        }

        public void OnClick_Hero()
        {
            if (cur_window_id == 1)
                return;
            obj_windows[cur_window_id].GetComponent<RectTransform>().DOAnchorPosX(-2000f, 0.2f, false);
            Vector2 origin_size = obj_buttons[cur_window_id].GetComponent<RectTransform>().sizeDelta;

            obj_buttons[cur_window_id].GetComponent<RectTransform>().DOSizeDelta(new Vector2(300f, origin_size.y), 0.2f, false);
            obj_buttons[cur_window_id].GetComponent<Image>().color = new Color(182f / 255f, 206f / 255f, 1f, 1f);
            obj_buttons[cur_window_id].GetComponentsInChildren<RectTransform>()[1].DOScale(Vector3.one, 0.2f);
            obj_titles[cur_window_id].enabled = false;



            obj_windows[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(2000f, 0f);
            obj_windows[1].GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f, false).OnComplete(() =>
            {
                // cur_window_id = 1;
            });
            obj_buttons[1].GetComponent<RectTransform>().DOSizeDelta(new Vector2(559, origin_size.y), 0.2f, false);
            obj_buttons[1].GetComponent<Image>().color = Color.white;
            obj_buttons[1].GetComponentsInChildren<RectTransform>()[1].DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f);
            obj_titles[1].enabled = true;
            cur_window_id = 1;
        }

        public void OnClick_Main()
        {
            if (cur_window_id == 2)
                return;
            obj_windows[cur_window_id].GetComponent<RectTransform>().DOAnchorPosX(-2000f, 0.2f, false);
            Vector2 origin_size = obj_buttons[cur_window_id].GetComponent<RectTransform>().sizeDelta;

            obj_buttons[cur_window_id].GetComponent<RectTransform>().DOSizeDelta(new Vector2(300f, origin_size.y), 0.2f, false);
            obj_buttons[cur_window_id].GetComponent<Image>().color = new Color(182f / 255f, 206f / 255f, 1f, 1f);
            obj_buttons[cur_window_id].GetComponentsInChildren<RectTransform>()[1].DOScale(Vector3.one, 0.2f);
            obj_titles[cur_window_id].enabled = false;



            obj_windows[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(2000f, 0f);
            obj_windows[2].GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f, false).OnComplete(() =>
            {
                // cur_window_id = 2;
            });
            obj_buttons[2].GetComponent<RectTransform>().DOSizeDelta(new Vector2(559, origin_size.y), 0.2f, false);
            obj_buttons[2].GetComponent<Image>().color = Color.white;
            obj_buttons[2].GetComponentsInChildren<RectTransform>()[1].DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f);
            obj_titles[2].enabled = true;
            cur_window_id = 2;
        }

        public void OnClick_Team()
        {
            if (cur_window_id == 3)
                return;
            obj_windows[cur_window_id].GetComponent<RectTransform>().DOAnchorPosX(-2000f, 0.2f, false);
            Vector2 origin_size = obj_buttons[cur_window_id].GetComponent<RectTransform>().sizeDelta;

            obj_buttons[cur_window_id].GetComponent<RectTransform>().DOSizeDelta(new Vector2(300f, origin_size.y), 0.2f, false);
            obj_buttons[cur_window_id].GetComponent<Image>().color = new Color(182f / 255f, 206f / 255f, 1f, 1f);
            obj_buttons[cur_window_id].GetComponentsInChildren<RectTransform>()[1].DOScale(Vector3.one, 0.2f);
            obj_titles[cur_window_id].enabled = false;



            obj_windows[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(2000f, 0f);
            obj_windows[3].GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f, false).OnComplete(() =>
            {
                // cur_window_id = 3;
            });
            obj_buttons[3].GetComponent<RectTransform>().DOSizeDelta(new Vector2(559, origin_size.y), 0.2f, false);
            obj_buttons[3].GetComponent<Image>().color = Color.white;
            obj_buttons[3].GetComponentsInChildren<RectTransform>()[1].DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f);
            obj_titles[3].enabled = true;
            cur_window_id = 3;
        }

        public void OnClick_Setting()
        {
            if (cur_window_id == 4)
                return;
            obj_windows[cur_window_id].GetComponent<RectTransform>().DOAnchorPosX(-2000f, 0.2f, false);
            Vector2 origin_size = obj_buttons[cur_window_id].GetComponent<RectTransform>().sizeDelta;

            obj_buttons[cur_window_id].GetComponent<RectTransform>().DOSizeDelta(new Vector2(300f, origin_size.y), 0.2f, false);
            obj_buttons[cur_window_id].GetComponent<Image>().color = new Color(182f / 255f, 206f / 255f, 1f, 1f);
            obj_buttons[cur_window_id].GetComponentsInChildren<RectTransform>()[1].DOScale(Vector3.one, 0.2f);
            obj_titles[cur_window_id].enabled = false;



            obj_windows[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(2000f, 0f);
            obj_windows[4].GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f, false).OnComplete(() =>
            {
                // cur_window_id = 4;
            });
            obj_buttons[4].GetComponent<RectTransform>().DOSizeDelta(new Vector2(559, origin_size.y), 0.2f, false);
            obj_buttons[4].GetComponent<Image>().color = Color.white;
            obj_buttons[4].GetComponentsInChildren<RectTransform>()[1].DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f);
            obj_titles[4].enabled = true;
            cur_window_id = 4;
        }


        IEnumerator iLoading()
        {
            float loadingTime = 0f;
            obj_loadingBarForeground.fillAmount = 0f;
            while (true)
            {
                loadingTime += (Time.deltaTime * 10f);
                yield return new WaitForSeconds(0.1f);
                obj_loadingBarForeground.fillAmount = (loadingTime / 3f);
                if (loadingTime >= 3f)
                    break;
            }
            SceneManager.LoadScene(str_nextSceneName);
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
    }
}

