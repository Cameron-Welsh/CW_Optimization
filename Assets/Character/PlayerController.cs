//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class PlayerController : MonoBehaviour
//{
//    public float moveSpeed = 3f;
//    public Transform bounds; // Assign the GameObject with the collider defining the bounds
//    public Team team; // Updated variable name to match the enum in Lawyer script

//    private GameObject[] enemies;
//    private bool isAttacking = false;

//    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

//    void Start()
//    {
//        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
//        FindEnemies();
//    }

//    void Update()
//    {
//        if (isAttacking)
//        {
//            MoveTowardsEnemy();
//            // Add attack logic here
//        }
//        else
//        {
//            MoveRandomly();
//        }

//        // Flip the sprite based on the movement direction
//        if (transform.position.x < GetRandomPointInBounds().x)
//        {
//            spriteRenderer.flipX = false; // Face right
//        }
//        else
//        {
//            spriteRenderer.flipX = true; // Face left
//        }
//    }

//    public void SetupBounds(Transform boundsTransform)
//    {
//        bounds = boundsTransform;
//    }

//    void FindEnemies()
//    {
//        GameObject[] allLawyers = GameObject.FindGameObjectsWithTag("Lawyer");

//        // Filter enemies based on team
//        List<GameObject> enemyList = new List<GameObject>();

//        foreach (GameObject lawyerObject in allLawyers)
//        {
//            Lawyer lawyer = lawyerObject.GetComponent<Lawyer>();

//            if (lawyer != null && lawyer.correspondingTeam != this.team)
//            {
//                enemyList.Add(lawyerObject);
//            }
//        }

//        enemies = enemyList.ToArray();
//    }

//    void MoveRandomly()
//    {
//        Vector3 randomDestination = GetRandomPointInBounds();

//        // Move towards the random destination
//        transform.position = Vector3.MoveTowards(transform.position, randomDestination, moveSpeed * Time.deltaTime);

//        // Check if reached the destination
//        if (Vector3.Distance(transform.position, randomDestination) < 0.1f)
//        {
//            // Switch to attacking mode after reaching the random destination
//            isAttacking = true;
//        }
//    }

//    void MoveTowardsEnemy()
//    {
//        if (enemies.Length > 0)
//        {
//            // Find the closest enemy
//            GameObject closestEnemy = FindClosestEnemy();

//            // Move towards the closest enemy
//            transform.position = Vector3.MoveTowards(transform.position, closestEnemy.transform.position, moveSpeed * Time.deltaTime);

//            // Check if reached the enemy
//            if (Vector3.Distance(transform.position, closestEnemy.transform.position) < 0.1f)
//            {
//                // Switch back to moving randomly after reaching the enemy
//                isAttacking = false;
//            }
//        }
//        else
//        {
//            // No enemies, switch back to moving randomly
//            isAttacking = false;
//        }
//    }

//    GameObject FindClosestEnemy()
//    {
//        float minDistance = float.MaxValue;
//        GameObject closestEnemy = null;

//        foreach (GameObject enemy in enemies)
//        {
//            float distance = Vector3.Distance(transform.position, enemy.transform.position);
//            if (distance < minDistance)
//            {
//                minDistance = distance;
//                closestEnemy = enemy;
//            }
//        }

//        return closestEnemy;
//    }

//    Vector3 GetRandomPointInBounds()
//    {
//        if (bounds != null)
//        {
//            Vector2 boundsSize = bounds.GetComponent<Collider2D>().bounds.size;
//            float randomX = Random.Range(bounds.position.x - boundsSize.x / 2, bounds.position.x + boundsSize.x / 2);
//            float randomY = Random.Range(bounds.position.y - boundsSize.y / 2, bounds.position.y + boundsSize.y / 2);
//            return new Vector3(randomX, randomY, transform.position.z);
//        }
//        else
//        {
//            Debug.LogError("Bounds not assigned!");
//            return transform.position;
//        }
//    }
//}





//{
//    #region Variables
//    public float moveSpeed = 5f;
//    public bool controlledByPlayer = false;
//    public Transform bounds;

//    private bool isAttacking = false;

//    #endregion
//    #region UnityCallbacks

//    void Start()
//    {

//    }


//    void Update()
//    {
//        if (controlledByPlayer)
//        {
//            float horizontalInput = Input.GetAxis("Horizontal");
//            float verticalInput = Input.GetAxis("Vertical");

//            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);
//            movement.Normalize();

//            transform.Translate(movement * moveSpeed * Time.deltaTime);
//        }
//        else
//        {
//            if (isAttacking)
//            {
//                MoveTowardsEnemy();
//            }
//            else
//            {
//                MoveRandomly();
//            }
//        }
//    }

//    #endregion
//    #region Functions

//    private void MoveRandomly()
//    {
//        Vector3 randomDestination = GetRandomPointInBounds();

//        transform.position = Vector3.MoveTowards(transform.position, randomDestination, moveSpeed * Time.deltaTime);

//        if (Vector3.Distance(transform.position, randomDestination) < 0.1f)
//        {
//            isAttacking = true;
//        }
//    }

//    void MoveTowardsEnemy()
//    {
//        if (enemyTarget != null)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, enemyTarget.transform.position, moveSpeed * Time.deltaTime);

//            if (Vector3.Distance(transform.position, enemyTarget.transform.position) < 0.1f)
//            {
//                // Switch back to moving randomly after reaching the enemy
//                isAttacking = false;
//            }
//        }
//        else
//        {
//            FindTarget();
//        }
//    }

//    Vector3 GetRandomPointInBounds()
//    {
//        if (bounds != null)
//        {
//            Vector2 boundsSize = bounds.GetComponent<Collider2D>().bounds.size;
//            float randomX = Random.Range(bounds.position.x - boundsSize.x / 2, bounds.position.x + boundsSize.x / 2);
//            float randomY = Random.Range(bounds.position.y - boundsSize.y / 2, bounds.position.y + boundsSize.y / 2);
//            return new Vector3(randomX, randomY, transform.position.z);
//        }
//        else
//        {
//            Debug.LogError("Bounds not assigned!");
//            return transform.position;
//        }
//    }

//    #endregion
//}
