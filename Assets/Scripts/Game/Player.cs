using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;

    Vector3 movement;

    Rigidbody m_RigidBody;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        movement.z  = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        m_RigidBody.velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        m_RigidBody.MovePosition(m_RigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        NPC hitNPC;
        if (collision.gameObject.TryGetComponent<NPC>(out hitNPC))
        {
            if (hitNPC.isInfected)
                OnInfected();
        }
    }

    private void OnInfected()
    {
        Destroy(gameObject);
    }
}
