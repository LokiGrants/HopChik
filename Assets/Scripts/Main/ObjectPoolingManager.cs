using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : Singleton<ObjectPoolingManager>
{
    public Transform checkpointObject;
    public List<Transform> checkpointPositions;

    public Material checkpointMaterial, checkpointMaterialShadow;
    public List<Texture> allPossibleFoods;

    private int listIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        SwitchCheckpoint();
        MoveCheckpoint();
    }

    public void SwitchCheckpoint()
    {
        PlayerController.Instance.ChangeCheckpoint(checkpointPositions[listIndex]);
    }

    public void MoveCheckpoint()
    {
        listIndex++;
        if (listIndex < checkpointPositions.Count-1)
        {
            checkpointObject.position = checkpointPositions[listIndex].position;
            RandomizeFruit();
        } else
        {
            checkpointObject.gameObject.SetActive(false);
        }
    }

    public void RestartLevel()
    {
        listIndex = 0;
        checkpointObject.gameObject.SetActive(true);
        SwitchCheckpoint();
        MoveCheckpoint();
        ReviveLevel();
    }

    public void ReviveLevel()
    {
        PlayerController.Instance.Respawn();
    }

    public void RandomizeFruit()
    {
        Texture chosenFood = allPossibleFoods[Random.Range(0, allPossibleFoods.Count)];

        checkpointMaterial.mainTexture = chosenFood;
        checkpointMaterialShadow.mainTexture = chosenFood;
    }
}
