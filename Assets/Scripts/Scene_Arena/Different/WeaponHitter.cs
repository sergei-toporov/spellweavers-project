using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitter : MonoBehaviour
{
    // Start is called before the first frame update
    protected Vector3 startPosition;

    protected float ttl = 5.0f;
    protected WaitForSeconds ttlObject;
    protected SpawnableBase parent;
    protected GenericDictionary<SpawnableBase, string> targets;
    void Start()
    {
        startPosition = transform.position;
        transform.Rotate(transform.forward);
        StartCoroutine(LifeExpirationCoroutine());
        ttlObject = new WaitForSeconds(ttl);
    }

    public void SetParent(SpawnableBase providedObject)
    {
        parent = providedObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (parent != null && collision.gameObject.TryGetComponent(out SpawnableBase target)) {
            targets = new GenericDictionary<SpawnableBase, string>();
            targets.Add(target, "");
            if (parent.CharStats.damageRadius > 0.0f)
            {
                foreach (Collider hitObject in Physics.OverlapSphere(transform.position, parent.CharStats.damageRadius))
                {                   
                    if (hitObject.TryGetComponent(out SpawnableBase hitObjectSB) && hitObjectSB != parent) {
                        targets.TryAdd(hitObjectSB, "");
                    }
                }
            }

            foreach (SpawnableBase hitObject in targets.Keys)
            {
                hitObject.TakeDamage(parent);
            }
        }


        DestroyObject();
    }

    protected IEnumerator LifeExpirationCoroutine()
    {
        yield return new WaitForSeconds(ttl);
        DestroyObject();
    }

    protected void FixedUpdate()
    {
        if (parent != null)
        {
            if (Vector3.Distance(startPosition, transform.position) >= parent.CharStats.attackRange)
            {
                DestroyObject();
            }
        }
    }

    protected void DestroyObject()
    {
        Destroy(gameObject);
    }
}
