using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    public Enemy enemyScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageStart()
    {
        print("anim event handler DamageStart");
        enemyScript.DamageStart();
    }

    public void DamageEnd()
    {
        print("anim event handler DamageEnd");
        enemyScript.DamageEnd();
    }

    public void AttackEnd()
    {
        print("anim event handler attack end");
        enemyScript.AttackEnd();
    }
}
