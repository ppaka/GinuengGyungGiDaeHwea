using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameManager manager;
    public List<Enemy> spawnedEnemy = new List<Enemy>();
    public Enemy[] enemyPrefabs;

    [Header("SpawnPoint Settings")]
    public Transform[] endPointsOfSpawnPoints;
    private List<Vector2> _spawnPoints;
    public int spawnPointAmounts;
    [Header("Delay Settings")]
    public float timeSinceEntitySpawned;
    public float defaultSpawnDelay = 0.8f, additiveSpawnDelayRate = 1.5f;
    private float _currentSpawnDelay = 0.8f;

    public void KillAll()
    {
        foreach (var entity in spawnedEnemy)
        {
            if (entity.type != EnemyType.npc0 && entity.type != EnemyType.npc1)
            {
                if (manager.currentStage == 1)
                {
                    manager.stageManager.stage1LastEnemy--;
                }
                else if (manager.currentStage == 2)
                {
                    manager.stageManager.stage2LastEnemy--;
                }
            }
            Destroy(entity.gameObject);
        }
        spawnedEnemy.Clear();
    }

    private void Start()
    {
        _spawnPoints = new List<Vector2>();
        for (int i = 0; i < spawnPointAmounts; i++)
        {
            var vector = Vector2.Lerp(endPointsOfSpawnPoints[0].position, endPointsOfSpawnPoints[1].position, (float)i / spawnPointAmounts);
            _spawnPoints.Add(vector);
        }
    }

    private void Update()
    {
        if (!manager.gameStarted) return;

        if (manager.currentStage == 1 && manager.stageManager.stage1LastEnemy <= 0 && !manager.stageManager.boss1.gameObject.activeSelf)
        {
            manager.stageManager.SpawnBoss();
            KillAll();
            manager.bulletManager.KillAll();
            return;
        }
        if (manager.currentStage == 2 && manager.stageManager.stage2LastEnemy <= 0 && !manager.stageManager.boss2.gameObject.activeSelf)
        {
            manager.stageManager.SpawnBoss();
            KillAll();
            manager.bulletManager.KillAll();
            return;
        }
        if (!manager.stageManager.boss1.gameObject.activeSelf && !manager.stageManager.boss2.gameObject.activeSelf)
        {
            timeSinceEntitySpawned += Time.deltaTime;
            if (timeSinceEntitySpawned > _currentSpawnDelay)
            {
                SpawnRandom();
                timeSinceEntitySpawned = 0;
                _currentSpawnDelay = defaultSpawnDelay + additiveSpawnDelayRate * Random.Range(0f, 1f);
            }
        }

        foreach (var i in spawnedEnemy)
        {
            var pos = Vector3.LerpUnclamped(i.spawnedPos, i.endPos, manager.gameTime - i.spawnedTime);
            i.transform.position = pos;

            if (i.type != EnemyType.enemy0 && i.type != EnemyType.npc0 && i.type != EnemyType.npc1)
            {
                i.timeSinceLastFire += Time.deltaTime;
                if (i.timeSinceLastFire > i.stat.shootDelay)
                {
                    i.timeSinceLastFire = 0;
                    switch (i.type)
                    {
                        case EnemyType.enemy1:
                            manager.bulletManager.SpawnTriple(BulletType.enemy1, i.transform.position, i.stat.dmg);
                            break;
                        case EnemyType.enemy2:
                            manager.bulletManager.Spawn(BulletType.enemy2, i.transform.position, i.stat.dmg);
                            break;
                        case EnemyType.enemy3:
                            manager.bulletManager.Spawn(BulletType.enemy3, i.transform.position, i.stat.dmg);
                            break;
                    }

                }
            }
        }
    }

    public void SpawnRandom()
    {
        Spawn(Random.Range(0, enemyPrefabs.Length), _spawnPoints[Random.Range(0, _spawnPoints.Count)]);
    }

    public void Spawn(int type)
    {
        Spawn(type, _spawnPoints[Random.Range(0, _spawnPoints.Count)]);
    }

    public void Spawn(int type, Vector3 position)
    {
        var i = Instantiate(enemyPrefabs[type]);
        i.transform.position = position;
        i.enemyMgr = this;
        i.Setup(manager.gameTime, position);
        spawnedEnemy.Add(i);
    }
}
