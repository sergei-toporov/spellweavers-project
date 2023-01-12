using Spellweavers;
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

    [SerializeField] protected string onCreateVFX;
    [SerializeField] protected string lifetimeVFX;
    [SerializeField] protected string onDestroyVFX;
    void Start()
    {
        startPosition = transform.position;
        transform.Rotate(transform.forward);
        ParticleSystem lifeVFX = VFXManager.Manager.ParticleVFXCollection.GetItemByKey(lifetimeVFX).prefab;        

        if (lifeVFX != null)
        {
            lifeVFX = Instantiate(lifeVFX, transform.position, transform.rotation);
            lifeVFX.transform.Rotate(new Vector3(180.0f, 0.0f, 0.0f));
            lifeVFX.transform.SetParent(gameObject.transform);
            lifeVFX.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            lifeVFX.Play();
        }        

        StartCoroutine(LifeExpirationCoroutine());
        ttlObject = new WaitForSeconds(ttl);
    }

    public void SetParent(SpawnableBase providedObject)
    {
        parent = providedObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ParticleSystem vfx = VFXManager.Manager.ParticleVFXCollection.GetItemByKey(onDestroyVFX).prefab;
        if (vfx != null)
        {
            vfx = Instantiate(vfx, transform.position, transform.rotation);
            var vfxMain = vfx.main;
            vfxMain.startSizeX = parent.CharStats.damageRadius;
            vfxMain.startSizeY = parent.CharStats.damageRadius / 2.0f;

            vfx.Play();
        }

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
