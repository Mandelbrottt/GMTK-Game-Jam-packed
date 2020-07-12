using System.Collections.Specialized;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPC : MonoBehaviour
{
    public float moveSpeed;

    public GameObject NPCPrefab;

    public Material healthyMaterial;
    public Material infectedMaterial;
    public Material infectedPuddleMaterial;
    public GameObject explodePrefab;
    public GameObject infectPrefab;

    public float infectedLifeTime;

    public bool isAlive;

    public bool isSplitChild = false;

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

		m_MeshRenderer.material = healthyMaterial;

		var mt = FindObjectOfType<MutationText>();
		mt.onLevelStart.AddListener(OnLevelStart);
		
		FindObjectOfType<LevelManager>().onPostLevelLoadEvent.AddListener(OnLevelLoad);
        
        isAlive = true;
	}

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
		{
			// Create the particle for infecting an npc then destroy it after it's dead
			var transform1 = transform;
			var infect     = Instantiate(infectPrefab, transform1.position, transform1.rotation);
			var ps         = infect.GetComponent<ParticleSystem>();
			Destroy(infect, ps.main.duration);
		}
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
        if (!isAlive)
            return;

        isAlive = false;

        m_NPCManager.UnregisterNPC(this);
        {
            // Create the particle for an npc dying then destroy it after the particle has ended
            var transform1 = transform;
            var infect = Instantiate(explodePrefab, transform1.position, transform1.rotation);
            var ps = infect.GetComponentInChildren<ParticleSystem>();
            Destroy(infect, ps.main.duration * 2);
        }

        switch (m_NPCManager.currentMutation)
        {
            case InfectedMutations.leavesAoeAfterDeath:
                m_MeshRenderer.material = infectedPuddleMaterial;
                moveSpeed = 0f;
                transform.localScale = new Vector3(1.9f, 1.9f, 1f);
                break;

            case InfectedMutations.splitsIntoTwoUponDeath:
                if (isSplitChild || Random.value >= 0.5f)
                { 
                    Destroy(gameObject);
                    break;
                }

                Vector3 leftDir = Vector3.Cross(m_RigidBody.velocity, Vector3.forward);
                Vector3 rightDir = -leftDir;

                GameObject newNPC1 = Instantiate(NPCPrefab);
                GameObject newNPC2 = Instantiate(NPCPrefab);

                newNPC1.GetComponent<Rigidbody>().velocity = leftDir;
                newNPC2.GetComponent<Rigidbody>().velocity = rightDir;

                newNPC1.transform.localScale *= 0.6f;
                newNPC2.transform.localScale *= 0.6f;

                newNPC1.transform.parent = transform.parent;
                newNPC2.transform.parent = transform.parent;

                newNPC1.transform.position = transform.position;
                newNPC2.transform.position = transform.position;

                NPC NPC1 = newNPC1.GetComponent<NPC>();
                NPC NPC2 = newNPC2.GetComponent<NPC>();

                NPC1.infectedLifeTime *= 0.5f;
                NPC2.infectedLifeTime *= 0.5f;

                NPC1.isSplitChild = true;
                NPC2.isSplitChild = true;

                NPC1.OnInfected();
                NPC2.OnInfected();

                Destroy(gameObject);

                break;

            default:
                Destroy(gameObject);
                break;
        }
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

	public void OnLevelLoad() {
		m_NPCManager = FindObjectOfType<NPCManager>();
		m_NPCManager.RegisterNPC(this);
	}
	
	public void OnLevelStart() {
        Vector2 moveDir   = Random.insideUnitCircle.normalized;
        Vector3 moveForce = new Vector3(moveDir.x, moveDir.y, 0f) * moveSpeed;
        m_RigidBody.AddForce(moveForce, ForceMode.Impulse);
    }
}