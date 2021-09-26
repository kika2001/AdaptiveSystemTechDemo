using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject startButtons;
    public GameObject optionsButtons;
    public GameObject optionsSubMenuParent;
    
    [Header("AppearEffect")] 
    public float appearTimeAnimation = 0.5f;
    public float appearTravelDistance = 2f;

    private void Awake()
    {
        CloseOptions();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Dungeon");
    }

    public void OpenOptions()
    {
        StartCoroutine(AppearAnimation(optionsButtons,startButtons));
    }

    public void OpenOptionsSubMenu(GameObject subMenu)
    {
       var child= optionsSubMenuParent.transform.Find(subMenu.name);
       if (child ==null) 
           return;
       for (int i = 0; i < optionsSubMenuParent.transform.childCount; i++)
       {
           optionsSubMenuParent.transform.GetChild(i).gameObject.SetActive(false);
       }
       child.gameObject.SetActive(true);
       //Maybe Play some animation
    }
    public void CloseOptions()
    {
        StartCoroutine(AppearAnimation(startButtons,optionsButtons));
    }
    public void CloseGame()
    {
        Application.Quit();
    }

    private IEnumerator AppearAnimation(GameObject appearGO,GameObject disappearGO)
    {
        UI3DCanvas.clickingSomething = true;
        Vector3 initialPos;
        Vector3 targetPos;
        float currentTime = 0;
        if (disappearGO.activeSelf)    
        {
            //Disappear
            initialPos = disappearGO.transform.position ;
            targetPos = disappearGO.transform.position +disappearGO.transform.forward * appearTravelDistance;
            do
            {
                yield return new WaitForEndOfFrame();
                currentTime += Time.deltaTime;
                disappearGO.transform.position = Vector3.Lerp(initialPos,targetPos,currentTime/appearTimeAnimation);
         
            } while (currentTime<appearTimeAnimation);

            disappearGO.transform.position = initialPos;
            disappearGO.SetActive(false);
        }

        appearGO.SetActive(true);
        //Appear
        initialPos = appearGO.transform.position + appearGO.transform.forward * appearTravelDistance;
        currentTime=0;
        targetPos = appearGO.transform.position;
        do
        {
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
            appearGO.transform.position = Vector3.Lerp(initialPos,targetPos,currentTime/appearTimeAnimation);
         
        } while (currentTime<appearTimeAnimation);
        yield return new WaitForSeconds(0.2f);
        UI3DCanvas.clickingSomething = false;
    }
}
