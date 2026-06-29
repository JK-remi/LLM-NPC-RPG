using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Opening : Panel_Base
{
    private bool isStarted = false;

    public float wait_time = 1f;
    public List<GameObject> hideObjs = new List<GameObject>();
    public GameObject textStart;

    protected void Start()
    {
        textStart.SetActive(false);
    }

    protected void Update()
    {
        if(!isStarted && Input.anyKeyDown)
        {
            isStarted = true;

            for(int i=0; i<hideObjs.Count; i++)
                hideObjs[i].SetActive(false);
            textStart.SetActive(true);
        }

        if(isStarted)
        {
            wait_time -= Time.deltaTime;
            if(wait_time < 0f)
            {
                GameManager.Instance.CloseOpening();
            }
        }
    }
}
