using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _endSpawn = false;
    [SerializeField]
    private GameObject[] powerup;

    // Update is called once per frame
    public void StartSpawn()
    {
        
        StartCoroutine(EnemySpawner());
        StartCoroutine(PowerUpSpawner());
    }

    IEnumerator EnemySpawner()
    {
        yield return new WaitForSeconds(0.5f);
        while (_endSpawn == false)
        {
            Vector3 NewPosition = new Vector3((Random.Range(-10.3f, 10.3f)), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, NewPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.5f);
        }
    }

    IEnumerator PowerUpSpawner()
    {
        yield return new WaitForSeconds(0.5f);
        while (_endSpawn == false)
        {
            Vector3 NewPosition = new Vector3((Random.Range(-10.3f, 10.3f)), 7, 0);
            int RandomPowerUp = Random.Range(0, 3);
            GameObject newEnemy = Instantiate(powerup[RandomPowerUp], NewPosition, Quaternion.identity);
            yield return new WaitForSeconds(10.0f);
        }
    }
    public void PlayerDeath()
    {
        _endSpawn = true;
    }
}