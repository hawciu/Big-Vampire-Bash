using System;
using UnityEngine;

public class EnemyModelHandler : MonoBehaviour
{
    public GameObject hatTargetObject;
    public GameObject modelMain;
    public GameObject body;
    public GameObject outline;

    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = modelMain.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Material GetMaterial()
    {
        return body.GetComponent<Renderer>().material;
    }

    public void SetMaterial(Material material)
    {
        body.GetComponent<Renderer>().material = material;
    }

    public void SetDefaultMaterial()
    {
        body.GetComponent<Renderer>().material = EnemyManager.instance.GetCurrentDefaultEnemyMaterial();
    }

    public void SetMaterialOutline(Material material)
    {
        outline.GetComponent<Renderer>().material = material;
    }

    public void PauseAnimator(bool pause)
    {
        animator.speed = pause ? 0 : 1;
    }
}
