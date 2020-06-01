using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public int healValue;
    public GameObject destroyParticle;
    public GameObject collisionParticle;
    private bool _isTriggered;
    public float lifeTime;
    public Transform fxTransform;
    public GameObject model;
    private bool _isDead;
    private void Start()
    {
        Invoke(nameof(Death),lifeTime);
    }


    private void Death()
    {
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        _isDead = true;
        model.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CannonBall") && !_isTriggered && !_isDead)
        {
            _isTriggered = true;
            ScoreManager.manager.Heal(healValue);
            ScoreManager.manager.AddHolyCowScore(healValue);
            Instantiate(collisionParticle, fxTransform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (other.CompareTag("CannonBall") && !_isTriggered && _isDead)
        {
            _isTriggered = true;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
