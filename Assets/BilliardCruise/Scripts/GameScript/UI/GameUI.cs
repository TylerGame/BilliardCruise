using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


namespace BilliardCruise.Sava.Scripts
{
    public class GameUI : MonoBehaviour
    {

        [SerializeField] private PauseUI pauseUI;

        public PlayerUI player1UI;
        public PlayerUI player2UI;
        [SerializeField] private Text prizeTxt;

        public Slider cueSlider;
        public SpinKnobButton spinKnobBtn;
        public SpinMenu spinMenu;
        public SpinKnob spinKnob;

        public Toast toastHandler;

        [SerializeField] private Sprite emptyBallSprite;
        [SerializeField] private List<Sprite> ballSprites;

        [SerializeField] private PoolManager poolManager;



        /// <summary>
        /// New Added UI
        /// </summary>
        /// 


        [SerializeField]
        GameObject obj_totalMoves, obj_goals;
        [SerializeField]
        TMP_Text txt_moves, txt_goals;

        [SerializeField]
        GameObject obj_booster1, obj_booster2, obj_booster3;
        [SerializeField]
        GameObject popups, rewardPopup, levelPopup, loosePopup, loadingUI;
        [SerializeField]
        Image obj_loadingBarForeground;
        [SerializeField]
        public Image c_text1, c_text2, c_text3, c_text4;
        public enum AnimationName
        {
            ClickMove,
            ClickGoal,

        }
        private static GameUI instance;
        public static GameUI Instance
        {
            get
            {
                return instance;
            }
        }


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        void Start()
        {
            EnableControls(false);
            loadingUI.SetActive(false);
            UpdateTopUI();
            // StartCoroutine(iTest());
            Debug.Log("Global Test === " + Global.coin);
        }

        IEnumerator iTest()
        {
            yield return new WaitForSeconds(2f);
            ShowLevelPopup();
        }

        public void UpdateTopUI()
        {
            txt_goals.text = GameManager.Instance.goal.ToString() + "/" + GameManager.Instance.gameData.levels[GameManager.Instance.level].goal.ToString();
            txt_moves.text = GameManager.Instance.moves.ToString();
        }

        public void DoClickAnim(AnimationName nm)
        {
            switch (nm)
            {
                case AnimationName.ClickMove:
                    obj_totalMoves.transform.DOPunchScale(Vector3.one * (-0.3f), 0.3f, 0);
                    break;
                case AnimationName.ClickGoal:
                    obj_goals.transform.DOPunchScale(Vector3.one * (-0.3f), 0.3f, 0);
                    break;
            }
        }

        public void PauseBtn_OnClick()
        {
            AudioManager.Instance.PlayBtnSound();
            poolManager.PauseGame();
            pauseUI.ShowPauseUI();
        }

        public Sprite GetEmptyBallSprite()
        {
            return emptyBallSprite;
        }

        public Sprite GetBallSprite(int ballNumber)
        {
            if (ballNumber < 0 || ballNumber >= ballSprites.Count)
            {
                return null;
            }
            return ballSprites[ballNumber];
        }

        public void EnableControls(bool enable)
        {
            if (enable)
            {
                //        cueSlider.ShowSlider();
            }
            else
            {
                //        cueSlider.HideSlider();
            }

            // spinKnobBtn.SetInteraction(enable);
            // spinMenu.SetInteraction(enable);
            // spinKnob.SetInteraction(enable);
        }

        public void ResetSpin()
        {
            // spinKnob.Reset();
            // spinKnobBtn.SetKnobPosition(spinKnob.Input);

            // spinMenu.HideMenu();
        }

        public void SetPrize(float prizeAmount)
        {
            //       prizeTxt.text = Formatter.FormatCash(prizeAmount);
        }


        public void ShowRewardPopup()
        {
            InputManager.Instance.Lock();
            popups.SetActive(true);
            levelPopup.SetActive(false);
            loosePopup.SetActive(false);
            rewardPopup.SetActive(true);

            rewardPopup.GetComponent<RectTransform>().DOPunchScale(Vector3.one * 0.3f, 0.5f, 0, 1).OnComplete(() =>
            {
                rewardPopup.GetComponent<WinPopupUI>().StartEffect();
            });
        }

        public void ShowLevelPopup()
        {
            InputManager.Instance.Lock();
            popups.SetActive(true);
            rewardPopup.SetActive(false);
            loosePopup.SetActive(false);
            levelPopup.SetActive(true);
            levelPopup.GetComponent<RectTransform>().DOPunchScale(Vector3.one * 0.3f, 0.5f, 1);
        }

        public void ShowLoosePopup()
        {
            InputManager.Instance.Lock();
            popups.SetActive(true);
            rewardPopup.SetActive(false);
            levelPopup.SetActive(false);
            loosePopup.SetActive(true);
            levelPopup.GetComponent<RectTransform>().DOPunchScale(Vector3.one * 0.3f, 0.5f, 1);
        }

        public void OnClick_TryAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnClick_NextLevel()
        {
            // SceneManager.LoadScene("Level0" + (GameManager.Instance.level + 1).ToString());
            popups.SetActive(false);
            StartCoroutine(iLoading());
            loadingUI.SetActive(true);
            StartCoroutine(iDisplayingText());
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

            if (GameManager.Instance.level == 1)
            {
                SceneManager.LoadScene("Menu");
            }
            else
            {
                SceneManager.LoadScene("Level0" + (GameManager.Instance.level + 1).ToString());
            }

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


        public void OnClick_WinPopupClose()
        {
            SceneManager.LoadScene("Menu");
        }

        public void OnClick_LevelPopupClose()
        {
            SceneManager.LoadScene("Menu");
        }

        public void OnClick_LoosePopupClose()
        {
            SceneManager.LoadScene("Menu");
        }

    }

}

