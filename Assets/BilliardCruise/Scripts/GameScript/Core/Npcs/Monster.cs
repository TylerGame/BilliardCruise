using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BilliardCruise.Sava.Scripts
{
    public class Monster : MonoBehaviour
    {

        public enum MonsterType
        {
            Fish,
            Octopus,
            Shark,
            Box,
            Battery
        }
        public MonsterType monsterType = MonsterType.Shark;
        public GameObject prefabOfHealthBar;
        private GameObject healthBar;
        public GameObject particle;
        Animator animatorOfParticle;
        Animator animatorOfMonster;

        public float MaxHealth = 100f;
        [HideInInspector]
        public float currentHealth = 100f;
        public float damage = 50f;
        [HideInInspector]
        public bool isDead = false;

        Vector3[] dirs = new Vector3[8];
        bool isCharging = false;

        // Start is called before the first frame update
        void Start()
        {
            Transform parentOfHealthBar = GameObject.Find("ExtraUI").transform;
            if (prefabOfHealthBar != null)
            {
                healthBar = Instantiate(prefabOfHealthBar, parentOfHealthBar);
                healthBar.GetComponent<UIFollow3DObject>().target = transform;
            }

            currentHealth = MaxHealth;
            if (particle != null)
                animatorOfParticle = particle.GetComponent<Animator>();
            animatorOfMonster = GetComponent<Animator>();
            dirs[0] = new Vector3(1, 0, 0);
            dirs[1] = new Vector3(1, 0, 1);
            dirs[2] = new Vector3(0, 0, 1);
            dirs[3] = new Vector3(-1, 0, 1);
            dirs[4] = new Vector3(-1, 0, 0);
            dirs[5] = new Vector3(-1, 0, -1);
            dirs[6] = new Vector3(0, 0, -1);
            dirs[7] = new Vector3(1, 0, -1);
        }

        // Update is called once per frame
        void Update()
        {
            // foreach (Vector3 dir in dirs)
            // {
            //     Debug.DrawLine(transform.position + new Vector3(0, 0, 0.5f), transform.position + new Vector3(0, 0, 0.5f) + dir, Color.red, 10f);
            // }
            if (monsterType == MonsterType.Battery && !isCharging)
            {
                isCharging = true;
                StartCoroutine(iChargeBattery());
            }
        }

        IEnumerator iChargeBattery()
        {
            yield return null;
            animatorOfMonster.SetTrigger("Charge");
            yield return new WaitForSeconds(5f);
            isCharging = false;

        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Ball")
            {
                switch (monsterType)
                {
                    case MonsterType.Fish:


                    case MonsterType.Octopus:

                    case MonsterType.Shark:
                        CollisionWithLiveBeing();
                        break;
                    case MonsterType.Box:

                    case MonsterType.Battery:
                        CollisionWithNonLiveBeing();
                        break;
                }
            }
        }

        void CollisionWithLiveBeing()
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                healthBar.GetComponentsInChildren<Image>()[1].fillAmount = currentHealth / MaxHealth;
                if (!isDead)
                {
                    animatorOfParticle.SetTrigger("SmokeRadial");
                    isDead = true;
                    GetComponent<Collider>().enabled = false;
                    healthBar.SetActive(false);
                    StartCoroutine(iDoDeath());
                }
            }
            else
            {
                healthBar.GetComponentsInChildren<Image>()[1].fillAmount = currentHealth / MaxHealth;
                animatorOfParticle.SetTrigger("WaterSplash");
                animatorOfMonster.SetTrigger("Hit");
            }
        }

        void CollisionWithNonLiveBeing()
        {

            if (!isDead)
            {
                animatorOfMonster.SetTrigger("Explosion");
                isDead = true;
                GetComponent<Collider>().enabled = false;
                StartCoroutine(iDoDeath());
            }

        }


        public void AttackPlayer()
        {
            StartCoroutine(iAttackPlayer());
        }
        IEnumerator iAttackPlayer()
        {
            yield return null;
            List<int> i_dirs = new List<int>();
            int i = 0;
            foreach (Vector3 dir in dirs)
            {
                RaycastHit hit;
                if (!Physics.SphereCast(transform.position + new Vector3(0, 0, 0.5f), 0.5f, dir, out hit, 2f))
                {
                    i_dirs.Add(i);
                    Debug.Log("i === " + i);
                }
                i++;
            }
            float min_distance = float.MaxValue;
            int min_i = -1;
            i = 0;
            foreach (Vector3 dir in dirs)
            {
                if (i_dirs.Contains(i))
                {
                    if (min_distance >= Vector3.Distance(transform.position + new Vector3(0, 0, 0.5f) + dir, PoolManager.Instance.CueBall.transform.position))
                    {
                        min_distance = Vector3.Distance(transform.position + new Vector3(0, 0, 0.5f) + dir, PoolManager.Instance.CueBall.transform.position);
                        min_i = i;
                    }
                }
                i++;
            }
            if (min_i != -1)
            {
                animatorOfMonster.SetTrigger("Move");
                animatorOfParticle.SetTrigger("WaterSplash");
                yield return new WaitForSeconds(0.1f);
                transform.position += dirs[min_i] / 2f;
                yield return new WaitForSeconds(0.3f);
                animatorOfMonster.SetTrigger("Move");
                animatorOfParticle.SetTrigger("WaterSplash");
                yield return new WaitForSeconds(0.1f);
                transform.position += dirs[min_i] / 2f;
            }

        }




        IEnumerator iDoDeath()
        {
            yield return null;
            SpriteRenderer[] renders = GetComponentsInChildren<SpriteRenderer>();
            if (monsterType != MonsterType.Battery && monsterType != MonsterType.Box)
            {
                foreach (SpriteRenderer render in renders)
                {
                    if (!render.gameObject.name.Equals("particle"))
                        render.enabled = false;
                }
            }

            yield return new WaitForSeconds(1f);
            if (healthBar != null)
                Destroy(healthBar);
            Destroy(gameObject);

        }
    }
}

