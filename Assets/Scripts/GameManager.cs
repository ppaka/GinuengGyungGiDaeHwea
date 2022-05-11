using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public BulletManager bulletManager;
    public EnemyManager enemyManager;
    public ItemManager itemManager;
    public StageManager stageManager;
    public Player player;
    public float gameTime;
    public bool gameStarted;
    public int currentStage = 0;
    public RawImage backgroundImage, backgroundImage2;
    public Text stageText, anyKeyText;
    public Image fillImg1, fillImg2;
    public GameObject playerCanvas;
    public AudioSource source;
    public AudioClip[] clips;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void StageTimer()
    {
        StartCoroutine(coroutine());
    }

    IEnumerator coroutine()
    {
        playerCanvas.SetActive(false);
        gameStarted = false;
        stageText.text = "Stage " + currentStage;

        fillImg1.gameObject.SetActive(true);
        fillImg2.gameObject.SetActive(true);
        stageText.gameObject.SetActive(true);
        yield return null;
        fillImg1.fillAmount = 1;
        fillImg2.fillAmount = 1;
        float delay = 1f;

        while (delay > 0)
        {
            delay -= Time.deltaTime * 0.25f;
            fillImg1.fillAmount = delay;
            fillImg2.fillAmount = delay;
            yield return null;
        }
        fillImg1.gameObject.SetActive(false);
        fillImg2.gameObject.SetActive(false);
        stageText.gameObject.SetActive(false);

        if (currentStage == 1)
        {
            gameStarted = true;
            player.hp = 100;
            player.gotongGauge = 10;
        }
        else if (currentStage == 2)
        {
            gameStarted = true;
            player.hp = 100;
            player.gotongGauge = 30;
        }
        stageManager.OnReset();
        playerCanvas.SetActive(true);
    }

    public void Setup(int stageNum)
    {
        player.anim.Stop();
        stageManager.boss1.anim.Stop();
        stageManager.boss2.anim.Stop();
        stageManager.boss1.gameObject.SetActive(false);
        stageManager.boss2.gameObject.SetActive(false);
        enemyManager.KillAll();
        bulletManager.KillAll();
        itemManager.DestroyAll();
        currentStage = stageNum;
        if (currentStage == 1)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage2.gameObject.SetActive(false);
            player.anim.Play("PlayerSpawn");
        }
        else if (currentStage == 2)
        {
            backgroundImage2.gameObject.SetActive(true);
            backgroundImage.gameObject.SetActive(false);
        }
        if (Time.timeScale == 0) StartCoroutine(timeTween());
        if (anyKeyText.gameObject.activeSelf) anyKeyText.gameObject.SetActive(false);
        StageTimer();
    }

    private IEnumerator timeTween()
    {
        while (Time.timeScale < 1)
        {
            Time.timeScale += 0.02f * Time.fixedUnscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 1;

        yield return null;
    }

    void Update()
    {
        if (Input.anyKeyDown && currentStage == 0)
        {
            source.PlayOneShot(clips[0]);
            Setup(1);
        }
        if (gameStarted) gameTime += Time.deltaTime;
        var uvRect = backgroundImage.uvRect;
        uvRect.y += Time.deltaTime * 0.02f;
        backgroundImage.uvRect = uvRect;

        var uvRect2 = backgroundImage2.uvRect;
        uvRect2.y += Time.deltaTime * 0.014f;
        backgroundImage2.uvRect = uvRect2;
    }
}
