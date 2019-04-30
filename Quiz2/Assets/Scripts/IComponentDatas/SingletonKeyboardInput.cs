﻿using Unity.Entities;

public struct SingletonKeyboardInput : IComponentData
{
    public bool SpaceBar;
    public bool Q_Key;
    public bool W_Key;
    public bool E_Key;
    public bool E_KeyActive;
    public bool R_Key;
    public bool F_Key;
    public float HorizontalMovement;
    public float VerticalMovement;
}
