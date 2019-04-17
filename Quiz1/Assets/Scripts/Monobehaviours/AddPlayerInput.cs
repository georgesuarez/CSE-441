using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[RequiresEntityConversion]
public class AddPlayerInput : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var mouseInput = new SingletonMouseInput
        {
            RightClickDown = false,
            LeftClickDown = false,
            RightClickUp = false,
            LeftClickUp = false,
            MousePosition = float3.zero,
            MouseRaycastPosition = float3.zero
        };
        dstManager.AddComponentData(entity, mouseInput);
    }
}
