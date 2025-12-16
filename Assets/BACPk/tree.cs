using System.Collections;
using UnityEngine;

public class tree : MonoBehaviour, IDamageable
{
    public GameObject snowObject;Coroutine currentShake;
    public ParticleSystem snow;
    bool doOnce = true;

    public void DoShake()
    {
        if (currentShake != null)
            StopCoroutine(currentShake);

        currentShake = StartCoroutine(SmoothShake(0.2f, 0.1f));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SmoothShake(float duration, float strength)
    {
        Vector3 startPos = transform.localPosition;
        float time = 0f;

        while (time < duration)
        {
            float damper = 1f - (time / duration);
            Vector3 offset = Random.insideUnitSphere * strength * damper;
            transform.localPosition = startPos + offset;

            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = startPos;
    }

    public void Damage(int amount)
    {
        snowObject.SetActive(false);
        DoShake();
        if (doOnce)
        {
            doOnce = false;
            Instantiate(snow, transform.position, Quaternion.identity);
        }
    }
}
