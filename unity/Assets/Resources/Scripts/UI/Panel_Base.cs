using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Base : MonoBehaviour
{
    public AudioClip clip;


    public virtual void Init() 
    {
        this.gameObject.SetActive(true);
        if(clip != null)
            GameManager.Instance.PlayAudio(clip);
    }

    public virtual void Close()
    {
        this.gameObject.SetActive(false);
    }
}
