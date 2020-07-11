using System.Collections.Specialized;
using System.IO.IsolatedStorage;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float moveSpeed;

    public Material healthyMaterial;
    public Material infectedMaterial;
    public GameObject explodePrefab;
    public GameObject infectPrefab;

    public float infectedLifeTime;

    public bool isInfected { get; private set; }

    MeshRenderer m_MeshRenderer;
    Rigidbody    m_RigidBody;

    NPCManager m_NPCManager;

    float m_TimeTillDeathCountdown;

    // Start is called before the first frame update
    void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_RigidBody    = GetComponent<Rigidbody>();

        m_NPCManager = FindObjectOfType<NPCManager>();

        m_NPCManager.RegisterNPC(this);

        m_MeshRenderer.material = healthyMaterial;

        Vector2 moveDir   = Random.insideUnitCircle.normalized;
        Vector3 moveForce = new Vector3(moveDir.x, moveDir.y, 0f) * moveSpeed;
        m_RigidBody.AddForce(moveForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDir = Vector3.Normalize(m_RigidBody.velocity);
        m_RigidBody.velocity = moveDir * moveSpeed;

        if (isInfected)
        {
            m_TimeTillDeathCountdown -= Time.deltaTime;

            if (m_TimeTillDeathCountdown < 0f)
            {
                OnDie();
            }
        }
    }

    public void OnInfected()
    {
        isInfected = true;
        Instantiate(infectPrefab, this.transform.position, this.transform.rotation);
        m_TimeTillDeathCountdown = infectedLifeTime;
        m_MeshRenderer.material  = infectedMaterial;

        switch (m_NPCManager.currentMutation)
        {
            case InfectedMutations.biggerSize:
                transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                break;

            case InfectedMutations.moveFaster:
                moveSpeed += 2.5f;
                break;

            case InfectedMutations.surviveLonger:
                m_TimeTillDeathCountdown += 6f;
                break;
        }
    }

    void OnDie()
    {
        m_NPCManager.UnregisterNPC(this);
        Instantiate(explodePrefab, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isInfected)
            return;

        NPC hitNPC;
        if (collision.gameObject.TryGetComponent<NPC>(out hitNPC))
        {
            if (hitNPC.isInfected)
                OnInfected();
        }
    }
}