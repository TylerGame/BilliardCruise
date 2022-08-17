using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BilliardCruise.Sava.Scripts
{
    public class BoosterController : MonoBehaviour
    {
        [SerializeField]
        TMP_Text txt_count;

        [SerializeField]
        Image c_icon, c_count;

        [SerializeField]
        Sprite sp_active_count, sp_deactive_count;

        [SerializeField]
        int index;



        // Start is called before the first frame update
        void Start()
        {

            if (GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].count > 0)
            {
                c_icon.sprite = GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].active_icon;
                c_count.sprite = sp_active_count;
                txt_count.text = GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].count.ToString();
                c_icon.GetComponent<Button>().interactable = true;
            }
            else
            {
                c_icon.sprite = GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].deactive_icon;
                c_count.sprite = sp_deactive_count;
                txt_count.text = "0";
                c_icon.GetComponent<Button>().interactable = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (!GameManager.Instance.isMoving)
            {
                GetComponent<Button>().interactable = !(GameManager.Instance.isTriggerArrowEffect || GameManager.Instance.isTriggerDiceEffect || GameManager.Instance.isTriggerEyeEffect || GameManager.Instance.isTriggerStrengthEffect);
            }
            else
            {
                GetComponent<Button>().interactable = false;
            }

        }

        public void OnClickMe()
        {

            if (GameManager.Instance.isTriggerArrowEffect || GameManager.Instance.isTriggerDiceEffect || GameManager.Instance.isTriggerEyeEffect || GameManager.Instance.isTriggerStrengthEffect)
            {
                return;
            }
            switch (GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].booster)
            {
                case Booster.BoosterType.Arrow:
                    Debug.Log("Arrow");
                    GameManager.Instance.isTriggerArrowEffect = true;
                    GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].count--;


                    break;
                case Booster.BoosterType.Muscle:
                    Debug.Log("Muscle");
                    GameManager.Instance.isTriggerStrengthEffect = true;
                    GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].count--;
                    break;
                case Booster.BoosterType.Dice:
                    Debug.Log("Dice");
                    GameManager.Instance.isTriggerDiceEffect = true;
                    GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].count--;
                    break;
                case Booster.BoosterType.Eye:
                    Debug.Log("Eye");
                    GameManager.Instance.isTriggerEyeEffect = true;
                    GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].count--;
                    break;
            }

            if (GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].count <= 0)
            {
                GetComponent<Image>().sprite = GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].deactive_icon;
                GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].count = 0;
            }
            txt_count.text = GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].count.ToString();


            ApplyEffect();
        }

        void ApplyEffect()
        {
            StartCoroutine(iApplyEffect());
        }

        IEnumerator iApplyEffect()
        {
            yield return null;
            PoolManager.Instance.CueBall.GetComponent<Ball_Local>().boosterEffectManager.SwitchActivationEffect();
            yield return new WaitForSeconds(0.5f);
            switch (GameManager.Instance.gameData.levels[GameManager.Instance.level].boosters[index].booster)
            {
                case Booster.BoosterType.Arrow:

                    break;
                case Booster.BoosterType.Muscle:
                    PoolManager.Instance.CueBall.GetComponent<Ball_Local>().boosterEffectManager.SwitchStrengthEffect(true);
                    break;
                case Booster.BoosterType.Dice:

                    break;
                case Booster.BoosterType.Eye:
                    PoolManager.Instance.CueBall.GetComponent<Ball_Local>().boosterEffectManager.SwitchInvisibleEffect(true);
                    GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                    foreach (GameObject monster in monsters)
                    {
                        if (monster.GetComponent<Monster>().monsterType == Monster.MonsterType.Fish || monster.GetComponent<Monster>().monsterType == Monster.MonsterType.Octopus || monster.GetComponent<Monster>().monsterType == Monster.MonsterType.Shark || monster.GetComponent<Monster>().monsterType == Monster.MonsterType.Box)
                        {
                            SpriteRenderer[] sprs = monster.GetComponentsInChildren<SpriteRenderer>();
                            foreach (SpriteRenderer spr in sprs)
                            {
                                spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0.5f);

                            }

                            Collider[] colliders = monster.GetComponentsInChildren<Collider>();
                            foreach (Collider col in colliders)
                            {
                                col.enabled = false;
                            }
                        }
                    }
                    break;
            }
        }
    }

}
