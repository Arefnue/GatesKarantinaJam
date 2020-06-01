using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class NavigationManager : MonoBehaviour
{
    #region Singleton
    
    public static NavigationManager manager;
    public int barricadeZone;
    public int outsideArea;

    public List<Transform> enemyMoveTransformList;

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
        
        barricadeZone = 1 << NavMesh.GetAreaFromName("BarricadeZone");
        outsideArea =  1 << NavMesh.GetAreaFromName("OutsideArea");

    }
    

    #endregion

    
    public Vector3 GetRandomPositionOnNavmesh(float radius,Vector3 basePosition,int areaMaskId) {
        var randomDirection = Random.insideUnitSphere * radius;
        randomDirection += basePosition;
        NavMeshHit hit;
        var finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, areaMaskId)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }
}
