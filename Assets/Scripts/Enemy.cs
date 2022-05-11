using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    enemy0,
    enemy1,
    enemy2,
    enemy3,
    npc0,
    npc1
}

[System.Serializable]
public class EnemyStat
{
    public int hp, maxHp;
    public int dmg;
    public float speed;
    public float shootDelay = 1f;
}

public class Enemy : MonoBehaviour
{
    public EnemyType type;
    public EnemyStat stat;
    public EnemyManager enemyMgr;

    public float spawnedTime, timeSinceLastFire;

    public Vector3 spawnedPos, endPos;
    private bool _alreadyTriggered;

    public void Setup(float time, Vector3 spawnedPos)
    {
        var endPos = new Vector3(0, spawnedPos.y, 0) + -transform.up * stat.speed;
        endPos.x = spawnedPos.x;
        Setup(time, spawnedPos, endPos);
    }

    public void Setup(float time, Vector3 spawnedPos, Vector3 endPos)
    {
        spawnedTime = time;
        this.spawnedPos = spawnedPos;
        this.endPos = endPos;
        stat.hp = stat.maxHp;
    }

    public void GetDmg(int dmg)
    {
        stat.hp -= dmg;
        if (stat.hp <= 0)
        {
            switch (type)
            {
                case EnemyType.npc0:
                    if (_alreadyTriggered) return;
                    enemyMgr.manager.player.AddScore(stat.maxHp * 100);
                    enemyMgr.manager.itemManager.SpawnRandom(transform.position);
                    _alreadyTriggered = true;
                    enemyMgr.manager.source.PlayOneShot(enemyMgr.manager.clips[5]);
                    break;
                case EnemyType.npc1:
                    if (_alreadyTriggered) return;
                    enemyMgr.manager.player.GetGotongDmg(10);
                    _alreadyTriggered = true;
                    enemyMgr.manager.source.PlayOneShot(enemyMgr.manager.clips[4]);
                    break;
                default:
                    enemyMgr.manager.player.AddScore(stat.maxHp * 100);
                    enemyMgr.manager.stageManager.OnCatchEnemy();
                    enemyMgr.manager.source.PlayOneShot(enemyMgr.manager.clips[3], 0.95f);
                    break;
            }

            Destroy(gameObject);
        }
        else
        {
            enemyMgr.manager.source.PlayOneShot(enemyMgr.manager.clips[6], 0.6f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (type == EnemyType.npc0)
            {
                if (_alreadyTriggered) return;
                if (enemyMgr.manager.player.forceInvincible) return;
                enemyMgr.manager.player.AddScore(stat.maxHp * 100);
                enemyMgr.manager.itemManager.SpawnRandom(transform.position);
                _alreadyTriggered = true;
                Destroy(gameObject);
            }
            else if (type == EnemyType.npc1)
            {
                if (_alreadyTriggered) return;
                if (enemyMgr.manager.player.forceInvincible || enemyMgr.manager.player.isInvicible()) return;
                enemyMgr.manager.player.GetGotongDmg(10);
                _alreadyTriggered = true;
                Destroy(gameObject);
            }
            else
            {
                enemyMgr.manager.player.GetDmg(Mathf.FloorToInt(stat.dmg / 2f));
            }
        }
        else if (collision.CompareTag("BulletBorder"))
        {
            if (type == EnemyType.npc0 || type == EnemyType.npc1)
            {
                Destroy(gameObject);
                return;
            }
            enemyMgr.manager.player.GetGotongDmg(Mathf.FloorToInt(stat.dmg / 2f));
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        enemyMgr.spawnedEnemy.Remove(this);
    }
}
