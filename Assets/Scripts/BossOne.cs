using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossOne : MonoBehaviour
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
        anim.Play("BossSpawn");
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
            manager.player.AddScore(100000);
            manager.player.AddScore((manager.player.hp + Mathf.Abs(manager.player.gotongGauge - manager.player.maxGotong)) * 100);
            alive = false;
            cg.gameObject.SetActive(false);
            anim.Play("BossDead");
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
            yield return new WaitForSeconds(3);
        }
    }

    private IEnumerator Pattern()
    {
        StartCoroutine(SpawnRandomly());
        while (alive)
        {
            yield return StartCoroutine(ShootToPlayer(50));
            yield return new WaitForSeconds(2);
            yield return StartCoroutine(ShootToPlayerSide(100));
            yield return new WaitForSeconds(3);
        }
        yield return null;
    }
    private IEnumerator ShootToPlayerSide(int rate)
    {
        for (int i = 0; i < rate; i++)
        {
            manager.bulletManager.ShootPlayer(BulletType.boss1, canons[0].position);
            manager.bulletManager.ShootPlayer(BulletType.boss1, canons[1].position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ShootToPlayer(int rate)
    {
        for (int i = 0; i < rate; i++)
        {
            manager.bulletManager.ShootPlayer(BulletType.boss1, transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
