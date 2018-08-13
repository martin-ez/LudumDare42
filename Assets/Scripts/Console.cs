using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    public float maxVelocity = 1f;
    public float span = 3f;
    public Vector3 scale = Vector3.one;

    Rigidbody rb;
    Spawner spawner;

    int level;

    bool falling;
    bool contact;
    bool set;

    float aniTime;
    float aniInterval;
    bool aniRigth;
    Vector3 leftPos;
    Vector3 rigthPos;

    Transform guides;
    LineRenderer guideLine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        falling = false;
        contact = false;
        set = false;
    }

    public void SetSpawner(Spawner sp, int lvl)
    {
        spawner = sp;
        level = lvl;
        aniInterval = Mathf.Lerp(4f, 1f, level / 5);
        leftPos = new Vector3(-span, sp.transform.position.y + 5f, 0f);
        rigthPos = new Vector3(span, sp.transform.position.y + 5f, 0f);
        transform.position = leftPos;
        aniRigth = true;
        float chance = Random.value;
        if (chance <= 0.5f)
        {
            transform.position = rigthPos;
            aniRigth = false;
        }
        aniTime = 0f;
        scale = GetComponent<BoxCollider>().size;
        SetGuideLines();
    }

    private void FixedUpdate()
    {
        if (!set && !falling)
        {
            bool space = Input.GetKey(KeyCode.Space);
            AdjustGuides();
            if (space)
            {
                transform.Find("GL").gameObject.SetActive(false);
                falling = true;
                rb.useGravity = true;
            }
            else
            {
                aniTime += Time.deltaTime;

                float i = aniTime / aniInterval;
                if (i > 1)
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

        if (!set && Mathf.Abs(rb.velocity.y) > maxVelocity)
        {
            rb.velocity = Vector3.down * maxVelocity;
        }

        if (!set && contact && rb.velocity.sqrMagnitude < 0.01f && rb.angularVelocity.sqrMagnitude < 0.01f)
        {
            set = true;
            spawner.Next();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!set && collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().PlaySound(AudioManager.Sound.Hit);
            contact = true;
        }
    }

    void SetGuideLines()
    {
        guides = transform.Find("GL/Sqr");
        guideLine = transform.Find("GL/Line").GetComponent<LineRenderer>();
        Vector3 linePos = new Vector3();
        Vector3 xVector = (Vector3.right * scale.x / 2f);
        Vector3 zVector = (Vector3.forward * scale.z / 2f);

        LineRenderer lr = guides.GetComponent<LineRenderer>();

        int xMul = 1;
        int zMul = 1;

        for (int i = 0; i < 4; i++)
        {
            if (i < 2)
            {
                xMul = -1;
            }
            else
            {
                xMul = 1;
            }
            if (i == 0 || i == 3)
            {
                zMul = -1;
            }
            else
            {
                zMul = 1;
            }
            linePos = (xMul * xVector + zMul * zVector);
            lr.SetPosition(i, linePos);
        }
    }

    void AdjustGuides()
    {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(downRay, out hit))
        {
            guides.localPosition = Vector3.down * (hit.distance - 0.1f);
            guideLine.SetPosition(1, Vector3.down * hit.distance);
        }
        else
        {
            guides.localPosition = Vector3.down * 100;
            guideLine.SetPosition(1, Vector3.down * 100);
        }
    }
}
