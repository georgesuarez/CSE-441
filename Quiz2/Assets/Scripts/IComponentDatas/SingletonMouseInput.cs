using Unity.Entities;
using Unity.Mathematics;

public struct SingletonMouseInput : IComponentData
{
    public bool LeftClickDown;
    public bool RightClickDown;
    public bool LeftClickUp;
    public bool RightClickUp;
    public float3 InitialMouseClickPosition;
    public float3 CurrentMousePosition;
    public float3 InitialMouseRaycastPosition;
    public float3 CurrentMouseRaycastPosition;
}