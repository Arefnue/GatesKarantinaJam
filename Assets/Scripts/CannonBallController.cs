using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CannonBallController : MonoBehaviour
{
    public Vector3 moveTo;
    public float cannonSpeed;
    public GameObject explosionObject;
    private bool _isTriggered;
    private Vector3 _moveDirection;
    public GameObject cowPrefab;
    public AudioClip explosionClip;
    
    //Puff particle here
    
    //Cow explosion particle here

    private void Start()
    {
        _moveDirection = Launcher.instance.hitPoint.forward;
    }
    
    private void FixedUpdate()
    {
        //transform.Translate( _moveDirection* (fireballSpeed * Time.deltaTime));
        transform.position = Vector3.MoveTowards(transform.position, moveTo, cannonSpeed * Time.deltaTime);
        transform.Translate(transform.up * (Time.deltaTime * cannonSpeed)/(2f));
        //cowPrefab.transform.position = transform.position;
    }

    public void Explode(Vector3 explosionPosition)
    {
        Instantiate(explosionObject, explosionPosition, Quaternion.identity);
        AudioManager.manager.PlaySfx(explosionClip,70,0.15f);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && !_isTriggered)
        {
            _isTriggered = true;
            Explode(other.ClosestPoint(transform.position));    
        }
        
        if (other.CompareTag("Props") && !_isTriggered)
        {
            _isTriggered = true;
            Explode(other.ClosestPoint(transform.position));    
        }
        
        if (other.CompareTag("PowerUp") && !_isTriggered)
        {
            _isTriggered = true;
            //Explode(other.ClosestPoint(transform.position));
            //Instantiate(explosionObject, other.ClosestPoint(transform.position), Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
    
}
