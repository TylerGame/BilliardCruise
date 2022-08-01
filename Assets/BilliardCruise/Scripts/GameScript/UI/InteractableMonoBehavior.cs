using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BilliardCruise.Sava.Scripts
{
    public class InteractableMonoBehavior : MonoBehaviour
    {

        protected bool isInteractable = true;

        [SerializeField]
        protected Color normalColor = Color.white;

        [SerializeField]
        protected Color disabledColor = Color.white;

        public virtual void SetInteraction(bool interactable)
        {
            isInteractable = interactable;
        }

    }
}

