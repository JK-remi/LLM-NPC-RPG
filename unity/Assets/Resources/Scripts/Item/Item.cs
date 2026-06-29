using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int val;
    protected int Value {  get { return val; } }

    [SerializeField]
    private bool isOnce = true;
    protected bool IsOnce { get { return isOnce; } }

    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(IsOnce)
        {
            GetItem(other.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!isOnce)
            GetItem(other.GetComponent<PlayerController>());
    }

    protected virtual void GetItem(PlayerController player) { }

    protected IEnumerator Disappear()
    {
        float time = 1f;
        BoxCollider2D col = this.GetComponent<BoxCollider2D>();
        col.enabled = false;
        if (audio)
        {
            audio.Play();
            time = audio.clip.length;
        }

        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        float elapsedTime = 0f;
        Color c = Color.white;
        while(elapsedTime < time) 
        {
            c.a = Mathf.Lerp(1f, 0f, elapsedTime / time);
            sprite.color = c;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.gameObject.SetActive(false);
    }
}
