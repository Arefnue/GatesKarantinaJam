using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private int _spawnRate;
    public int firstSpawn;
    public int minSpawnRate;
    public int maxSpawnRate;
    //public GameObject spawnParticle;
    
    private void Start()
    {
        _spawnRate = firstSpawn;
        StartCoroutine(Spawn());
        
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);
            //Instantiate(spawnParticle, transform.position, Quaternion.identity);
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            _spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        }
        
    }
    
}
