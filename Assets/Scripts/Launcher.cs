using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviour
{
    #region Singleton
    
    public static Launcher instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }

    }
    

    #endregion
    
    public CannonBallController cannonBall;
    public Transform hitPoint;
    public GameObject cannonModel;
    //public Animator _anim;
    public Animator farmerAnimator;

    public GameObject cannonMuzzleParticle;

    public List<AudioClip> cowThrowClipList;
    public AudioClip cannonFireClip;

    private bool _isFiring;
    
    public void Fire(Vector3 moveTo)
    {
        if (FireManager.manager.canFire && !_isFiring)
        {
            hitPoint.LookAt(moveTo);
            
            StartCoroutine(Animation(moveTo));
            
        }
    }
    

    IEnumerator Animation(Vector3 moveTo)
    {
        _isFiring = true;
        var lookPos = new Vector3(moveTo.x,cannonModel.transform.position.y,moveTo.z);
        cannonModel.transform.LookAt(lookPos);
        farmerAnimator.SetInteger("State",1);
        yield return new WaitForSeconds(0.6f);
        FireManager.manager.OnFire();

        var cannonParticle = Instantiate(cannonMuzzleParticle, hitPoint.transform.position, Quaternion.identity);
        
        
        AudioManager.manager.PlaySfx(cannonFireClip,12);
        var randomCowClip = Random.Range(0, cowThrowClipList.Count);
        AudioManager.manager.PlaySfx(cowThrowClipList[randomCowClip],10);
        var obj=Instantiate(cannonBall, hitPoint.position, Quaternion.identity);
        
        
        
        obj.moveTo = moveTo;
        //ScoreManager.manager.Fire();
        
        farmerAnimator.SetInteger("State",0);
        _isFiring = false;
    }
    
}
