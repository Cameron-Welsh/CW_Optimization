using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class GameStateManager : MonoBehaviour
    {
        #region Singleton

        private static GameStateManager _instance;

        public static GameStateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameStateManager>();
                    if (_instance == null)
                    {
                        GameObject managerObject = new GameObject("GameStateManager");
                        _instance = managerObject.AddComponent<GameStateManager>();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Variables
        SpawnManager spawnManager;
        public GameObject lawyerPrefab;
        public GameObject screenSwipePrefab;
        public Material screenSwipeMaterial;
        public int teamSize = 15;
        public int reinforcementSize = 5;
        public bool leftVictory = false;
        public bool rightVictory = false;
        public GameObject background;
        public Sprite courtroom;
        public Sprite hallway;
        public Sprite courtroomSteps;
        public Transform movementBounds;

        public List<GameObject> prosecutorsTeamPool;
        public List<GameObject> defendantsTeamPool;

        public enum Arena
        {
            Courtroom,
            ProsecutorHall,
            DefendantsHall,
            ProsecutorsPodium,
            DefendantsPodium,
        }

        public Arena currentArena;
        public Vector3[] spawnLocations;

        private Coroutine reinforcementCoroutine;
        public float reinforcementInterval = 5.0f;

        #endregion

        #region UnityCallbacks
        void Start()
        {
            spawnLocations = new Vector3[10];
            spawnLocations[0].x = -12.0f;
            spawnLocations[0].y = -2.0f;
            // ... (other spawn locations remain unchanged)
            spawnLocations[5].x = 12.0f;
            spawnLocations[5].y = -2.0f;
            // ... (other spawn locations remain unchanged)

            currentArena = Arena.Courtroom;
            spawnManager = GetComponent<SpawnManager>();

            InitializeTeamPools();
            reinforcementCoroutine = StartCoroutine(ReinforcementCoroutine());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeLevel();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {

                reinforcementInterval--;

            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                reinforcementInterval++;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                currentArena = Arena.Courtroom;
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                currentArena = Arena.ProsecutorHall;
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                currentArena = Arena.DefendantsPodium;
            }
        }

        void OnDestroy()
        {
            if (reinforcementCoroutine != null)
            {
                StopCoroutine(reinforcementCoroutine);
            }
        }
        #endregion

        #region Functions

        public void ChangeLevel()
        {
            if (leftVictory)
            {
                screenSwipeMaterial.SetFloat("_SwipeDirection", 1.0f);
                StartCoroutine(ChangeBackgrounds());
            }
        }

        IEnumerator ChangeBackgrounds()
        {
            float duration = 1.0f;
            float targetValue = 0f;

            float startTime = Time.time;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                if (screenSwipeMaterial != null)
                {
                    float newProgress = Mathf.Lerp(1f, targetValue, elapsedTime / duration);
                    screenSwipeMaterial.SetFloat("_ScreenSwipePercent", newProgress);
                }

                elapsedTime = Time.time - startTime;
                yield return null;
            }

            if (screenSwipeMaterial.GetFloat("_SwipeDirection") == 1f)
            {
                screenSwipeMaterial.SetFloat("_SwipeDirection", 0f);
            }

            SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();

            switch (currentArena)
            {
                case Arena.Courtroom:
                    spriteRenderer.sprite = courtroom;
                    break;
                case Arena.ProsecutorHall:
                case Arena.DefendantsHall:
                    spriteRenderer.sprite = hallway;
                    spriteRenderer.sprite = hallway;
                    break;
                case Arena.ProsecutorsPodium:
                case Arena.DefendantsPodium:
                    spriteRenderer.sprite = courtroomSteps;
                    spriteRenderer.sprite = courtroomSteps;
                    break;
            }

            targetValue = 1f;
            startTime = Time.time;
            elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                if (screenSwipeMaterial != null)
                {
                    float newProgress = Mathf.Lerp(0f, targetValue, elapsedTime / duration);
                    screenSwipeMaterial.SetFloat("_ScreenSwipePercent", newProgress);
                }

                elapsedTime = Time.time - startTime;
                yield return null;
            }
        }

        void InitializeTeamPools()
        {
            prosecutorsTeamPool = new List<GameObject>();
            defendantsTeamPool = new List<GameObject>();

            for (int i = 0; i < teamSize; i++)
            {
                GameObject prosecutingLawyer = Instantiate(lawyerPrefab);
                prosecutingLawyer.SetActive(false);
                Lawyer prosecutingLawyerComponent = prosecutingLawyer.GetComponent<Lawyer>();
                prosecutingLawyerComponent.correspondingTeam = Lawyer.Team.Prosecutors;
                prosecutorsTeamPool.Add(prosecutingLawyer);

                GameObject defendingLawyer = Instantiate(lawyerPrefab);
                defendingLawyer.SetActive(false);
                Lawyer defendingLawyerComponent = defendingLawyer.GetComponent<Lawyer>();
                defendingLawyerComponent.correspondingTeam = Lawyer.Team.Defendants;
                defendantsTeamPool.Add(defendingLawyer);
            }

            InitializeAI();
        }

        void InitializeAI()
        {
            foreach (GameObject prosecutor in prosecutorsTeamPool)
            {
                SetupAI(prosecutor);
            }

            foreach (GameObject defendant in defendantsTeamPool)
            {
                SetupAI(defendant);
            }
        }

        void SetupAI(GameObject lawyerObject)
        {
            Lawyer lawyer = lawyerObject.GetComponent<Lawyer>();
            if (lawyer != null)
            {
   
            }
            else
            {
                Debug.LogError("Lawyer component not found on GameObject: " + lawyerObject.name);
            }
        }

        IEnumerator ReinforcementCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(reinforcementInterval);

                ReinforceProsecutors();
                ReinforceDefendants();
            }
        }

        public void ReinforceProsecutors()
        {
            int count = 0;

            foreach (GameObject ProsecutingLawyer in prosecutorsTeamPool)
            {
                if (!ProsecutingLawyer.activeSelf && count < reinforcementSize)
                {
                    ProsecutingLawyer.SetActive(true);
                    ProsecutingLawyer.transform.position = spawnLocations[0];
                    count++;
                }
            }
        }

        public void ReinforceDefendants()
        {
            int count = 0;

            foreach (GameObject DefendingLawyer in defendantsTeamPool)
            {
                if (!DefendingLawyer.activeSelf && count < reinforcementSize)
                {
                    DefendingLawyer.SetActive(true);
                    DefendingLawyer.transform.position = spawnLocations[5];
                    count++;
                }
            }
        }
        #endregion
    }
}