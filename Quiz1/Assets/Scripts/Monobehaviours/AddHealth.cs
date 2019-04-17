using Unity.Entities;
using UnityEngine;

public class AddHealth : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var health = new Health
        {
            Value = 100
        };
        dstManager.AddComponentData(entity, health);
    }
    
    
}
