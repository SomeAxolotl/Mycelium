using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuInput : MonoBehaviour
{
    [SerializeField] private GameObject _Next;
    [SerializeField] private GameObject _Previous;
    public static MenuInput instance;
    private PlayerInput _playerInput;
    public void Awake()
    {
        if (instance==null)
        {
            instance=this;
        }
    }
    
}

