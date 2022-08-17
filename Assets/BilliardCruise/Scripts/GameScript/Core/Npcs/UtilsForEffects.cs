using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BilliardCruise.Sava.Scripts
{

    public class UtilsForEffects : MonoBehaviour
    {
        Animator anim;
        SpriteRenderer spr;
        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            spr = GetComponent<SpriteRenderer>();
            StartCoroutine(iDestroyMe());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UseCrackEffect()
        {
            StartCoroutine(iUseCrackEffect());
        }

        IEnumerator iUseCrackEffect()
        {
            yield return new WaitForEndOfFrame();
            anim.SetTrigger("Crack");
        }

        IEnumerator iDestroyMe()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
}

