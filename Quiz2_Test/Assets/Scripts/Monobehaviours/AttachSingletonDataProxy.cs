using Unity.Entities;
using UnityEngine;

public class AttachSingletonDataProxy : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var nativeSingleton = new SingletonNativeEconomy
        {
            Gold = 0,
            Lumber = 0,
            Population = 0,
            WinCondition = false
        };
        dstManager.AddComponentData(entity, nativeSingleton);

        var singleton = new SingletonEconomy
        {
            Gold = 0,
            Lumber = 0,
            Population = 0,
            WinCondition = false
        };
        dstManager.AddComponentData(entity, singleton);
    }
}
