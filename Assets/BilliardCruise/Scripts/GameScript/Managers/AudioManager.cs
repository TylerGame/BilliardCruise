using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BilliardCruise.Sava.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonMonoBehavior<AudioManager> {
        
        [SerializeField] private AudioClip btnSound;
        [SerializeField] private AudioClip swooshSound;

        private AudioSource soundsSource;

        protected override void Awake() {
            base.Awake ();
            DontDestroyOnLoad (this.gameObject);

            soundsSource = GetComponent<AudioSource> ();
        }

        void Start() {
            EnableSound (Prefs.IsSoundOn);
        }

        public void PlayBtnSound() {
            soundsSource.clip = btnSound;
            soundsSource.Play ();
        }

        public void PlaySwooshSound() {
            soundsSource.clip = swooshSound;
            soundsSource.Play ();
        }

        public void EnableSound(bool enable) {
            AudioListener.volume = enable ? 1 : 0;
            Prefs.IsSoundOn = enable;
        }

        public void ToggleSound() {
            EnableSound (!Prefs.IsSoundOn);
        }

    }

}

