using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float SightRadius, AttackRadius;
    [SerializeField] LayerMask PlayerLayer, GroundLayer;
    NavMeshAgent Agent;
    Transform PlayerTransform;
    bool WalkPointSet = false;
    float RandomX;
    float RandomZ;
    bool PlayerInSightRadius, PlayerInAttackRadius;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform BulletSpawnPoint;
    bool Attacking = false;
    [SerializeField] float MinX, MaxX, MinZ, MaxZ; //These variables are used to set the enemy patrol route

    //Gizmos sphere used to detect and attack the player
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, SightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        PlayerTransform = FindFirstObjectByType<CharacterMovement>().transform;
    }


    void Update()
    {
        PlayerInAttackRadius = Physics.CheckSphere(transform.position, AttackRadius, PlayerLayer);
        PlayerInSightRadius = Physics.CheckSphere(transform.position, SightRadius, PlayerLayer);

        if (!PlayerInSightRadius && !PlayerInAttackRadius) { Patrolling(); }
        if (PlayerInSightRadius && !PlayerInAttackRadius) { ChasePlayer(); }
        if (PlayerInSightRadius && PlayerInAttackRadius) { AttackPlayer(); }
    }

    //Navigates the assigned area
    void Patrolling() {
        if (!WalkPointSet) {
            SearchRandomPoint();
        }
        else {
            Agent.SetDestination(new Vector3(RandomX, 0, RandomZ));
        }
        Vector3 distanceToWalkPoint = transform.position - new Vector3(RandomX, 0, RandomZ);
        if (distanceToWalkPoint.magnitude < 1) {
            WalkPointSet = false;
        }
    }

    //Selects a position to travel to on platform
    void SearchRandomPoint() {
        RandomX = Random.Range(MinX, MaxX);
        RandomZ = Random.Range(MinZ, MaxZ);
        if (Physics.Raycast(transform.position, -transform.up, 2.0f, GroundLayer)) {
            WalkPointSet = true;
        }
    }

    //Pursues player when they spotted them
    void ChasePlayer() {
        Agent.SetDestination(PlayerTransform.position);
    }

    //Fires a projectile at the player
    void AttackPlayer() {
        Agent.SetDestination(transform.position);
        transform.LookAt(PlayerTransform.position);
        //Attacking = true;
        if (!Attacking) {
            GameObject BulletSpawned = Instantiate(Bullet, BulletSpawnPoint.position, BulletSpawnPoint.rotation);
            Rigidbody BulletRB = BulletSpawned.GetComponent<Rigidbody>();
            BulletRB.AddForce(transform.forward * 30.0f, ForceMode.Impulse);
            Attacking = true;
            Invoke("ResetAttack", 1.5f); //Takes a second to reset attack before firing again
        }
    }

    void ResetAttack() {
        Attacking = false;
    }
}
