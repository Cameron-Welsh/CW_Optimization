using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class GameStateManager : MonoBehaviour
    {
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
            spawnLocations[0].y = 0.0f;
            spawnLocations[1].x = -14.0f;
            spawnLocations[1].y = -1.0f;
            spawnLocations[2].x = -14.0f;
            spawnLocations[2].y = -2.0f;
            spawnLocations[3].x = -14.0f;
            spawnLocations[3].y = -3.0f;
            spawnLocations[4].x = -12.0f;
            spawnLocations[4].y = -4.0f;
            spawnLocations[5].x = 12.0f;
            spawnLocations[5].y = 0.0f;
            spawnLocations[6].x = 14.0f;
            spawnLocations[6].y = -1.0f;
            spawnLocations[7].x = 14.0f;
            spawnLocations[7].y = -2.0f;
            spawnLocations[8].x = 14.0f;
            spawnLocations[8].y = -3.0f;
            spawnLocations[9].x = 12.0f;
            spawnLocations[9].y = -4.0f;

            currentArena = Arena.Courtroom;
            spawnManager = GetComponent<SpawnManager>();

            InitializeTeamPools();
            reinforcementCoroutine = StartCoroutine(ReinforcementCoroutine());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                changeLevel();
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

        public void changeLevel()
        {
            if (leftVictory)
            {
                screenSwipeMaterial.SetFloat("_SwipeDirection", 1.0f);
                StartCoroutine(ChangeBackgrounds());
            }
        }

        IEnumerator ChangeBackgrounds() //LOOK HERE! I never disabled the screenSwipePrefab, which i totally should. Its always active with a complex shader on it. Not good!
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
        void InitializeTeamPools() //I already object pooled but I can act like i didn't
        {
            prosecutorsTeamPool = new List<GameObject>();
            defendantsTeamPool = new List<GameObject>();

            for (int i = 0; i < teamSize; i++)
            {
                GameObject ProsecutingLawyer = Instantiate(lawyerPrefab);
                ProsecutingLawyer.SetActive(false);
                Lawyer prosecutingLawyerComponent = ProsecutingLawyer.GetComponent<Lawyer>();
                prosecutingLawyerComponent.correspondingTeam = Lawyer.Team.Prosecutors;
                prosecutorsTeamPool.Add(ProsecutingLawyer);

                GameObject DefendingLawyer = Instantiate(lawyerPrefab);
                DefendingLawyer.SetActive(false);
                Lawyer defendingLawyerComponent = DefendingLawyer.GetComponent<Lawyer>();
                defendingLawyerComponent.correspondingTeam = Lawyer.Team.Defendants;
                defendantsTeamPool.Add(DefendingLawyer);
            }
        }

        //public void UpdateSpawnLocations()
        //{
        //    switch (currentArena)
        //    {
        //        case Arena.Courtroom:
        //            spawnLocations[0].x = -12.0f;
        //            spawnLocations[0].y = 0.0f;
        //            spawnLocations[1].x = -14.0f;
        //            spawnLocations[1].y = -1.0f;
        //            spawnLocations[2].x = -14.0f;
        //            spawnLocations[2].y = -2.0f;
        //            spawnLocations[3].x = -14.0f;
        //            spawnLocations[3].y = -3.0f;
        //            spawnLocations[4].x = -12.0f;
        //            spawnLocations[4].y = -4.0f;
        //            spawnLocations[5].x = 12.0f;
        //            spawnLocations[5].y = 0.0f;
        //            spawnLocations[6].x = 14.0f;
        //            spawnLocations[6].y = -1.0f;
        //            spawnLocations[7].x = 14.0f;
        //            spawnLocations[7].y = -2.0f;
        //            spawnLocations[8].x = 14.0f;
        //            spawnLocations[8].y = -3.0f;
        //            spawnLocations[9].x = 12.0f;
        //            spawnLocations[9].y = -4.0f;
        //            break;
        //        case Arena.DefendantsHall:
        //            spawnLocations[0].x = -12.0f;
        //            spawnLocations[0].y = 0.0f;
        //            spawnLocations[1].x = -14.0f;
        //            spawnLocations[1].y = -1.0f;
        //            spawnLocations[2].x = -14.0f;
        //            spawnLocations[2].y = -2.0f;
        //            spawnLocations[3].x = -14.0f;
        //            spawnLocations[3].y = -3.0f;
        //            spawnLocations[4].x = -12.0f;
        //            spawnLocations[4].y = -4.0f;
        //            spawnLocations[5].x = 12.0f;
        //            spawnLocations[5].y = 0.0f;
        //            spawnLocations[6].x = 14.0f;
        //            spawnLocations[6].y = -1.0f;
        //            spawnLocations[7].x = 14.0f;
        //            spawnLocations[7].y = -2.0f;
        //            spawnLocations[8].x = 14.0f;
        //            spawnLocations[8].y = -3.0f;
        //            spawnLocations[9].x = 12.0f;
        //            spawnLocations[9].y = -4.0f;
        //            break;
        //        case Arena.ProsecutorHall:
        //            spawnLocations[0].x = -22.0f;
        //            spawnLocations[0].y = 0.0f;
        //            spawnLocations[1].x = -24.0f;
        //            spawnLocations[1].y = -1.0f;
        //            spawnLocations[2].x = -24.0f;
        //            spawnLocations[2].y = -2.0f;
        //            spawnLocations[3].x = -24.0f;
        //            spawnLocations[3].y = -3.0f;
        //            spawnLocations[4].x = -22.0f;
        //            spawnLocations[4].y = -4.0f;
        //            spawnLocations[5].x = 22.0f;
        //            spawnLocations[5].y = 0.0f;
        //            spawnLocations[6].x = 24.0f;
        //            spawnLocations[6].y = -1.0f;
        //            spawnLocations[7].x = 24.0f;
        //            spawnLocations[7].y = -2.0f;
        //            spawnLocations[8].x = 24.0f;
        //            spawnLocations[8].y = -3.0f;
        //            spawnLocations[9].x = 22.0f;
        //            spawnLocations[9].y = -4.0f;
        //            break;
        //        default:
        //            Debug.Log("Unrecognized Arena");
        //            break;
        //    }

        //}

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
                    ProsecutingLawyer.transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)];
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
                    Vector3 randomSpawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
                    DefendingLawyer.transform.position = randomSpawnLocation;
                    DefendingLawyer.SetActive(true);
                    count++;
                }
            }
        }
        #endregion
    }
}
