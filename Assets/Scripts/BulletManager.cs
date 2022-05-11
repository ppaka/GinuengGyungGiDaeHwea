using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    player0,
    player1,
    player2,
    player3,
    player4,
    enemy1,
    enemy2,
    enemy3,
    boss1,
    boss2,
    boss2Circle,
}

public class BulletManager : MonoBehaviour
{
    public GameManager manager;
    public Bullet[] bulletPrefabs;
    public List<Bullet> spawnedBullets = new List<Bullet>();

    public void KillAll()
    {
        foreach (var i in spawnedBullets)
        {
            Destroy(i.gameObject);
        }
        spawnedBullets.Clear();
    }

    public void Spawn(BulletType bulletType, Vector3 position)
    {
        var i = Instantiate(bulletPrefabs[(int)bulletType]);
        i.bulletMgr = this;
        i.transform.position = position;
        i.Setup(manager.gameTime, position);
        spawnedBullets.Add(i);
    }

    public void Spawn(BulletType bulletType, Vector3 position, int dmg)
    {
        var i = Instantiate(bulletPrefabs[(int)bulletType]);
        i.bulletMgr = this;
        i.transform.position = position;
        i.stat.dmg = dmg;
        i.Setup(manager.gameTime, position);
        spawnedBullets.Add(i);
    }

    public void SpawnBomb(int rate, Vector3 position)
    {
        for (int i = 0; i < rate; i++)
        {
            var cache = Instantiate(bulletPrefabs[(int)BulletType.player1]);
            cache.transform.position = position;
            cache.transform.Rotate(new Vector3(0, 0, 360f * i / rate - 90));

            cache.spawnedPos = position;
            cache.endPos = position + (cache.transform.localRotation * -cache.transform.up) * cache.stat.speed;
            cache.bulletMgr = this;
            cache.spawnedTime = manager.gameTime;
            spawnedBullets.Add(cache);
        }
    }

    public void SpawnTriple(BulletType type, Vector3 position, int dmg)
    {
        for (int i = 0; i < 3; i++)
        {
            var cache = Instantiate(bulletPrefabs[(int)type]);
            cache.transform.position = position;
            cache.transform.Rotate(new Vector3(0, 0, 45f * i / 3f - 15f));

            cache.stat.dmg = dmg;
            cache.spawnedPos = position;
            cache.endPos = position + (cache.transform.localRotation * -cache.transform.up) * cache.stat.speed;
            cache.bulletMgr = this;
            cache.spawnedTime = manager.gameTime;
            spawnedBullets.Add(cache);
        }
    }

    public void SpawnTriple(BulletType type, Vector3 position, int dmg, float angle, int rate, Vector3 toDir)
    {
        for (int i = 0; i < rate; i++)
        {
            var cache = Instantiate(bulletPrefabs[(int)type]);
            cache.transform.position = position;

            int m = 1;
            if (toDir.x < 0) m = -1;

            var ang = Vector3.Angle(Vector3.down, toDir.normalized);
            cache.transform.Rotate(new Vector3(0, 0, angle / 2 * i / (rate - 1) - angle / 4 + m * ang / 2));

            cache.stat.dmg = dmg;
            cache.spawnedPos = position;
            cache.endPos = position + cache.transform.localRotation * -cache.transform.up * cache.stat.speed;
            cache.bulletMgr = this;
            cache.spawnedTime = manager.gameTime;
            spawnedBullets.Add(cache);
        }
    }

    public void SpawnTriple(BulletType type, Vector3 position, int dmg, float additiveRotation)
    {
        for (int i = 0; i < 3; i++)
        {
            var cache = Instantiate(bulletPrefabs[(int)type]);
            cache.transform.position = position;
            cache.transform.Rotate(new Vector3(0, 0, 45f * i / 3f - 15f + additiveRotation));

            cache.stat.dmg = dmg;
            cache.spawnedPos = position;
            cache.endPos = position + (cache.transform.localRotation * -cache.transform.up) * cache.stat.speed;
            cache.bulletMgr = this;
            cache.spawnedTime = manager.gameTime;
            spawnedBullets.Add(cache);
        }
    }

    public void SpawnCircle(BulletType type, int rate, Vector3 position, float additiveRotation)
    {
        for (int i = 0; i < rate; i++)
        {
            var cache = Instantiate(bulletPrefabs[(int)type]);
            cache.transform.position = position;
            cache.transform.Rotate(new Vector3(0, 0, 360f * i / rate - 90 + additiveRotation));

            cache.spawnedPos = position;
            cache.endPos = position + (cache.transform.localRotation * -cache.transform.up) * cache.stat.speed;
            cache.bulletMgr = this;
            cache.spawnedTime = manager.gameTime;
            spawnedBullets.Add(cache);
        }
    }

    public void SpawnTriple(BulletType type, Vector3 position)
    {
        for (int i = 0; i < 3; i++)
        {
            var cache = Instantiate(bulletPrefabs[(int)type]);
            cache.transform.position = position;
            cache.transform.Rotate(new Vector3(0, 0, 45f * i / 3f - 15f));

            cache.spawnedPos = position;
            cache.endPos = position + (cache.transform.localRotation * -cache.transform.up) * cache.stat.speed;
            cache.bulletMgr = this;
            cache.spawnedTime = manager.gameTime;
            spawnedBullets.Add(cache);
        }
    }

    public void ShootPlayer(BulletType type, Vector3 position)
    {
        var dir = (manager.player.transform.position - position).normalized;
        var cache = Instantiate(bulletPrefabs[(int)type]);
        cache.transform.position = position;

        cache.spawnedPos = position;
        cache.endPos = position + dir * cache.stat.speed;
        cache.bulletMgr = this;
        cache.spawnedTime = manager.gameTime;
        spawnedBullets.Add(cache);
    }

    private void Update()
    {
        foreach (var i in spawnedBullets)
        {
            var pos = Vector3.LerpUnclamped(i.spawnedPos, i.endPos, manager.gameTime - i.spawnedTime);
            i.transform.position = pos;
        }
    }
}