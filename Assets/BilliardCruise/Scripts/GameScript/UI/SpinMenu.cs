using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BilliardCruise.Sava.Scripts
{
    public class SpinMenu : InteractableMonoBehavior, IPointerDownHandler, IPointerUpHandler
    {

        [SerializeField] private GameObject holder;
        public SpinKnob spinKnob;

        public delegate void ReleaseDelegate();
        public ReleaseDelegate Released;

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!isInteractable)
            {
                return;
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!isInteractable)
            {
                return;
            }

            if (Released != null)
            {
                Released();
            }
        }

        public void ShowMenu()
        {
            holder.SetActive(true);
        }

        public void HideMenu()
        {
            holder.SetActive(false);
        }

    }
}