using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public float checkpointMoveWaitTime = .5f;

    private Coroutine arrivedAtCheckpoint;

    public void OnTriggerEnter(Collider other)
    {
        //Might need a check on player to avoid enemy collision
        if (arrivedAtCheckpoint == null)
        {
            arrivedAtCheckpoint = StartCoroutine(ArrivedAtCheckpoint());
        }
    }

    IEnumerator ArrivedAtCheckpoint()
    {
        ObjectPoolingManager.Instance.SwitchCheckpoint();
        //Particles to start for movement or animation
        AudioManager.Instance.PlayAudio(AudioTypeEnum.SoundEffects_02, 0f, true, .1f);
        yield return new WaitForSeconds(checkpointMoveWaitTime);
        ObjectPoolingManager.Instance.MoveCheckpoint();

        arrivedAtCheckpoint = null;
    }
}
