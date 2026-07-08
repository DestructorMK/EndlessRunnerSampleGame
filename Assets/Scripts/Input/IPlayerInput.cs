/// <summary>
/// Abstraction over "what is the player asking the character to do this frame".
/// KeyboardPlayerInput is the only implementation today; a Kinect-skeleton implementation
/// can be swapped in later without touching CharacterInputController.
/// </summary>
public interface IPlayerInput
{
    // -1 = left, 1 = right, 0 = no lane-change request this frame.
    int LaneDirection();
    bool JumpPressed();
    bool DuckPressed();
}
