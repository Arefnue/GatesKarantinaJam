using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : MonoBehaviour
{
    ReloadSingle singleton;

    public float speed;
    public float phase2_multiplier;
    public float small_multiplier;
    
    public int place;
    public float behindCannonDistance;
    public float cowDistance;

    bool phase2 = false;
    GameObject cannon;
    GameObject cowPrefab;
    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;
    Vector3 cowSpawn;

    private void Start()
    {
        singleton = ReloadSingle.single;
        cowPrefab = singleton.cowPref;
        
        cannon = GameObject.FindGameObjectWithTag("Cannon");
        pos1 = cannon.transform.position - new Vector3(0,0,behindCannonDistance);
        pos2 = cannon.transform.position;
        pos3 = transform.position + new Vector3(cowDistance, 0, 0);
        if (place == 1)
            cowSpawn = transform.position - new Vector3(cowDistance, 0, 0);
        else
            cowSpawn = transform.position;
    }
    private void Update()
    {
        if (place == 1 && singleton.enter) { 
            if (!phase2)
            {
                if (transform.position != pos1)
                    transform.position = Vector3.MoveTowards(transform.position, pos1, speed * Time.deltaTime);
                else
                    phase2 = true;
            }
            else
            {
                if (transform.localScale != Vector3.zero)
                {
                    transform.position = Vector3.MoveTowards(transform.position, pos2, phase2_multiplier * speed * Time.deltaTime);
                    transform.localScale = new Vector3(transform.localScale.x - small_multiplier, transform.localScale.y - small_multiplier, transform.localScale.z - small_multiplier);
                }
                else
                {
                    singleton.enter = false;
                    Destroy(gameObject);
                    Instantiate(cowPrefab, cowSpawn, Quaternion.identity);
                    singleton.decreasePlace = true;
                }
            }
        }
        else if (singleton.enter)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos3, speed *Time.deltaTime);
            if (transform.position == pos3 && singleton.decreasePlace)
            {
                place--;
                singleton.decreasePlace = false;
            }   
        }

    }
}
