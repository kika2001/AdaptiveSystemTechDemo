using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
   [SerializeField] private UnityEvent onClickEvent;
   [SerializeField] private UnityEvent onEnableEvent;
   //Clicking Effect
   [Header("Clicking Effect")]
   public float clickTimeAnimation =0.5f;
   public float clickTravelDistance =2f;

   public void PlayEvent()
   {
      if (!UI3DCanvas.clickingSomething)
      {
         StartCoroutine(ClickAnimation());
      }
   }

   private IEnumerator ClickAnimation()
   {
      UI3DCanvas.clickingSomething = true;
      Vector3 initialPos = transform.position;
      float currentTime=0;
      Vector3 targetPos = transform.position +transform.forward * clickTravelDistance;
      do
      {
         yield return new WaitForEndOfFrame();
         currentTime += Time.deltaTime;
         if (currentTime/clickTimeAnimation<0.5)
         {
            transform.position = Vector3.Lerp(initialPos,targetPos,currentTime/clickTimeAnimation);
         }
         else
         {
            transform.position = Vector3.Lerp(initialPos,targetPos,1-(currentTime/clickTimeAnimation));
         }
         
      } while (currentTime<clickTimeAnimation);
      yield return new WaitForSeconds(0.2f);
      UI3DCanvas.clickingSomething = false;
      onClickEvent.Invoke();
   }

   
}
