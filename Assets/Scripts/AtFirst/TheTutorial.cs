using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheTutorial : MonoBehaviour
{
    PlayerManager theplayer;
    EventManager theevent;
    EffectManager theeffect;
    DialogueSystem thedialogue;
    FadeScreenSetting thefade;
    WaitForSeconds interval;
	Forgetmenot FMN;
	
    private bool isevent1 = false;
    private bool isevent2 = false;

    void Start()
    {
        theplayer = FindObjectOfType<PlayerManager>();
        theevent = FindObjectOfType<EventManager>();
        theeffect = FindObjectOfType<EffectManager>();
        thedialogue = FindObjectOfType<DialogueSystem>();
        thefade = FindObjectOfType<FadeScreenSetting>();
		FMN = FindObjectOfType<Forgetmenot>();
        SceneManager.sceneLoaded += InvestigateBoyHouse;
        interval = new WaitForSeconds(1f);
    }

    private void InvestigateBoyHouse(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 3) //Boy의 집안
        {
            GameObject.Find("Boyhousedoor-1").layer = 8;
            if (!isevent1)
                StartCoroutine(Event1());
        }
        if (scene.buildIndex == 4) //서재
        {
            if (!isevent2)
                StartCoroutine(Event2());
        }
    }

    IEnumerator Event1()
    {
        theplayer.keygetpossible = false;
        yield return interval;
        theevent.EventShowOn();
        theevent.StartCoroutine(theevent.PlayerMove("D"));
        yield return interval;
        yield return interval;
        theplayer.keygetpossible = true;
        theevent.EventShowOff();
        thedialogue.Action(null);
        isevent1 = true;
    }

    IEnumerator Event2()
    {
        theplayer.keygetpossible = false;
        theevent.StartCoroutine(theevent.PlayerMove("UUUUUR"));
        yield return interval;
        yield return interval;
        yield return interval;
        yield return interval;
        yield return interval;
        theplayer.keygetpossible = true;
        thedialogue.NPCValueSet("0:0:0:1");
        thedialogue.Action(null);
        isevent2 = true;
    }

    public IEnumerator EndTutorial()
    {
        theeffect.LoadingSignDisplay(true);
        yield return new WaitForSeconds(0.5f);
		FMN.IsfirstSet(false);
        yield return interval;
        theeffect.LoadingSignDisplay(false);
        theplayer.ontitle = true;
        theplayer.keygetpossible = false;
        GameObject.Find("Boyhousedoor-1").GetComponent<Animator>().SetBool("Open", true);
        SoundManager.instance.SoundPlay(2);
        BGMManager.instance.StartCoroutine(BGMManager.instance.BGMFadeOut());
        yield return interval;
        yield return StartCoroutine(thefade.ScreenFadeInAtTime(0.005f));
        yield return interval;
        yield return interval;
        yield return interval;
        theplayer.towardpoint = 5;
        SceneManager.sceneLoaded -= InvestigateBoyHouse;
        SceneManager.LoadScene("BW_boyhouseyard");
        Destroy(this.gameObject);
    }
}
