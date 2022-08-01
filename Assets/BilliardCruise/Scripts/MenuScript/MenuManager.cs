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


        // Start is called before the first frame update
        void Start()
        {
            obj_loadingBarForeground.fillAmount = 0f;
            cur_window_id = 2;
            cur_window = obj_windows[cur_window_id];
            swipeDetector = GameObject.FindObjectOfType<SwipeDetector>();
            isAcceptableSwipe = true;


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

        void OnDisable()
        {
            SwipeDetector._OnSwipeLeft -= OnSwipeLeft;
            SwipeDetector._OnSwipeRight -= OnSwipeRight;
            SwipeDetector._OnSwipeDown -= OnSwipeDown;
            SwipeDetector._OnSwipeUp -= OnSwipeUp;
        }

        public void OnClick_PlayGameUI()
        {
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

