using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameOverReason
{
    Gotong,
    Hp,
    Clear
}

public class ScoreSaver : MonoBehaviour
{
    private static ScoreSaver instance;

    public static ScoreSaver Instance
    {
        get
        {
            if (instance != null) return instance;
            var i = FindObjectOfType<ScoreSaver>();
            if (i != null)
            {
                instance = i;
                return instance;
            }
            i = Instantiate(Resources.Load<ScoreSaver>("ScoreSaver"));
            instance = i;
            return instance;
        }
        set
        {
            instance = value;
        }
    }



    public List<(string name, int score)> scores = new List<(string name, int score)>();
    public int lastScore;
    public GameOverReason reason;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance == this) return;
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Save(string name, int score)
    {
        scores.Add((name, score));
    }

    public void Clean()
    {
        scores.Sort((value, tupleValue) => tupleValue.score.CompareTo(value.score));

        var newList = new List<(string name, int score)>();
        newList.AddRange(scores);

        scores.Clear();
        for (var i = 0; i < 5; i++)
        {
            if (i >= newList.Count) break;
            scores.Add(newList[i]);
        }
    }
}
