using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
   public GameObject fireObject;
   public LayerMask groundMask;
   private void Update()
   {
      TouchControl();
      MouseControl();
   }

   public void MouseControl()
   {
      if (Input.GetMouseButtonDown(0)) {
         var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit hit;
         
         if(Physics.Raycast(ray, out hit,999f,groundMask))
         {
            if (hit.collider != null) {
                 
               var touchedObject = hit.transform.gameObject;
               //Instantiate(fireObject, hit.point, Quaternion.identity);
               Launcher.instance.Fire(hit.point);
            }
         }
      }
   }


   public void TouchControl()
   {
      if (Input.touchCount == 1 && Input.GetTouch(0).phase ==TouchPhase.Began) {
         var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
         RaycastHit hit;
         
         if(Physics.Raycast(ray, out hit,999f,groundMask))
         {
            if (hit.collider != null) {
                 
               var touchedObject = hit.transform.gameObject;
               //Instantiate(fireObject, hit.point, Quaternion.identity);
               Launcher.instance.Fire(hit.point);
            }
         }
      }
   }
   
}
