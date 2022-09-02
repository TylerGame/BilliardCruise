using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace BilliardCruise.Sava.Scripts
{


    [Serializable]
    public class UndoMonsterInfo
    {
        public Vector3 position;
        public float maxHealth;
        public float currentHealth;
        public float damage;
        public bool isdead;
        public GameObject ballIn;


    }
    public class Monster : MonoBehaviour
    {

        public enum MonsterType
        {
            Fish,
            Octopus,
            Shark,
            Box,
            Battery,
            Bottle
        }
        public MonsterType monsterType = MonsterType.Shark;

        public SpriteRenderer healthBar;
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

        public GameObject ballIn;

        public GameObject prefabeOfUtilsforEffects;

        public GameObject left_neighbor;
        public GameObject right_neighbor;

        List<UndoMonsterInfo> undos = new List<UndoMonsterInfo>();


        // Start is called before the first frame update
        void Start()
        {
            currentHealth = MaxHealth;
            if (healthBar != null)
                healthBar.size = new Vector2(currentHealth / MaxHealth, 0.65f);
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


            SaveUndo();
        }

        public void DoUndo()
        {
            if (undos.Count >= 2)
            {
                UndoMonsterInfo undo = undos[undos.Count - 2];
                currentHealth = undo.currentHealth;
                MaxHealth = undo.maxHealth;
                damage = undo.damage;
                isDead = undo.isdead;
                ballIn = undo.ballIn;
                transform.position = undo.position;
                if (healthBar != null)
                    healthBar.size = new Vector2(currentHealth / MaxHealth, 0.65f);
                GetComponent<Collider>().enabled = !isDead;
                GetComponent<Animator>().enabled = !isDead;
                if (monsterType == MonsterType.Battery || monsterType == MonsterType.Box)
                {
                    GetComponent<SpriteRenderer>().enabled = !isDead;
                }
                undos.RemoveAt(undos.Count - 1);
                undos.RemoveAt(undos.Count - 1);
                SaveUndo();
                Transform[] children = GetComponentsInChildren<Transform>();
                if (monsterType != MonsterType.Battery && monsterType != MonsterType.Box)
                {
                    foreach (Transform child in children)
                    {
                        if (!child.gameObject.name.Equals("particle"))
                        {
                            if (child.GetComponent<SpriteRenderer>() != null && !isDead)
                            {
                                child.GetComponent<SpriteRenderer>().enabled = true;
                            }
                        }
                    }
                }
            }
        }

        public void SaveUndo()
        {
            UndoMonsterInfo undo = new UndoMonsterInfo();
            undo.currentHealth = currentHealth;
            undo.maxHealth = MaxHealth;
            undo.damage = damage;
            undo.ballIn = ballIn;
            undo.isdead = isDead;
            undo.position = transform.position;
            undos.Add(undo);
        }

        // Update is called once per frame
        void Update()
        {
            // foreach (Vector3 dir in dirs)
            // {
            //     Debug.DrawLine(transform.position + new Vector3(0, 0, 0.5f), transform.position + new Vector3(0, 0, 0.5f) + dir, Color.red, 10f);
            // }
            if (monsterType == MonsterType.Battery && !isCharging && !isDead)
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

        public void GameOver()
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
                case MonsterType.Bottle:
                    CrackBottle();
                    break;
            }
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
                    case MonsterType.Bottle:
                        CrackBottle();
                        break;
                }
            }
        }


        void CrackBottle()
        {
            if (!isDead)
            {
                isDead = true;
                animatorOfParticle.SetTrigger("Crack");
                animatorOfMonster.SetTrigger("Break");
                GetComponent<Collider>().enabled = false;
                // GetComponentInChildren<Collider>().enabled = false;
                StartCoroutine(iDoDeath());
                if (ballIn != null)
                {
                    ballIn.GetComponent<Rigidbody>().isKinematic = false;
                    ballIn.GetComponent<Collider>().enabled = true;
                    ballIn.GetComponent<Rigidbody>().AddForce(Random.insideUnitCircle.normalized * Random.Range(1f, 5f));
                }
            }
        }
        public void CollisionWithLiveBeing()
        {
            if (GameManager.Instance.isTriggerStrengthEffect || GameManager.Instance.isGameEnding)
                damage = 10000f;
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                if (healthBar != null)
                    healthBar.size = new Vector2(currentHealth / MaxHealth, 0.65f);
                if (!isDead)
                {
                    animatorOfParticle.SetTrigger("SmokeRadial");
                    isDead = true;
                    GetComponent<Collider>().enabled = false;

                    if (healthBar != null)
                        healthBar.enabled = false;
                    StartCoroutine(iDoDeath());
                }
            }
            else
            {
                if (healthBar != null)
                    healthBar.size = new Vector2(currentHealth / MaxHealth, 0.65f);
                animatorOfParticle.SetTrigger("WaterSplash");
                animatorOfMonster.SetTrigger("Hit");
            }
        }

        public void CollisionWithNonLiveBeing()
        {
            if (!isDead)
            {
                if (GameManager.Instance.isTriggerStrengthEffect)
                {
                    StartCoroutine(iCollisionChain());
                }

                animatorOfMonster.SetTrigger("Explosion");
                isDead = true;
                GetComponent<Collider>().enabled = false;
                if (right_neighbor != null)
                {
                    right_neighbor.GetComponent<Monster>().left_neighbor = left_neighbor;
                }
                if (left_neighbor != null)
                {
                    left_neighbor.GetComponent<Monster>().right_neighbor = right_neighbor;
                }


                StartCoroutine(iDoDeath());

            }
        }


        IEnumerator iCollisionChain()
        {
            yield return new WaitForEndOfFrame();
            if (right_neighbor != null)
                right_neighbor.GetComponent<Monster>().CollisionWithNonLiveBeing();
        }

        public void AttackPlayer()
        {
            if (!isDead)
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

            Transform[] children = GetComponentsInChildren<Transform>();
            if (monsterType != MonsterType.Battery && monsterType != MonsterType.Box)
            {
                foreach (Transform child in children)
                {
                    if (!child.gameObject.name.Equals("particle"))
                    {
                        if (child.GetComponent<SpriteRenderer>() != null)
                        {
                            child.GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.7f);
            GetComponent<Collider>().enabled = false;
            GetComponent<Animator>().enabled = false;
            if (monsterType == MonsterType.Battery || monsterType == MonsterType.Box)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
            children = GetComponentsInChildren<Transform>();
            if (monsterType != MonsterType.Battery && monsterType != MonsterType.Box)
            {
                foreach (Transform child in children)
                {
                    if (!child.gameObject.name.Equals("particle"))
                    {
                        if (child.GetComponent<SpriteRenderer>() != null)
                        {
                            child.GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                }
            }
            // if (healthBar != null)
            //     Destroy(healthBar);
            //  Destroy(gameObject);
            // gameObject.SetActive(false);


        }

        public void DisableObject()
        {

        }
    }
}

