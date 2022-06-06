using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeImage : MonoBehaviour
{
    public GameObject[] background;
    int index;
    public GameObject nextButton;
    public GameObject startGameButton;


    private void Start()
    {
        startGameButton.SetActive(false);
    }

    private void Update()
    {
        if (index >= 4)
            index = 4;

        if (index < 0)
            index = 0;

        if(index == 0)
        {
            background[0].gameObject.SetActive(true);
        }

        if (index == 4)
        {
            nextButton.SetActive(false);
            startGameButton.SetActive(true);
        }
    }

    public void Next()
    {
        index += 1;

        for(int i=0; i < background.Length; i++)
        {

            background[i].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);

        }

        Debug.Log(index);
    }

    
}
