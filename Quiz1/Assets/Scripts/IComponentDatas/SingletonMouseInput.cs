using Unity.Entities;
using Unity.Mathematics;

public struct SingletonMouseInput : IComponentData
{
    public bool LeftClickDown;
    public bool RightClickDown;
    public bool LeftClickUp;
    public bool RightClickUp;
    public float3 MousePosition;
    public float3 MouseRaycastPosition;
}