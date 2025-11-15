using Godot;

public partial class MessageManager : Node
{
    public static MessageManager Instance { get; private set; }

    private Label _messageLabel;

    public override void _Ready()
    {
        Instance = this;

        _messageLabel = new Label();
        _messageLabel.Position = new Vector2(50, 400);
        _messageLabel.Size = new Vector2(700, 100);
        _messageLabel.Visible = false;
        AddChild(_messageLabel);

    }

    public static void PlayText(params string[] lines)
    {
        if (Instance == null)
        {
            GD.PrintErr("MessageManager Instance is null!");
            return;
        }

        Instance._messageLabel.Visible = true;
        Instance._messageLabel.Text = string.Join("\n", lines);

        Instance.GetTree().CreateTimer(1.5).Timeout += CloseMessageBox;
    }

    private static void CloseMessageBox()
    {
        if (Instance != null && Instance._messageLabel != null)
        {
            Instance._messageLabel.Visible = false;
        }
    }
}