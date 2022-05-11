using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameManager manager;
    public BossOne boss1;
    public BossTwo boss2;
    public Text missionText;

    public float stage1LastEnemy = 20, stage1MaxEnemy = 20;
    public float stage2LastEnemy = 40, stage2MaxEnemy = 40;

    public bool bossSpawned;

    public void SpawnBoss()
    {
        if (manager.currentStage == 1)
        {
            bossSpawned = true;
            boss1.gameObject.SetActive(true);
        }
        else if (manager.currentStage == 2)
        {
            bossSpawned = true;
            boss2.gameObject.SetActive(true);
        }
    }

    public void OnBossDead()
    {
        if (manager.currentStage == 1)
        {
            manager.Setup(2);
        }
        else if (manager.currentStage == 2)
        {
            manager.gameStarted = false;
            ScoreSaver.Instance.reason = GameOverReason.Clear;
            ScoreSaver.Instance.lastScore = manager.player.score;
            SceneLoader.Instance.LoadScene("Result");
        }
    }

    private void Update()
    {
        if (bossSpawned) missionText.text = "���� ���";
        if (manager.currentStage == 1)
        {
            if (bossSpawned) missionText.text = "���� ���";
            else missionText.text = "�� " + stage1LastEnemy + "/" + stage1MaxEnemy + " ���� óġ�ϱ�";
        }
        else if (manager.currentStage == 2)
        {
            if (bossSpawned) missionText.text = "���� ���";
            else missionText.text = "�� " + stage2LastEnemy + "/" + stage2MaxEnemy + " ���� óġ�ϱ�";
        }
    }

    public void OnReset()
    {
        bossSpawned = false;
        stage1LastEnemy = stage1MaxEnemy;
        stage2LastEnemy = stage2MaxEnemy;
    }

    public void OnCatchEnemy()
    {
        if (manager.currentStage == 1)
        {
            stage1LastEnemy--;
        }
        else if (manager.currentStage == 2)
        {
            stage2LastEnemy--;
        }
    }
}
