using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public float checkpointMoveWaitTime = .5f;

    private Coroutine arrivedAtCheckpoint;

    public void OnTriggerEnter(Collider other)
    {
        if (arrivedAtCheckpoint == null)
        {
            arrivedAtCheckpoint = StartCoroutine(ArrivedAtCheckpoint());
        }
    }

    IEnumerator ArrivedAtCheckpoint()
    {
        ObjectPoolingManager.Instance.SwitchCheckpoint();
        //Particles to start for movement or animation
        yield return new WaitForSeconds(checkpointMoveWaitTime);
        ObjectPoolingManager.Instance.MoveCheckpoint();

        arrivedAtCheckpoint = null;
    }
}
