using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    public float maxVelocity = 1f;
    public float speed = 1f;
    public float span = 5f;

    Rigidbody rb;
    Spawner spawner;

    bool falling;
    bool contact;
    bool set;
    Spawner sp;

    float aniTime;
    float aniInterval;
    bool aniRigth;
    Vector3 leftPos;
    Vector3 rigthPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        falling = false;
        contact = false;
        set = false;

        leftPos = new Vector3(-span, 7.5f, 0f);
        rigthPos = new Vector3(span, 7.5f, 0f);
    }

    public void SetSpawner(Spawner sp)
    {
        spawner = sp;
        aniInterval = 5f / speed;
        rb.MovePosition(leftPos);
        aniRigth = true;
        float chance = Random.value;
        if (chance <= 0.5f)
        {
            rb.MovePosition(rigthPos);
            aniRigth = false;
        }
        aniTime = 0f;
    }

    private void FixedUpdate()
    {
        if (set) return;
        if (!falling)
        {
            bool space = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0);
            if (space)
            {
                falling = true;
                rb.useGravity = true;
            }
            else
            {
                aniTime += Time.deltaTime;

                float i = aniTime / aniInterval;
                if ( i > 1)
                {
                    i = 0f;
                    aniTime = 0f;
                    aniRigth = !aniRigth;
                }
                if (aniRigth)
                {
                    rb.MovePosition(Vector3.Lerp(leftPos, rigthPos, i));
                }
                else
                {
                    rb.MovePosition(Vector3.Lerp(rigthPos, leftPos, i));
                }
            }
        }

        if (Mathf.Abs(rb.velocity.y) > maxVelocity)
        {
            rb.velocity = Vector3.down * maxVelocity;
        }

        if (contact && Mathf.Approximately(rb.velocity.sqrMagnitude, 0f) && Mathf.Approximately(rb.angularVelocity.sqrMagnitude, 0f))
        {
            rb.mass = 10;
            set = true;
            spawner.Next();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            contact = true;
        }
    }
}
