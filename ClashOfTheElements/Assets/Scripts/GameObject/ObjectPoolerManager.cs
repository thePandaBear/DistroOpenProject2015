﻿using UnityEngine;
using System.Collections;
using System;

public class ObjectPoolerManager : MonoBehaviour {

    //we'll need pools for arrows and audio objects
    public ObjectPooler ArrowPooler;
    public GameObject ArrowPrefab;


    //basic singleton implementation
    public static ObjectPoolerManager Instance {get;private set;}
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //just instantiate the pools
        if (ArrowPooler == null)
        {
            GameObject go = new GameObject("ArrowPooler");
            ArrowPooler = go.AddComponent<ObjectPooler>();
            ArrowPooler.PooledObject = ArrowPrefab;
            go.transform.parent = this.gameObject.transform;
            ArrowPooler.Initialize();
        }        
    }
}
