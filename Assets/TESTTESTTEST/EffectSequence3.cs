using System.Collections;
using UnityEngine;

public class EffectSequenceSimultaneous : MonoBehaviour
{
    public ParticleSystem effect1;
    public ParticleSystem effect2;
    public ParticleSystem effect3;

    private void Start()
    {
        _ = StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        effect1.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        effect2.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        effect3.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        effect1.Play();

        yield return new WaitUntil(() => !effect1.IsAlive());

        effect2.Play();
        effect3.Play();
    }
}