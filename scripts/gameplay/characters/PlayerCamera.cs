using Godot;

public partial class PlayerCamera : Camera2D
{
    public override void _Ready()
    {
        Enabled = true;
        GD.Print("[PlayerCamera] Ready!");
    }
}