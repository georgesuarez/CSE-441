using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MouseInputSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var mouseInput = GetSingleton<SingletonMouseInput>();

        mouseInput.MousePosition = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mouseInput.MousePosition);
       
        RaycastHit hit;
        if ( Physics.Raycast(ray, out hit) )
        {
            mouseInput.MouseRaycastPosition = hit.point;
        }

        if ( Input.GetMouseButtonDown(0) )
        {
            mouseInput.LeftClickDown = true;
            mouseInput.RightClickUp = false;
        }
        else if ( Input.GetMouseButtonUp(0) )
        {
            mouseInput.LeftClickUp = true;
            mouseInput.LeftClickDown = false;
        }
        if ( Input.GetMouseButtonDown(1) )
        {
            mouseInput.RightClickDown = true;
            mouseInput.RightClickUp = false;
        }
        else if ( Input.GetMouseButtonUp(1) )
        {
            mouseInput.RightClickUp = true;
            mouseInput.RightClickDown = false;
        }
        else
        {
            mouseInput.RightClickUp = false;
            mouseInput.LeftClickUp = false;
        }

        SetSingleton<SingletonMouseInput>(mouseInput);
    }
}

/*
 * var direction = ray.direction - mouseInput.MousePosition;
 * 
       RaycastInput raycastInput = new RaycastInput()
       {
           Ray = new Unity.Physics.Ray()
           {
               Origin = mousePos,
               Direction = direction
           },
           Filter = new CollisionFilter()
           {
               CategoryBits = ~0u,
               MaskBits = ~0u,
               GroupIndex = 0
           }
       };
       Unity.Physics.RaycastHit hit;

       var physicsWorldSystem = Unity.Entities.World.Active.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>();
       var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;

       bool hasHit = collisionWorld.CastRay(raycastInput, out hit);
       if ( hasHit )
       {
           mouseInput.MouseRaycastPosition = hit.Position;
       }
*/