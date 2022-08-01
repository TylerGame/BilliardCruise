using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BilliardCruise.Sava.Scripts
{
        [RequireComponent(typeof(Animator))]
    public class PauseUI : MonoBehaviour {

        [SerializeField] private GameObject holder;
        [SerializeField] private AnimationClip showClip;
        [SerializeField] private AnimationClip hideClip;

        [SerializeField] private PoolManager poolManager;

        private Animator anim;

        void Awake() {
            anim = GetComponent<Animator> ();

            holder.SetActive (false);
        }

        public void ResumeBtn_OnClick() {
            AudioManager.Instance.PlayBtnSound ();

            poolManager.UnpauseGame ();
            HidePauseUI ();
        }

        public void QuitBtn_OnClick() {
            AudioManager.Instance.PlayBtnSound ();

            poolManager.UnpauseGame ();

            // CustomNetworkManager nm = GameObject.FindObjectOfType<CustomNetworkManager> ();
            // if (nm != null) {
            //     nm.CleanAndStopHost ();
            // }

            PoolSceneManager.Instance.GoToMainMenu ();
        }

        public void CloseBtn_OnClick() {
            AudioManager.Instance.PlayBtnSound ();

            poolManager.UnpauseGame ();
            HidePauseUI ();
        }

        public void ShowPauseUI() {
            holder.SetActive (true);
            anim.Play (showClip.name);
        }

        public void HidePauseUI() {
            anim.Play (hideClip.name);
        }

        public void HideAnim_OnEnded() {
            holder.SetActive (false);
        }

        public void PlaySwooshSound() {
            AudioManager.Instance.PlaySwooshSound ();
        }

    }
}

