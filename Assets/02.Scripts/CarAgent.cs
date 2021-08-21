using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    private Transform tr;
    private Rigidbody rb;
    private Transform targetTr;

    //private Transform targetTr;
    public TrackCheckpoint trackCheckpoint;


    public float moveSpeed = 1.5f;
    public float turnSpeed = 20.0f;


    Vector3 rotationRight = new Vector3(0, -30, 0);
    Vector3 rotationLeft = new Vector3(0, 30, 0);
    

    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        targetTr = tr.parent.Find("CheckPoint").GetComponent<Transform>();

        MaxStep = 2000;
    }

    IEnumerator RevertMaterial(Material changedMt)
    {
        yield return new WaitForSeconds(0.2f);
    }

    public override void OnEpisodeBegin()
    {

        rb.velocity = rb.angularVelocity = Vector3.zero;

        tr.localPosition = new Vector3(0.0f, 0.2f, 0.0f);
        tr.localRotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(targetTr.localPosition);  // (x, y, z)  3
        sensor.AddObservation(tr.localPosition);        // (x, y, z)  3
        sensor.AddObservation(rb.velocity.x);           // 1
        sensor.AddObservation(rb.velocity.z);           // 1
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions;

        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;

        // Branch 0
        switch (action[0])
        {
            case 1: dir = tr.forward; break;
            case 2: dir = -tr.forward; break;
        }
        // Branch 1
        switch (action[1])
        {
            case 1: rot = -tr.up; break; //���� ȸ��
            case 2: rot = tr.up; break;  //������ ȸ��
        }


        // ���̳ʽ� �г�Ƽ
        SetReward(-1 / (float)MaxStep); // 2000 -> 0.002 
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        actions.Clear();

        // Branch 0 - �̵� (����/����/����) 0, 1, 2 : Size 3
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1; //����
        }
        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = 2; //����
        }
        // Branch 1 - ȸ�� (����/����ȸ��/������ȸ��) 0, 1, 2 : Size 3
        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 1; //���� ȸ��
        }
        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 2; //������ ȸ��
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        //TrackCheckpoint trackCheckpoint = coll.transform.GetComponent<TrackCheckpoint>();
        
        //trackCheckpoint.check

        if (coll.gameObject.name == "Floor") return;

        //if (trackCheckpoint.PlayerTroughCheckpoint(string s in trackCheckpoint.GetComponent) == )
        //{
            
        //}
        //if (coll.collider.tag == TrackCheckpoint.hintColor.ToString())
        //{
        //    SetReward(+1.0f);
        //    EndEpisode();
        //    StartCoroutine(RevertMaterial(goodMt));
        //}
        //if
        //{
        if (coll.collider.CompareTag("WALL")/* || coll.gameObject.name == "Hint"*/)
        {
            SetReward(-0.05f);
            EndEpisode();
        }
        //else
        //{
        //    SetReward(-1.0f);
        //    EndEpisode();
        //    StartCoroutine(RevertMaterial(badMt));
        //}
        //}
    }
}