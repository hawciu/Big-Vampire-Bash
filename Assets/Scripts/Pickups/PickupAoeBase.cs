using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

enum State
{
    INACTIVE,
    ACTIVATED,
    PREPARING,
    DAMAGING,
    DONE
}

public class PickupAoeBase : MonoBehaviour, IPickupEffect
{
    public GameObject visualIndicator;
    public ParticleSystem spikesSmall;
    public ParticleSystem spikesSmallDust;

    bool activated = false;
    float startTime;
    State state = State.INACTIVE;

    List<GameObject> hitEnemies = new();
    int damageIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePickup();
    }

    void UpdatePickup()
    {
        switch(state)
        {
            case State.INACTIVE:
                break;

            case State.ACTIVATED:
                startTime = Time.time;
                spikesSmall.Play();
                spikesSmallDust.Play();
                state = State.PREPARING;
                break;

            case State.PREPARING:
                if (startTime + 2f < Time.time)
                {
                    spikesSmall.Stop();
                    spikesSmallDust.Stop();
                    float range = 10;
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("ENEMY"));
                    foreach (Collider hitCollider in hitColliders)
                    {
                        hitCollider.gameObject.GetComponent<EnemySimple>().Damage(3);
                        EffectsManager.instance.SpawnAnEffect(ParticleType.GROUND_SPIKE, hitCollider.gameObject.transform.position);
                    }
                    state = State.DONE;
                }
                break;

            case State.DAMAGING:
                foreach(GameObject i in hitEnemies)
                {
                    hitEnemies[damageIndex].GetComponent<EnemySimple>().Damage(3);
                }
                state = State.DONE;
                break;

            case State.DONE:
                Destroy(transform.parent.gameObject);
                break;

        }
    }

    public void Activate()
    {
        if (activated) return;
        activated = true;
        state = State.ACTIVATED;
        state = State.PREPARING;
    }
}
