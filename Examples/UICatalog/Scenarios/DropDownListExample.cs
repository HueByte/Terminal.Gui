#nullable enable

namespace UICatalog.Scenarios;

[ScenarioMetadata ("DropDownListExample", "Shows how to use MenuBar as a Drop Down List")]
[ScenarioCategory ("Controls")]
public sealed class DropDownListExample : Scenario
{
    public override void Main ()
    {
        // Init
        Application.Init ();

        // Setup - Create a top-level application window and configure it.
        Window appWindow = new ()
        {
            Title = GetQuitKeyAndName (),
            BorderStyle = LineStyle.None
        };

        Label l = new Label () { Title = "_DropDown:" };

        TextField tf = new () { X = Pos.Right(l), Width = 10 };

        MenuBarItem? menuBarItem  = new ($"{Glyphs.DownArrow}",
                                Enumerable.Range (1, 5)
                                          .Select (selector: i => new MenuItem($"item {i}", null, null, null) )
                                          .ToArray ());

        var mb = new MenuBar ([menuBarItem])
        {
            CanFocus = true,
            Width = 1,
            Y = Pos.Top (tf),
            X = Pos.Right (tf)
        };

        // HACKS required to make this work:
        mb.Accepted += (s, e) => {
                        // BUG: This does not select menu item 0
                        // Instead what happens is the first keystroke the user presses
                        // gets swallowed and focus is moved to 0.  Result is that you have
                        // to press down arrow twice to select first menu item and/or have to
                        // press Tab twice to move focus back to TextField
                        mb.OpenMenu ();
                    };

        appWindow.Add (l, tf, mb);

        // Run - Start the application.
        Application.Run (appWindow);
        appWindow.Dispose ();

        // Shutdown - Calling Application.Shutdown is required.
        Application.Shutdown ();
    }
}
