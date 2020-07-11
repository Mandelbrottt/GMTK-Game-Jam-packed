using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float roundStartInvincibilityTime;

    public Material invincibleMaterial;
    public Material standardMaterial;

	public UnityEvent OnPlayerInfected;

    Vector3 movement;

    MeshRenderer m_MeshRenderer;
    Rigidbody    m_RigidBody;

    float m_InvincibilityCountdown;

    // Start is called before the first frame update
    void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_RigidBody    = GetComponent<Rigidbody>();

        movement.z  = 0f;

        OnRoundStart();

		var playerManager = FindObjectOfType<PlayerManager>();
		playerManager.RegisterPlayer(this);
	}

    // Update is called once per frame
    void Update()
    {
        m_InvincibilityCountdown -= Time.deltaTime;
        if (m_InvincibilityCountdown < 0f)
            m_MeshRenderer.material = standardMaterial;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        m_RigidBody.velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        m_RigidBody.MovePosition(m_RigidBody.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    public void OnRoundStart()
    {
        m_InvincibilityCountdown = roundStartInvincibilityTime;
        m_MeshRenderer.material  = invincibleMaterial;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_InvincibilityCountdown > 0f)
            return;

        NPC hitNPC;
        if (collision.gameObject.TryGetComponent<NPC>(out hitNPC))
        {
            if (hitNPC.isInfected)
                OnInfected();
        }
    }

    private void OnInfected()
    {
		OnPlayerInfected?.Invoke();
		
        Destroy(gameObject);
	}
}
