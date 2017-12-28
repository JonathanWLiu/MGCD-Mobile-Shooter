using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour {
    [System.Serializable]
    public class Stages{
        public float spawnCooldown;
        public float spawnNumber;
        public float waveLength;
    }

    [SerializeField]
    private GameObject zombiePrefab;
    [SerializeField]
    private Stages[] stages;
    [SerializeField]
    private float spawnOffset;

    private int curStage = 0;

    private Coroutine zombieSpawnCoroutine;


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnZombies()
    {
        int spawnPos = 0;
        Vector3 pos = Vector3.zero;
        
        for (int i = 0; i < stages[curStage].spawnNumber; i++)
        {
            spawnPos = Random.Range(0, 7);
            switch (spawnPos)
            {
                case 0:
                    pos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10));
                    pos += new Vector3(-spawnOffset, -spawnOffset);
                    break;
                case 1:
                    pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 10));
                    pos += new Vector3(0, -spawnOffset);
                    break;
                case 2:
                    pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10));
                    pos += new Vector3(spawnOffset, 0);
                    break;
                case 3:
                    pos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 10));
                    pos += new Vector3(-spawnOffset, 0);
                    break;
                case 4:
                    pos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 10));
                    pos += new Vector3(-spawnOffset, spawnOffset);
                    break;
                case 5:
                    pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));
                    pos += new Vector3(spawnOffset, spawnOffset);
                    break;
                case 6:
                    pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 10));
                    pos += new Vector3(spawnOffset, 0);
                    break;
                case 7:
                    pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 10));
                    pos += new Vector3(0, spawnOffset);
                    break;
            }
            Instantiate(zombiePrefab, pos, Quaternion.identity);
        }
        yield return new WaitForSeconds(stages[curStage].spawnCooldown);
        zombieSpawnCoroutine = StartCoroutine(SpawnZombies());
    }

    IEnumerator IncreaseStage()
    {
        yield return new WaitForSeconds(stages[curStage].waveLength);
        curStage++;
        if (stages.Length - 1 > curStage)
        {
            StartCoroutine(IncreaseStage());
        }
    }
    
    public void StartSpawning()
    {
        zombieSpawnCoroutine = StartCoroutine(SpawnZombies());
        curStage = 0;
        if (stages.Length - 1 > curStage)
        {
            StartCoroutine(IncreaseStage());
        }
    }
    public void StopSpawning()
    {
        StopCoroutine(zombieSpawnCoroutine);
    }
}
