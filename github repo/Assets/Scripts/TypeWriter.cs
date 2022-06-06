using System.Collections;
using System.Collections.Generic;
using UnityEngine;using TMPro;

public class TypeWriter : MonoBehaviour
{
    // Start is called before the first frame update
    public void Run(string textToType,TMP_Text textLabel )
    {
        StartCoroutine(TypeText(textToType, textLabel));
    }

    // Update is called once per frame
    private IEnumerator TypeText(string textToType, TMP_Text textLabel){
    float t = 0;
    int charIndex = 0;
    
    while(charIndex < textToType.Length) {
                         t += Time.deltaTime;
                         charIndex= Mathf.FloorToInt(t);
        charIndex = Mathf.Clamp(charIndex,0,textToType.Length);
        textLabel.text = textToType.Substring(0,charIndex);
        yield return null;}

textLabel.text = textToType;

    }
}
