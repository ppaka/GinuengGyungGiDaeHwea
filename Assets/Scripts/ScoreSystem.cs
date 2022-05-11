using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public InputField inputField;
    public Transform parent;
    public Text recentScoreText;
    public List<ScoreListItem> items = new List<ScoreListItem>();
    public ScoreListItem prefab;
    private bool _canSave;
    public Animation anim;
    public Text deadreasonText;
    public AudioSource source;
    public AudioClip clip;

    void Test()
    {
        ScoreSaver.Instance.Save("test", 123);
        ScoreSaver.Instance.Save("dick", 555);
        ScoreSaver.Instance.Save("aaaa", 900);
        ScoreSaver.Instance.Save("cc", 111555);
        ScoreSaver.Instance.Save("asdf", 44);
        ScoreSaver.Instance.Save("wwww", 11);
        //Print();
        ScoreSaver.Instance.Clean();
        //Print();
        ScoreSaver.Instance.lastScore = 500;
    }

    public void ChangeScene()
    {
        source.PlayOneShot(clip);
        SceneLoader.Instance.LoadScene("Menu");
    }

    private void Start()
    {
        //Test();
        recentScoreText.text = ScoreSaver.Instance.lastScore.ToString();
        foreach (var score in ScoreSaver.Instance.scores)
        {
            var i = Instantiate(prefab, parent);
            i.nameText.text = score.name;
            i.scoreText.text = score.score.ToString();
            items.Add(i);
        }

        foreach (var item in ScoreSaver.Instance.scores)
        {
            if (item.score < ScoreSaver.Instance.lastScore)
            {
                _canSave = true;
                break;
            }
        }

        if (ScoreSaver.Instance.scores.Count < 5) _canSave = true;

        if (_canSave) inputField.gameObject.SetActive(true);

        if (ScoreSaver.Instance.reason == GameOverReason.Gotong)
        {
            deadreasonText.text = "고통 게이지를 주의깊게 봐주세요";
        }
        else if (ScoreSaver.Instance.reason == GameOverReason.Hp)
        {
            deadreasonText.text = "체력 관리는 중요합니다";
        }
        else if (ScoreSaver.Instance.reason == GameOverReason.Clear)
        {
            deadreasonText.text = "클리어 하셨습니다! 축하합니다!";
        }
        Invoke("PlayAnim", 0.8f);
    }

    private void PlayAnim()
    {
        anim.Play();
    }

    public void Save(string nameValue)
    {
        if (!_canSave) return;

        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
        items.Clear();

        ScoreSaver.Instance.Save(nameValue, ScoreSaver.Instance.lastScore);
        ScoreSaver.Instance.Clean();

        foreach (var score in ScoreSaver.Instance.scores)
        {
            var i = Instantiate(prefab, parent);
            i.nameText.text = score.name;
            i.scoreText.text = score.score.ToString();
            items.Add(i);
        }

        inputField.readOnly = true;
        _canSave = false;
    }

    void Print()
    {
        print("-----------");
        foreach (var i in ScoreSaver.Instance.scores)
        {
            print(i);
        }
    }
}
