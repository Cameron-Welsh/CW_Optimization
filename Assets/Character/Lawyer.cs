using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class Lawyer : MonoBehaviour
    {
        public enum Team
        {
            Prosecutors,
            Defendants,
        }
        public Team correspondingTeam;
        public int health;
        public int maxHealth;
        public bool hasItem = false;
        private AudioSource audioSource;
        public AudioClip hurt;
        public AudioClip death;


        private SpriteRenderer spriteRenderer;
        private GameObject[] enemies;
        public bool isAttacking = false;
        private Animator animator; // Animator component for handling animations
        public float moveSpeed = 3f;

        private bool hasCheckedTeam = false;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>(); // Assign the Animator component
            FindEnemies(); // Ensure that enemies array is properly initialized
        }


        void FixedUpdate()
        {
            // Check team only once
            if (!hasCheckedTeam)
            {
                CheckTeam();
                hasCheckedTeam = true;
            }
            if (isAttacking)
            {
                MoveTowardsEnemy();
            }
            else
            {
                // If not attacking, walk randomly
                if (Random.Range(0f, 1f) < 0.01f) // Adjust the probability as needed
                {
                    isAttacking = true; // Set isAttacking to true when deciding to walk randomly
                    RandomlyWalk();
                }
                else
                {
                    PlayAnimation("Walk");
                }
            }

            // Flip the sprite based on the movement direction
            if (transform.position.x < GetClosestEnemyPosition().x)
            {
                spriteRenderer.flipX = false; // Face right
            }
            else
            {
                spriteRenderer.flipX = true; // Face left
            }
        }

        void OnEnable()
        {
            // Set the order in layer to a random value within the specified range
            SetRandomOrderInLayer();
        }

        void SetRandomOrderInLayer()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                int randomOrder = Random.Range(0, 61); // Change the range based on your preference
                spriteRenderer.sortingOrder = randomOrder;
            }
            else
            {
                Debug.LogError("SpriteRenderer component not found on GameObject: " + gameObject.name);
            }
        }
        void CheckTeam()
        {
            // Assuming that the enum Team is defined outside the Lawyer class
            if (correspondingTeam == Team.Prosecutors)
            {
                ChangeColor(Color.gray); // Change to the color you want for Prosecutors
            }
            else if (correspondingTeam == Team.Defendants)
            {
                
            }
        }
        void ChangeColor(Color newColor)
        {
            // Get the SpriteRenderer component if not obtained yet
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            // Change the color
            if (spriteRenderer != null)
            {
                spriteRenderer.color = newColor;
            }
            else
            {
                Debug.LogError("SpriteRenderer component not found on GameObject: " + gameObject.name);
            }
        }
        void FindEnemies()
        {
            GameObject[] allLawyers = GameObject.FindGameObjectsWithTag("Lawyer");

            // Filter enemies based on team
            List<GameObject> enemyList = new List<GameObject>();

            foreach (GameObject lawyerObject in allLawyers)
            {
                Lawyer lawyer = lawyerObject.GetComponent<Lawyer>();

                if (lawyer != null && lawyer.correspondingTeam != this.correspondingTeam)
                {
                    enemyList.Add(lawyerObject);
                }
            }

            enemies = enemyList.ToArray();
        }

        void MoveTowardsEnemy()
        {
            if (enemies.Length > 0)
            {
                // Find the closest enemy
                GameObject closestEnemy = FindClosestEnemy();

                // Move towards the closest enemy
                transform.position = Vector3.MoveTowards(transform.position, closestEnemy.transform.position, moveSpeed * Time.deltaTime);

                // Check if reached the enemy
                if (Vector3.Distance(transform.position, closestEnemy.transform.position) < 0.1f)
                {
                    Debug.Log("Reached the enemy!");
                    // Switch back to moving randomly after reaching the enemy
                    isAttacking = false;
                    Attack(closestEnemy);
                        PlayAnimation("Attack");

                }
            }
            else
            {
                // No enemies, switch back to moving randomly
                isAttacking = false;
            }
        }

        void Attack(GameObject target)
        {
            // Check if the target has the Lawyer component
            Lawyer targetLawyer = target.GetComponent<Lawyer>();
            if (targetLawyer != null)
            {
                Debug.Log("Attacking enemy!");
                targetLawyer.DealDamage();

            }

        }
        //private void OnTriggerEnter()
        //{
        //    DealDamage();
        //}

        void DealDamage()
        {
            PlayAnimation("Hurt");
            health -= 1;
            audioSource.clip = hurt;
            float randomVolume = Random.Range(0.4f, 0.6f);
            audioSource.volume = randomVolume;
            float randomPitch = Random.Range(1.0f, 2.30f);
            audioSource.pitch = randomPitch;
            audioSource.Play();
            if (health <= 0)
            {
                health = 0;
                Death();
            }
        }

        void Death()
        {
            audioSource.clip = death;
            audioSource.Play();
            PlayAnimation("Die");
            StartCoroutine(DisableAfterAnimation());
        }

        IEnumerator DisableAfterAnimation()
        {
            // Wait for the death animation to complete (you might need to adjust the time based on the actual animation length)
            yield return new WaitForSeconds(2.0f);

            // Disable the lawyer
            gameObject.SetActive(false);
        }

        void RandomlyWalk()
        {
            Vector3 randomDestination = GetRandomPointInScene();

            // Move towards the random destination
            StartCoroutine(MoveTowardsDestination(randomDestination));
        }

        GameObject FindClosestEnemy()
        {
            float minDistance = float.MaxValue;
            GameObject closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }

        Vector3 GetClosestEnemyPosition()
        {
            GameObject closestEnemy = FindClosestEnemy();

            if (closestEnemy != null)
            {
                return closestEnemy.transform.position;
            }

            return transform.position;
        }

        void PlayAnimation(string animationTrigger)
        {
            if (animator != null)
            {
                // Set the specified trigger to transition to the corresponding animation state
                animator.SetTrigger(animationTrigger);
            }
        }

        Vector3 GetRandomPointInScene()
        {
            float randomX = Random.Range(-10f, 10f); // Adjust the range based on your scene
            float randomY = Random.Range(-5f, 5f);  // Adjust the range based on your scene
            return new Vector3(randomX, randomY, transform.position.z);
        }


        IEnumerator MoveTowardsDestination(Vector3 destination)
        {
            while (Vector3.Distance(transform.position, destination) > 0.1f)
            {
                // Move towards the random destination
                transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

                // Check if reached the destination
                yield return null;
            }

            // Switch to attacking mode after reaching the random destination
            isAttacking = true;
        }
    }
}
