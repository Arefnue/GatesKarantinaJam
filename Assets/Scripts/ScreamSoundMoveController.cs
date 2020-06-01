using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamSoundMoveController : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float speed;

    private bool _isReached;
    

    private void Update()
    {
        if (_isReached)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos.position, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position,startPos.position) <= 0.1f)
            {
                _isReached = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos.position, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position,endPos.position) <= 0.1f)
            {
                _isReached = true;
            }
        }
    }
}
