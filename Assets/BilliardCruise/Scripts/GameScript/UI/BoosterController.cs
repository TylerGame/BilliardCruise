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
            if (GameManager.Instance.gameData.boostersOfGame[GameManager.Instance.level].boosters[index].count > 0)
            {
                c_icon.sprite = GameManager.Instance.gameData.boostersOfGame[GameManager.Instance.level].boosters[index].active_icon;
                c_count.sprite = sp_active_count;
                txt_count.text = GameManager.Instance.gameData.boostersOfGame[GameManager.Instance.level].boosters[index].count.ToString();
                c_icon.GetComponent<Button>().interactable = true;
            }
            else
            {
                c_icon.sprite = GameManager.Instance.gameData.boostersOfGame[GameManager.Instance.level].boosters[index].deactive_icon;
                c_count.sprite = sp_deactive_count;
                txt_count.text = "0";
                c_icon.GetComponent<Button>().interactable = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickMe()
        {
            switch (GameManager.Instance.gameData.boostersOfGame[GameManager.Instance.level].boosters[index].booster)
            {
                case Booster.BoosterType.Arrow:
                    Debug.Log("Arrow");

                    break;
                case Booster.BoosterType.Muscle:
                    Debug.Log("Muscle");

                    break;
                case Booster.BoosterType.Dice:
                    Debug.Log("Dice");

                    break;
                case Booster.BoosterType.Eye:
                    Debug.Log("Eye");

                    break;

            }
        }
    }

}
