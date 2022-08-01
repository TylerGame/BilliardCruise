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
        GameObject popups, rewardPopup, levelPopup;



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
            UpdateTopUI();
            // StartCoroutine(iTest());
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
            rewardPopup.SetActive(true);
            rewardPopup.GetComponent<RectTransform>().DOPunchScale(Vector3.one * 0.3f, 0.5f, 0, 1);
        }

        public void ShowLevelPopup()
        {
            InputManager.Instance.Lock();
            popups.SetActive(true);
            rewardPopup.SetActive(false);
            levelPopup.SetActive(true);
            levelPopup.GetComponent<RectTransform>().DOPunchScale(Vector3.one * 0.3f, 0.5f, 1);
        }

        public void OnClick_NextLevel()
        {
            GameManager.Instance.level++;
            SceneManager.LoadScene("Level0" + GameManager.Instance.level);
        }

    }
}

