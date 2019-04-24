using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class SpawnMeteorProxy : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject Prefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnMeteor = new MeteorSpawner
        {
            Prefab = conversionSystem.GetPrimaryEntity(Prefab)
        };
        dstManager.AddComponentData(entity, spawnMeteor);
    }

    public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
    {
        gameObjects.Add(Prefab);
    }
}
