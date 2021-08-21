using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoint : MonoBehaviour
{
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;


    private List<Checkpoint> checkpointList;
    private int nextCheckpointIndex;
    private void Awake()
    {
        Transform checkpointManagerTransform = transform.Find("CheckpointManager");

        checkpointList = new List<Checkpoint>();
        foreach (Transform checkpointTransform in checkpointManagerTransform)
        {
            Checkpoint checkpoint = checkpointTransform.GetComponent<Checkpoint>();

            checkpoint.SetTrackCheckpoints(this);
            
            checkpointList.Add(checkpoint);
        }

        nextCheckpointIndex = 0; 
    }
    public void PlayerTroughCheckpoint(Checkpoint checkpoint)
    {
        if (checkpointList.IndexOf(checkpoint) == nextCheckpointIndex)
        {
            nextCheckpointIndex = (nextCheckpointIndex + 1) % checkpointList.Count;
            print("Correct");
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            print("Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

        }
    }
}
