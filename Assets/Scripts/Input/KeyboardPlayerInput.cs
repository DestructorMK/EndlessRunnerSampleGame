using UnityEngine;

public class KeyboardPlayerInput : IPlayerInput
{
    public int LaneDirection()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            return -1;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            return 1;
        return 0;
    }

    public bool JumpPressed()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    public bool DuckPressed()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }

    public bool VaultPressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
