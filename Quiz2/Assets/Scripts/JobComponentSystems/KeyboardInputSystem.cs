using Unity.Jobs;
using Unity.Entities;
using UnityEngine;

public class KeyboardInputSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var keyboardInput = GetSingleton<SingletonKeyboardInput>();

        keyboardInput.HorizontalMovement = Input.GetAxis("Horizontal");
        keyboardInput.VerticalMovement = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
        {
            keyboardInput.SpaceBar = true;
        }
        else
        {
            keyboardInput.SpaceBar = false;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            keyboardInput.Q_Key = true;
        }
        else
        {
            keyboardInput.Q_Key = false;
        }


        if (Input.GetKey(KeyCode.W))
        {
            keyboardInput.W_Key = true;
        }
        else
        {
            keyboardInput.W_Key = false;
        }


        if (Input.GetKey(KeyCode.E))
        {
            keyboardInput.E_Key = true;
        }
        else
        {
            keyboardInput.E_Key = false;
        }


        if (Input.GetKey(KeyCode.R))
        {
            keyboardInput.R_Key = true;
        }
        else
        {
            keyboardInput.R_Key = false;
        }

        SetSingleton<SingletonKeyboardInput>(keyboardInput);

        return inputDeps;
    }
}
