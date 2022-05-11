using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossTwo : MonoBehaviour
{
    public GameManager manager;
    public Animation anim;
    public Transform[] canons;
    public bool alive = true;
    private bool _spawnCompletly;
    public int maxHp = 30, hp;
    public CanvasGroup cg;
    public Text hpText;
    public Image hpImage;

    private void OnEnable()
    {
        _spawnCompletly = false;
        hp = maxHp;
        anim.Play("BossSpawnTwo");
        cg.gameObject.SetActive(true);
    }

    private void Update()
    {
        hpText.text = hp + "/" + maxHp;
        hpImage.fillAmount = (float)hp / maxHp;

        var dir = manager.player.transform.position - transform.position;
        Debug.DrawRay(transform.position, dir);
    }

    public void GetDmg(int dmg)
    {
        if (!_spawnCompletly) return;
        hp -= dmg;
        if (hp <= 0)
        {
            if (!alive) return;
            manager.player.AddScore(300000);
            manager.player.AddScore((manager.player.hp + Mathf.Abs(manager.player.gotongGauge - manager.player.maxGotong)) * 100);
            alive = false;
            cg.gameObject.SetActive(false);
            anim.Play("BossDeadTwo");
            manager.source.PlayOneShot(manager.clips[7], 0.6f);
        }
        else
        {
            manager.source.PlayOneShot(manager.clips[6], 0.6f);
        }
    }

    public void CallOnBossDead()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        manager.stageManager.OnBossDead();
    }

    public void StartPattern()
    {
        _spawnCompletly = true;
        StartCoroutine(Pattern());
    }

    private IEnumerator SpawnRandomly()
    {
        while (alive)
        {
            manager.enemyManager.SpawnRandom();
            yield return new WaitForSeconds(4);
        }
    }

    private IEnumerator Pattern()
    {
        StartCoroutine(SpawnRandomly());
        while (alive)
        {
            StartCoroutine(ShootCircle(15, 40));
            yield return StartCoroutine(ShootToPlayer(50));
            yield return new WaitForSeconds(2);
            yield return StartCoroutine(ShootTriple(10, 5));
            StartCoroutine(ShootCircle(4, 50));
            yield return StartCoroutine(ShootToPlayer(50));
            StartCoroutine(ShootTriple(10, 5));
            StartCoroutine(ShootCircle(10, 48, 55));
            yield return StartCoroutine(ShootToPlayerSide(100));
            yield return new WaitForSeconds(3);
        }
        yield return null;
    }

    private IEnumerator ShootTriple(int count, int rate)
    {
        for (int i = 0; i < count; i++)
        {
            var dir = (manager.player.transform.position - transform.position).normalized;
            manager.bulletManager.SpawnTriple(BulletType.boss2, transform.position, 6, 60, rate, dir);
            yield return new WaitForSeconds(1f);
        }
    }


    private IEnumerator ShootCircle(int count, int rate, float anglesAddEveryCount = 30f)
    {
        for (int i = 0; i < count; i++)
        {
            manager.bulletManager.SpawnCircle(BulletType.boss2, rate, transform.position, anglesAddEveryCount * i);
            yield return new WaitForSeconds(1.5f);
        }
    }

    private IEnumerator ShootToPlayerSide(int rate)
    {
        for (int i = 0; i < rate; i++)
        {
            manager.bulletManager.ShootPlayer(BulletType.boss2, canons[0].position);
            manager.bulletManager.ShootPlayer(BulletType.boss2, canons[1].position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ShootToPlayer(int rate)
    {
        for (int i = 0; i < rate; i++)
        {
            manager.bulletManager.ShootPlayer(BulletType.boss2, transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
