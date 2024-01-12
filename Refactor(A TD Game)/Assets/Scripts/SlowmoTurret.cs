using System.Collections;
using UnityEngine;
using UnityEditor;
public class SlowmoTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    
    [Header("Attributes")]
    [SerializeField] private float targetingRange = 4f;
    [SerializeField] private float freezeTime = 1.5f;
    [SerializeField] private float aps = 0.25f; //Attacks Per Second

    private float timeUntilFire;

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if(timeUntilFire >= 1f / aps)
        {
            FreezeEnemies();
            timeUntilFire = 0.0f;
        }
        
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if(hits.Length > 0)
        {
            for(int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyManager em = hit.transform.GetComponent< EnemyManager>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }


    private IEnumerator ResetEnemySpeed(EnemyManager em)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();
    }

    
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
