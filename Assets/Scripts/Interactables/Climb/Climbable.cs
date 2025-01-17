using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Climbable : Interactable
{
    [SerializeField] private Transform topTransform = null;
    [SerializeField] private Transform climbDownTransform = null;
    private TopClimbingTriggerCheck topClimbingTriggerCheck = null;
    private int triggerCount = 0;
    private List<ClimbingStone> climbingStones = new List<ClimbingStone>();

    public int TriggerCount { get => triggerCount; set => triggerCount = value; }
    public TopClimbingTriggerCheck TopClimbingTriggerCheck { get => topClimbingTriggerCheck; }
    public Transform TopTransform { get => topTransform; }
    public Transform ClimbDownTransform { get => climbDownTransform; }
    public List<ClimbingStone> ClimbingStones { get => climbingStones; }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.GetComponent<CharController>() != null)
        {
            triggerCount++;
        }
    }

    private void FillList()
    {
        climbingStones = GetComponentsInChildren<ClimbingStone>().ToList();
    }

    protected override void Start()
    {
        FillList();

        GetComponent<Collider>().isTrigger = false; 

        topClimbingTriggerCheck = GetComponentInChildren<TopClimbingTriggerCheck>();

        base.Start();
    }

    protected override void Validate()
    {
        base.Validate();

        if (topTransform == null)
        {
            Debug.LogWarning("Top transform is not assigned!");
        }

        if (climbDownTransform == null)
        {
            Debug.LogWarning("Climb down transform is not assigned!");
        }

        if (topClimbingTriggerCheck == null)
        {
            Debug.LogWarning("Triggercheck is not assigned!");
        }
    }
}
