using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BilliardCruise.Sava.Scripts
{
    public class BoosterEffectManager : MonoBehaviour
    {
        SpriteRenderer sp_renderer;
        Animator anim;
        public GameObject owner;

        public bool isState = false;

        // Start is called before the first frame update
        void Start()
        {
            sp_renderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(owner.transform.position.x, 2f, owner.transform.position.z);
        }


        public void SwitchStrengthEffect(bool state)
        {
            anim.SetBool("Strength", state);
            isState = state;
        }

        public void SwitchInvisibleEffect(bool state)
        {
            anim.SetBool("Invisible", state);
            isState = state;
        }

        public void SwitchActivationEffect()
        {
            anim.SetTrigger("Activation");
        }
    }
}


