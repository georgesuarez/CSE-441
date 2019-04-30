using Unity.Entities;
using UnityEngine;

public class AttachWorkerDataProxy : MonoBehaviour, IConvertGameObjectToEntity
{
    public float goldAmount;
    public float lumberAmount;
    public int populationAmount;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var worker = new Worker
        {
            Gold = goldAmount,
            Lumber = lumberAmount,
            Population = populationAmount
        };
        dstManager.AddComponentData(entity, worker);
    }
}
