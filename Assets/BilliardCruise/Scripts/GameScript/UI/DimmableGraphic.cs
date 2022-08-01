using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BilliardCruise.Sava.Scripts
{
        [RequireComponent(typeof(Graphic))]
    public class DimmableGraphic : MonoBehaviour {

        [SerializeField] private Color dimColor = Color.gray;

        private Color normalColor;

        private Graphic graphic;

        void Awake() {
            graphic = GetComponent<Graphic> ();
            normalColor = graphic.color;
        }

        public void Dim() {
            graphic.color = dimColor;
        }

        public void UnDim() {
            graphic.color = normalColor;
        }

    }
}