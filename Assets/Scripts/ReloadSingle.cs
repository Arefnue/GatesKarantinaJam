using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSingle : MonoBehaviour
{
    public static ReloadSingle single;

    public bool enter, decreasePlace;
    public GameObject cowPref;
    private void Awake()
    {
        single = this;
    }
}
