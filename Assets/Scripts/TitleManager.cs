using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    private float time;
    public Image bgImage;
    public AudioSource source;
    public AudioClip clip;

    private void Update()
    {
        time += Time.deltaTime;
        var color = bgImage.color;
        color.a = 0.4f + Mathf.Cos(time) / 10;
        bgImage.color = color;
    }

    public void Play()
    {
        source.PlayOneShot(clip);
        SceneLoader.Instance.LoadScene("GamePlay");
    }

    public void Quit()
    {
        source.PlayOneShot(clip);
        Application.Quit();
    }
}
