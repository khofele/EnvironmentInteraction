using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLeanable : Leanable
{
    private Transform resetTransformOne = null;
    private Transform resetTransformTwo = null;
    private Transform snapTransformOne = null;
    private Transform snapTransformTwo = null;
    private int triggerCount = 0;

    public Transform ResetTransformOne { get => resetTransformOne; }
    public Transform ResetTransformTwo { get => resetTransformTwo; }
    public Transform SnapTransformOne { get => snapTransformOne; }
    public Transform SnapTransformTwo { get => snapTransformTwo; }
    public int TriggerCount { get => triggerCount; set => triggerCount = value; }   

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.GetComponent<CharController>() != null)
        {
            triggerCount++;
        }
    }

    private void GetTransforms()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform transform in children)
        {
            if (transform.CompareTag("SnapTransformOne"))
            {
                snapTransformOne = transform;
            }
            else if (transform.CompareTag("SnapTransformTwo"))
            {
                snapTransformTwo = transform;
            }
            else if (transform.CompareTag("ResetTransformOne"))
            {
                resetTransformOne = transform;
            }
            else if (transform.CompareTag("ResetTransformTwo"))
            {
                resetTransformTwo = transform;
            }
        }

        if (snapTransformOne == null || snapTransformTwo == null)
        {
            Debug.LogWarning("Snaptransform is missing!");
        }

        if (resetTransformOne == null || resetTransformTwo == null)
        {
            Debug.LogWarning("Resettransform is missing!");
        }
    }

    protected override void Validate()
    {
        base.Validate();
        GetTransforms();
    }
}
