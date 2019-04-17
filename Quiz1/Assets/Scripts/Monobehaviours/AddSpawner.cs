using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;

public class AddSpawner : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject Prefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnerData = new UnitSpawner
        {
            Prefab = conversionSystem.GetPrimaryEntity(Prefab),
        };
        dstManager.AddComponentData(entity, spawnerData);
    }

    public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
    {
        gameObjects.Add(Prefab);
    }
}
