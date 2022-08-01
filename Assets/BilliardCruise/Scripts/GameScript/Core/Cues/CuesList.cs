using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BilliardCruise.Sava.Scripts
{
    [CreateAssetMenu(fileName = "CuesList", menuName = "Cues/CuesList", order = 1)]
    public class CuesList : ScriptableObject
    {

        [SerializeField] private string collectionName = "";
        [SerializeField] private List<CueStats> cues;

        [SerializeField] private float maxPower = 0;
        [SerializeField] private float maxSpin = 0;
        [SerializeField] private float maxAim = 0;
        [SerializeField] private float maxTime = 0;

        public string CollectionName
        {
            get
            {
                return collectionName;
            }
        }

        public List<CueStats> Cues
        {
            get
            {
                return cues;
            }
        }

        public CueStats GetCue(string cueId)
        {
            if (cueId.Equals(""))
                return Cues[0];
            foreach (CueStats cue in Cues)
            {
                if (cue.CueId.Equals(cueId))
                {
                    return cue;
                }
            }

            return null;
        }

        public float GetCueRelativePower(CueStats cue)
        {
            return cue.MaxStrength / maxPower;
        }

        public float GetCueRelativeSpin(CueStats cue)
        {
            return cue.MaxSpin / maxSpin;
        }

        public float GetCueRelativeAim(CueStats cue)
        {
            return cue.AimLength / maxAim;
        }

        public float GetCueRelativeTime(CueStats cue)
        {
            return cue.TimePerMove / maxTime;
        }

    }
}
