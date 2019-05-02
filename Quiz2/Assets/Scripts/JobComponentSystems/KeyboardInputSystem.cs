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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            keyboardInput.SpaceBar = true;
        }
        else
        {
            keyboardInput.SpaceBar = false;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            keyboardInput.Q_Key = true;

            if (!keyboardInput.Q_KeyActive)
            {
                keyboardInput.Q_KeyActive = true;
            }
        }
        else
        {
            keyboardInput.Q_Key = false;
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            keyboardInput.W_Key = true;
        }
        else
        {
            keyboardInput.W_Key = false;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            keyboardInput.E_Key = true;
        }
        else
        {
            keyboardInput.E_Key = false;
        }


        if (Input.GetKeyDown(KeyCode.R))
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
