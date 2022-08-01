using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace BilliardCruise.Sava.Scripts
{
        public class PlayerUI : MonoBehaviour {

        [SerializeField] private TMP_Text nameTxt;
        [SerializeField] private Image avatarImg;

        public Timer timer;
        public List<Image> ballImages;

        private List<DimmableGraphic> dimmableObjects;

        void Awake() {
            FindDimmableObjects ();
        }

        void Start() {
            DimUI ();
        }

        public void SetProfileUI(string name, Sprite avatar) {
            SetNameTxt (name);
            SetAvatar (avatar);
        }

        public void SetNameTxt(string txt) {
            nameTxt.text = Formatter.FormatName (txt);
        }

        public void SetAvatar(Sprite avatar) {
            avatarImg.sprite = avatar;
        }

        public void DimUI() {
            foreach (DimmableGraphic dimmableObj in dimmableObjects) {
                dimmableObj.Dim ();
            }
        }

        public void UnDimUI() {
            foreach (DimmableGraphic dimmableObj in dimmableObjects) {
                dimmableObj.UnDim ();
            }
        }

        private void FindDimmableObjects() {
            if (dimmableObjects == null) {
                dimmableObjects = new List<DimmableGraphic> (GetComponentsInChildren<DimmableGraphic> ());
            }
        }

    }

}
