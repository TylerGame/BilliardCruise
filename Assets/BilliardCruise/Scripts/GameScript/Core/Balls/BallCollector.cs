using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BilliardCruise.Sava.Scripts
{
    public class BallCollector : MonoBehaviour
    {

        private AudioSource audioSrc;

        [SerializeField] private AudioClip collectorSound;

        void Awake()
        {
            audioSrc = GetComponent<AudioSource>();
        }

        public void StartCollectorSound()
        {
            audioSrc.loop = true;
            audioSrc.clip = collectorSound;
            audioSrc.Play();
        }

        public void StopCollectorSound()
        {
            audioSrc.Stop();
        }

    }
}