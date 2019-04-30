using Unity.Entities;
using UnityEngine;

public class AddHealthProxy : MonoBehaviour, IConvertGameObjectToEntity
{
    public int healthValue = 100;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var health = new Health
        {
            Value = healthValue
        };
        dstManager.AddComponentData(entity, health);
    }
}
