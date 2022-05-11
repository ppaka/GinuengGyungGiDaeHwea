using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager manager;
    public SpriteRenderer sr;
    public Image hpImage, gotongImage;
    public Text hpText, gotongText, scoreText;
    public int score;
    public InputField hpInput, gotongInput;

    [Header("Player Stat")]
    public int maxHp = 100;
    public int hp = 100;
    public int maxGotong = 100;
    public int gotongGauge = 0;
    public int weaponLevel = 0;
    public float speed = 10f;

    [Header("Weapon Settings")]
    public float timeSinceLastFire;
    public float[] fireDelay;

    public float timeSinceLastHit = 10f;
    private float _invincibleTime = 1.5f;
    private float _invincibleEffectTime = 2.5f;
    private Camera _camera;
    public bool forceInvincible;
    public Animation anim;

    private void Start()
    {
        _camera = Camera.main;
    }

    public bool isInvicible()
    {
        if (timeSinceLastHit < _invincibleTime)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (!manager.gameStarted) return;

        Clock();
        InvinsibleEffect();

        if (!hpInput.gameObject.activeSelf && !gotongInput.gameObject.activeSelf)
        {
            if (Input.GetKey(KeyCode.X)) Fire();
            if (Input.GetKeyDown(KeyCode.Alpha1)) UpgradeWeapon(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) UpgradeWeapon(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) UpgradeWeapon(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) UpgradeWeapon(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) UpgradeWeapon(4);
            if (Input.GetKeyDown(KeyCode.F1))
            {
                manager.stageManager.boss1.gameObject.SetActive(false);
                manager.stageManager.boss2.gameObject.SetActive(false);
                manager.Setup(1);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                manager.stageManager.boss1.gameObject.SetActive(false);
                manager.stageManager.boss2.gameObject.SetActive(false);
                manager.Setup(2);
            }
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                forceInvincible = false;
                timeSinceLastHit = 1;
                _invincibleEffectTime = 1.5f;
                _invincibleTime = 1.5f;
            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                forceInvincible = true;
                timeSinceLastHit = 0;
                _invincibleEffectTime = float.MaxValue;
                _invincibleTime = float.MaxValue;
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                manager.enemyManager.KillAll();
                if (manager.currentStage == 1 && manager.stageManager.boss1.gameObject.activeSelf)
                {
                    manager.stageManager.boss1.anim.Stop();
                    manager.stageManager.boss1.GetDmg(manager.stageManager.boss1.maxHp);
                }
                else if (manager.currentStage == 2 && manager.stageManager.boss2.gameObject.activeSelf)
                {
                    manager.stageManager.boss2.anim.Stop();
                    manager.stageManager.boss2.GetDmg(manager.stageManager.boss2.maxHp);
                }
            }
            if (Input.GetKeyDown(KeyCode.Semicolon))
            {
                if (hpInput.gameObject.activeSelf || gotongInput.gameObject.activeSelf) return;
                hpInput.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Quote))
            {
                if (hpInput.gameObject.activeSelf || gotongInput.gameObject.activeSelf) return;
                gotongInput.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.O)) manager.enemyManager.Spawn((int)EnemyType.npc0);
            if (Input.GetKeyDown(KeyCode.P)) manager.enemyManager.Spawn((int)EnemyType.npc1);

            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * speed * Time.deltaTime;
        }

        var point = _camera.WorldToViewportPoint(transform.position);
        var x = Mathf.Clamp01(point.x);
        var y = Mathf.Clamp01(point.y);
        var convert = _camera.ViewportToWorldPoint(new Vector3(x, y, 0));
        convert.z = 0;
        transform.position = convert;

        hpImage.fillAmount = (float)hp / maxHp;
        hpText.text = hp.ToString();
        gotongImage.fillAmount = (float)gotongGauge / maxGotong;
        gotongText.text = gotongGauge.ToString();
        scoreText.text = score.ToString();
    }

    private void Clock()
    {
        timeSinceLastFire += Time.deltaTime;
        timeSinceLastHit += Time.deltaTime;
    }

    private void Fire()
    {
        if (timeSinceLastFire < fireDelay[weaponLevel]) return;
        manager.bulletManager.Spawn((BulletType)weaponLevel, transform.position);
        timeSinceLastFire = 0;
    }

    private void InvinsibleEffect()
    {
        if (timeSinceLastHit > _invincibleEffectTime)
        {
            var color = sr.color;
            color.a = 1;
            sr.color = color;
        }
        else
        {
            var color = sr.color;
            color.a = Mathf.Cos(timeSinceLastHit * 20f);
            sr.color = color;
        }
    }

    public void OnHpInputEndEdit(string value)
    {
        hp = int.Parse(value);
        hpInput.text = "";
        hpInput.gameObject.SetActive(false);
    }

    public void OnGotongInputEndEdit(string value)
    {
        gotongGauge = int.Parse(value);
        gotongInput.text = "";
        gotongInput.gameObject.SetActive(false);
    }

    public void HealHp(int amount)
    {
        hp = Mathf.Clamp(hp + amount, 0, maxHp);
    }

    public void HealGotong(int amount)
    {
        gotongGauge = Mathf.Clamp(gotongGauge - amount, 0, maxGotong);
    }

    public void InvinsibleByItem()
    {
        if (forceInvincible) return;
        timeSinceLastHit = 0;
        _invincibleTime = 3f;
        _invincibleEffectTime = 2.5f;
    }

    private void InvinsibleByEnemy()
    {
        timeSinceLastHit = 0;
        _invincibleTime = 1.5f;
        _invincibleEffectTime = 1.5f;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void GetDmg(int dmg)
    {
        if (isInvicible()) return;

        InvinsibleByEnemy();
        hp -= dmg;
        if (hp <= 0)
        {
            ScoreSaver.Instance.reason = GameOverReason.Hp;
            manager.gameStarted = false;
            manager.source.PlayOneShot(manager.clips[1]);
            anim.Play("PlayerDead");
        }
        else
        {
            manager.source.PlayOneShot(manager.clips[2], 0.7f);
        }
    }

    public void OnGameOver()
    {
        ScoreSaver.Instance.lastScore = score;
        SceneLoader.Instance.LoadScene("Result");
    }

    public void GetGotongDmg(int dmg)
    {
        gotongGauge += dmg;
        if (gotongGauge >= maxGotong)
        {
            ScoreSaver.Instance.reason = GameOverReason.Gotong;
            manager.gameStarted = false;
            manager.source.PlayOneShot(manager.clips[1]);
            anim.Play("PlayerDead");
        }
        else
        {
            manager.source.PlayOneShot(manager.clips[2], 0.6f);
        }
    }

    public void UpgradeWeapon(int lv)
    {
        if (lv > 4 || lv < 0) return;
        weaponLevel = lv;
    }

    public void UpgradeWeapon()
    {
        UpgradeWeapon(weaponLevel + 1);
    }

    public void FireBomb()
    {
        int rate = 60;
        manager.bulletManager.SpawnBomb(rate, transform.position);
    }
}
