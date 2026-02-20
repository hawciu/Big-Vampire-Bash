using System;
using Unity.VisualScripting;
using UnityEngine;
public enum LootLevel
{
    WHITE,
    GREEN,
    BLUE,
    PURPLE,
    ORANGE
}


public class LootParticleController : MonoBehaviour
{
    public LootLevel rarity;
    public ParticleSystem orbMain, orbPulse, pulseHorizontal, ringHorizontal, lineVertical, pulseLineVertical, sparks;

    public Color32 p_white = new Color32(255, 255, 255, 255),
        p_green = new Color32(120, 245, 108, 255),
        p_blue = new Color32(245, 108, 255, 255),
        p_purple = new Color32(245, 108, 255, 255),
        p_orange = new Color32(245, 108, 255, 255);

    Color colorToSet;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //test only
        ParticleSetup();
    }

    void SetParticleRarity(LootLevel targetRarity)
    {
        rarity = targetRarity;
        ParticleSetup();
    }

    void ParticleSetup()
    {
        switch (rarity)
        {
            case LootLevel.WHITE:
                colorToSet = p_white;
                SetPulseHorizontalSize(2f);
                break;

            case LootLevel.GREEN:
                colorToSet = p_green;
                SetPulseHorizontalSize(3f);
                break;

            case LootLevel.BLUE:
                colorToSet = p_blue;
                SetOrbMainSize(1.2f);
                SetOrbPulseSize(3f);
                SetOrbPulseEmission(0.6f);
                SetPulseHorizontalSize(4f);
                SetRingHorizontalEmission(0.6f);
                SetLineVerticalScale(new Vector3(0.3f, 1.1f, 0.3f));
                break;

            case LootLevel.PURPLE:
                colorToSet = p_purple;
                SetOrbMainSize(1.5f);
                SetOrbPulseSize(3.5f);
                SetOrbPulseEmission(1f);
                SetPulseHorizontalSize(5f);
                SetRingHorizontalEmission(1f);
                SetLineVerticalScale(new Vector3(0.4f, 1.3f, 0.4f));
                SetPulseLineVerticalScale(new Vector3(0.8f, 1.5f, 0.8f));
                SetPulseLineVerticalEmission(1.3f);
                SetSparksEmission(3);
                SetSparksLifetime(0.7f);
                SetSparksStartSize(0.15f);
                break;

            case LootLevel.ORANGE:
                colorToSet = p_orange;
                SetOrbMainSize(2f);
                SetOrbPulseSize(4f);
                SetOrbPulseEmission(1.4f);
                SetPulseHorizontalSize(6f);
                SetRingHorizontalEmission(1.4f);
                SetLineVerticalScale(new Vector3(0.5f, 1.5f, 0.5f));
                SetPulseLineVerticalScale(new Vector3(0.8f, 2f, 0.8f));
                SetPulseLineVerticalEmission(1.9f);
                SetSparksEmission(5);
                SetSparksLifetime(1);
                SetSparksStartSize(0.2f);
                break;
        }

        ActivateEmitters();
        SetParticleEmittersColor();
    }


    void ActivateEmitters()
    {
        switch (rarity)
        {
            case LootLevel.WHITE:
                orbMain.Play();
                //orbPulse.Play();
                pulseHorizontal.Play();
                //ringHorizontal.Play();
                //lineVertical.Play();
                break;

            case LootLevel.GREEN:
                orbMain.Play();
                //orbPulse.Play();
                pulseHorizontal.Play();
                //ringHorizontal.Play();
                lineVertical.Play();
                break;

            case LootLevel.BLUE:
                orbMain.Play();
                orbPulse.Play();
                pulseHorizontal.Play();
                ringHorizontal.Play();
                lineVertical.Play();
                pulseLineVertical.Play();
                break;

            case LootLevel.PURPLE:
                orbMain.Play();
                orbPulse.Play();
                pulseHorizontal.Play();
                ringHorizontal.Play();
                lineVertical.Play();
                pulseLineVertical.Play();
                sparks.Play();
                break;

            case LootLevel.ORANGE:
                orbMain.Play();
                orbPulse.Play();
                pulseHorizontal.Play();
                ringHorizontal.Play();
                lineVertical.Play();
                pulseLineVertical.Play();
                sparks.Play();
                break;
        }
    }

    private void SetParticleEmittersColor()
    {
        ParticleSystem.MainModule psmain;
        foreach (Transform child in transform)
        {
            psmain = child.gameObject.GetComponent<ParticleSystem>().main;
            psmain.startColor = colorToSet;
        }
    }

    void SetOrbMainSize(float size)
    {
        ParticleSystem.MainModule psmain;
        psmain = orbMain.main;
        psmain.startSize = size;
    }

    void SetOrbPulseSize(float size)
    {
        ParticleSystem.MainModule psmain;
        psmain = orbPulse.main;
        psmain.startSize = size;
    }

    void SetOrbPulseEmission(float targetEmission)
    {
        ParticleSystem.EmissionModule psemission = orbPulse.emission;
        psemission.rateOverTime = targetEmission;
    }

    void SetPulseHorizontalSize(float size)
    {
        ParticleSystem.MainModule psmain;
        psmain = pulseHorizontal.main;
        psmain.startSize = size;
    }

    void SetRingHorizontalEmission(float targetEmission)
    {
        ParticleSystem.EmissionModule psemission = ringHorizontal.emission;
        psemission.rateOverTime = targetEmission;
    }

    void SetLineVerticalScale(Vector3 targetScale)
    {
        lineVertical.transform.localScale = targetScale;
    }

    void SetPulseLineVerticalScale(Vector3 targetScale)
    {
        pulseLineVertical.transform.localScale = targetScale;
    }

    void SetPulseLineVerticalEmission(float targetEmission)
    {
        ParticleSystem.EmissionModule psemission = pulseLineVertical.emission;
        psemission.rateOverTime = targetEmission;
    }

    void SetSparksEmission(float targetEmission)
    {
        ParticleSystem.EmissionModule psemission = sparks.emission;
        psemission.rateOverTime= targetEmission;
    }

    void SetSparksLifetime(float targetLifetime)
    {
        ParticleSystem.MainModule psmain;
        psmain = sparks.main;
        psmain.startLifetime = targetLifetime;
    }

    void SetSparksStartSize(float targetSize)
    {
        ParticleSystem.MainModule psmain;
        psmain = sparks.main;
        psmain.startSize = targetSize;
    }
}
