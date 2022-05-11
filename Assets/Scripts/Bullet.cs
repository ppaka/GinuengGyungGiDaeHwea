using UnityEngine;

public enum BulletOwner
{
    Player,
    Enemy
}

[System.Serializable]
public class BulletStat
{
    public float speed;
    public int dmg;
}

public class Bullet : MonoBehaviour
{
    public BulletManager bulletMgr;

    public BulletOwner owner;
    public BulletStat stat;
    public float spawnedTime;
    public Vector3 spawnedPos, endPos;

    public void Setup(float time, Vector3 spawnedPos)
    {
        if (owner == BulletOwner.Player)
        {
            var endPos = new Vector3(0, spawnedPos.y, 0) + transform.up * stat.speed;
            endPos.x = spawnedPos.x;
            Setup(time, spawnedPos, endPos);
        }
        else
        {
            var endPos = new Vector3(0, spawnedPos.y, 0) - transform.up * stat.speed;
            endPos.x = spawnedPos.x;
            Setup(time, spawnedPos, endPos);
        }
    }

    public void Setup(float time, Vector3 spawnedPos, Vector3 endPos)
    {
        spawnedTime = time;
        this.spawnedPos = spawnedPos;
        this.endPos = endPos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (owner == BulletOwner.Player) return;
            if (bulletMgr.manager.player.forceInvincible) return;
            Destroy(gameObject);
            bulletMgr.manager.player.GetDmg(stat.dmg);
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (owner == BulletOwner.Enemy) return;
            Destroy(gameObject);
            collision.gameObject.GetComponent<Enemy>().GetDmg(stat.dmg);
        }
        else if (collision.CompareTag("BulletBorder"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("BossOne"))
        {
            if (owner == BulletOwner.Enemy) return;
            Destroy(gameObject);
            bulletMgr.manager.stageManager.boss1.GetDmg(stat.dmg);
        }
        else if (collision.CompareTag("BossTwo"))
        {
            if (owner == BulletOwner.Enemy) return;
            Destroy(gameObject);
            bulletMgr.manager.stageManager.boss2.GetDmg(stat.dmg);
        }
    }

    private void OnDestroy()
    {
        bulletMgr.spawnedBullets.Remove(this);
    }
}
