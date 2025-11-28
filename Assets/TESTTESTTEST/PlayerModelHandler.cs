using System;
using UnityEngine;

public class PlayerModelHandler : MonoBehaviour
{
    public GameObject model;
    public Animator animator;

    internal Animator GetAnimator()
    {
        print("get animator " + animator);
        return animator;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
