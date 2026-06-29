using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hit : MonoBehaviour
{
    public float time = 1f;
    public float radius = 10f;
    private AudioSource audio;
    private Image img;
    public TextMeshProUGUI txt;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        img = GetComponent<Image>();
        if (audio != null) time = audio.clip.length;
        this.gameObject.SetActive(false);
    }

    public void Play(Transform t, int dmg)
    {
        this.gameObject.SetActive(true);
        this.transform.position = t.position;
        txt.text = string.Format("{0}", dmg);
        StartCoroutine(ProcessDestroy());
    }

    IEnumerator ProcessDestroy()
    {
        Vector3 pos = this.transform.position;
        pos.x += Random.Range(-radius, radius);
        pos.y += Random.Range(-radius, radius);
        this.transform.position = pos;

        if(audio) audio.Play();

        Color c = Color.white;
        float elapsedTime = 0f;
        while (elapsedTime < time) 
        {
            c.a = Mathf.Lerp(1f, 0f, elapsedTime / time);
            img.color = c;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.gameObject.SetActive(false);
    }
}
