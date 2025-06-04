using Godot;

public partial class InventoryUi : Control
{
    private bool is_closed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        close();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("inventoryMenu"))
        {
            GD.Print("t was just pressed\n");
            if (!is_closed)
                close();
            else
                open();
        }
    }

    private void close()
    {
        Visible = false;
        is_closed = true;
    }

    private void open()
    {
        Visible = true;
        is_closed = false;
    }
}