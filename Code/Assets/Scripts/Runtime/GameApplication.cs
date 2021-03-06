﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;

public class GameApplication : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(this);

        Init();
    }

    void Init()
    {
    }
    
    void Start()
    {
        AppFacade.Instance.StartUp();
    }
    
    void Update()
    {
        AppFacade.Instance.OnUpdate();
    }

    void LateUpdate()
    {
        AppFacade.Instance.OnLateUpdate();
    }

    void FixedUpdate()
    {
        AppFacade.Instance.OnFixedUpdate();
    }

    void OnDestroy()
    {
        AppFacade.Instance.Close();
    }
}
