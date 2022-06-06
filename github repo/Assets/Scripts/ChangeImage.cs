using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeImage : MonoBehaviour
{
    public GameObject[] background;
    public GameObject[] texts;
    int index;

    private void Update()
    {
        if (index >= 4)
            index = 4;

        if (index < 0)
            index = 0;

        if(index == 0)
        {
            background[0].gameObject.SetActive(true);
            texts[0].gameObject.SetActive(true);
        }
    }

    public void Next()
    {
        index += 1;

        for(int i=0; i < background.Length; i++)
        {

            background[i].gameObject.SetActive(false);
            texts[i].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
            texts[index].gameObject.SetActive(true);

        }

        Debug.Log(index);
    }

    
}
