using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    public static DontDestroy Instance { get; private set; }
   
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }
}
