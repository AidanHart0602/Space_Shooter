using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _secondEnemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _endSpawn = false;
    [SerializeField]
    private GameObject[] powerUp;
    [SerializeField]
    private GameObject _ammoCollectable;

    private UiManager _uiManager;
    public int currentWave = 0;
    public int enemyLimit = 0;
    public int enemiesLeft = 0;
    public bool waveStart = false;
    private float _spawnIncrease = 0f;

    void Start()
    {
        currentWave = 0;
        waveStart = false;

        _uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI MANAGER IN SPAWN MANAGER IS NULL");
        }
    }

    private void Update()
    {
        if(enemiesLeft == 0 && waveStart == true)
        {
            StopAllCoroutines();
            EndWave(); 
        }
    }
    public void StartSpawn()
    {
        StartCoroutine(EnemySpawner());
        StartCoroutine(PowerUpSpawner());
        StartCoroutine(AmmoBoxSpawner());
        StartCoroutine(RarePowerUps());
        waveStart = false;
        enemiesLeft = enemyLimit;
    }
    private void StartWave()
    {
        StartCoroutine(WaveCooldown());
        _endSpawn = false;
        enemiesLeft = enemyLimit;
        StartSpawn();
    }

    public void EndWave()
    {
        currentWave += 1;
        enemyLimit += 10;
        _spawnIncrease += 0.2f;
        _uiManager.UpdateWaves(currentWave);
        StartWave();
    }

    IEnumerator EnemySpawner()
    {
        int enemiesSpawned = 0;
        yield return new WaitForSeconds(0.5f);
        while (_endSpawn == false)
        {
            if(enemiesSpawned != enemyLimit)
            {
                Vector3 NewPosition = new Vector3((Random.Range(-10.3f, 10.3f)), 7, 0);
                float zAngle = Random.Range(-45f, 45f);
                GameObject newEnemy = Instantiate(_enemyPrefab, NewPosition, Quaternion.Euler(0, 0, zAngle));
                if (currentWave >= 2)
                {
                    Vector3 position = new Vector3((Random.Range(-10.3f, 10.3f)),7, 0);
                   Instantiate(_secondEnemyPrefab, position, Quaternion.identity);
                    enemiesSpawned++;
                }
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(2.5f - _spawnIncrease);
                enemiesSpawned++;
            }

            if (enemiesSpawned == enemyLimit)
            {
                _endSpawn = true;
                waveStart = true;
                enemiesSpawned = 0;
            }
            yield return new WaitForSeconds(1.0f);           
        }
    }

    IEnumerator WaveCooldown() 
    {
        yield return new WaitForSeconds(2.0f);
    }

    IEnumerator PowerUpSpawner()
    {
        yield return new WaitForSeconds(0.5f);
        while (_endSpawn == false)
        {
            Vector3 NewPosition = new Vector3((Random.Range(-10.3f, 10.3f)), 7, 0);
            int RandomPowerUp = Random.Range(0, 4);
            GameObject newEnemy = Instantiate(powerUp[RandomPowerUp], NewPosition, Quaternion.identity);
            yield return new WaitForSeconds(10f);
        }
    }
    IEnumerator AmmoBoxSpawner()
    {
        yield return new WaitForSeconds(5f);
        while(_endSpawn == false)
        {
            Vector3 NewPosition = new Vector3((Random.Range(-10.3f, 10.3f)), 7, 0);
            GameObject newEnemy = Instantiate(_ammoCollectable, NewPosition, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
        
    }
    IEnumerator RarePowerUps() 
    {
        float randomSpawn = Random.Range(20f, 40f);
        yield return new WaitForSeconds(40f);
        while(_endSpawn == false)
        {
            int HealthAndGiantLaser = Random.Range(5, 6);
            Vector3 NewPosition = new Vector3((Random.Range(-10.3f, 10.3f)), 7, 0);
            GameObject newEnemy = Instantiate(powerUp[HealthAndGiantLaser], NewPosition, Quaternion.identity);
            yield return new WaitForSeconds(randomSpawn);
        }
    }
    public void PlayerDeath()
    {
        _endSpawn = true;
    }
}