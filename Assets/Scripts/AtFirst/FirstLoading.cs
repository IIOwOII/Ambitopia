using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLoading : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ReadytoTutorial());
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
    }

    IEnumerator ReadytoTutorial()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<TheFirst>().enabled = true;
        GameObject tutorialtrigger = new GameObject("TutorialTrigger");
        tutorialtrigger.AddComponent<TheTutorial>();
        tutorialtrigger.transform.SetParent(FindObjectOfType<PlayerManager>().gameObject.transform);
        this.enabled = false;
    }
}
