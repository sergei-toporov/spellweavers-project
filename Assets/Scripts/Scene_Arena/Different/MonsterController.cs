using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    protected SpawnableMonster baseObject;
    public SpawnableMonster BaseObject { get => baseObject; }
    protected float attackDelayTime;
    protected WaitForSeconds attackDelayObject;
    protected OnCharacterEmitterController onCharacterEmitter;

    [SerializeField] protected bool canAttack = false;
    [SerializeField] protected bool isAttacking = false;
    public bool inAttackRange = false;

    protected void Start()
    {
        baseObject = GetComponent<SpawnableMonster>();
        onCharacterEmitter = GetComponentInChildren<OnCharacterEmitterController>();
        SetAttackDelayParameters();
    }

    protected void Update()
    {
        //if (!isAttacking && Vector3.Distance(transform.position, ArenaManager.Manager.Player.transform.position) <= baseObject.CharStats.attackRange)
        if (!isAttacking && inAttackRange)
        {
            Attack();
        }
    }


    protected void SetAttackDelayParameters()
    {
        attackDelayTime = 60.0f / baseObject.CharStats.attacksPerMinute;
        canAttack = true;
        attackDelayObject = new WaitForSeconds(attackDelayTime);
    }

    public void Attack()
    {
        isAttacking = true;
        if (baseObject.HitterPrefab != null)
        {
            WeaponHitter strike = Instantiate(baseObject.HitterPrefab, onCharacterEmitter.transform.position, onCharacterEmitter.transform.rotation);
            strike.SetParent(baseObject);
            if (strike.TryGetComponent(out Rigidbody strikeRb))
            {
                strikeRb.AddForce(onCharacterEmitter.transform.forward, ForceMode.Impulse);
            }
        }
        StartCoroutine(AttackDelay());
    }

    protected IEnumerator AttackDelay()
    {
        yield return attackDelayObject;
        isAttacking = false;
    }
}
