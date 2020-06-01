using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    #region Singleton
    
    public static FireManager manager;
    
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            
        }
        else
        {
            Destroy(gameObject);
        }

    }
    

    #endregion
    
    
    public bool canFire { get; private set; }
    public GameObject cowHead;
    public Transform spawnPosition;
    public Transform waitPosition;
    public GameObject cannonObject;
    public float fireRate;
    public GameObject cowModel;
    public float cannonRadius;
    public Animator cowAnimator;
    public GameObject puffParticle;
    
    private void Start()
    {
        canFire = true;
    }


    private void Update()
    {
        if (canFire)
        {
            cowAnimator.SetInteger("State",1);
            cowModel.transform.LookAt(waitPosition.position);
            cowModel.transform.position = Vector3.MoveTowards(cowModel.transform.position, waitPosition.position, Time.deltaTime * fireRate);
            if (Vector3.Distance(cowModel.transform.position,waitPosition.position)<= 0.1f)
            {
                cowAnimator.SetInteger("State",0);
            }
        }
        else
        {
            cowAnimator.SetInteger("State",1);
            cowModel.transform.LookAt(cannonObject.transform.position);
            cowModel.transform.position = Vector3.MoveTowards(cowModel.transform.position, cannonObject.transform.position, Time.deltaTime * fireRate);
            if (Vector3.Distance(cowModel.transform.position,cannonObject.transform.position)<= cannonRadius)
            {
                
                cowModel.SetActive(false);
                Reload();
                cowModel.transform.position = spawnPosition.position;
                cowModel.SetActive(true);
            }
        }
    }


    public void OnFire()
    {
        
        cowHead.SetActive(false);
        canFire = false;
        //StartCoroutine(Delay());
    }

    public void Reload()
    {
        
        Instantiate(puffParticle, cowHead.transform.position, Quaternion.identity);
        Instantiate(puffParticle, cowModel.transform.position, Quaternion.identity);
        
        cowHead.SetActive(true);
        canFire = true;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(fireRate);
        Reload();
    }
    


}
