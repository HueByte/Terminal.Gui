#nullable enable

namespace UICatalog.Scenarios;

[ScenarioMetadata ("DropDownListExample", "Shows how to use MenuBar as a Drop Down List")]
[ScenarioCategory ("Controls")]
public sealed class DropDownListExample : Scenario
{
    public override void Main ()
    {
        // Init
        using IApplication app = Application.Create ();
        app.Init ();

        // Setup - Create a top-level application window and configure it.
        using Window appWindow = new ();
        appWindow.Title = GetQuitKeyAndName ();
        appWindow.BorderStyle = LineStyle.None;

        var label = new Label { Title = "_DropDown TextField Using Menu:" };
        View view = CreateDropDownTextFieldUsingMenu ();
        view.X = Pos.Right (label) + 1;

        appWindow.Add (label, view);

        // Run - Start the application.
        app.Run (appWindow);
    }

    private View CreateDropDownTextFieldUsingMenu ()
    {
        TextField tf = new () { Text = "item 1", Width = 10, Height = 1 };

        MenuBarItem? menuBarItem = new ($"{Glyphs.DownArrow}",
                                        Enumerable.Range (1, 5)
                                                  .Select (i =>
                                                           {
                                                               var item = new MenuItem ($"item {i}", null, null, null);

                                                               item.Accepting += (s, e) =>
                                                                                 {
                                                                                     tf.Text = item.Title;

                                                                                     //e.Handled = true;
                                                                                 };

                                                               return item;
                                                           })
                                                  .ToArray ()) { MarginThickness = Thickness.Empty };

        menuBarItem.PopoverMenuOpenChanged += (s, e) =>
                                              {
                                                  if (e.Value && s is MenuBarItem sender)
                                                  {
                                                      sender.PopoverMenu?.Root?.X = tf.FrameToScreen ().X;
                                                      sender.PopoverMenu?.Root?.Width = tf.Width + sender.Width;

                                                      // Find the subview of Root whos Text matches tf.Text and setfocus to it
                                                      MenuItem? menuItemToSelect = sender.PopoverMenu?.Root?.SubViews.OfType<MenuItem> ()
                                                                                         .FirstOrDefault (mi => mi.Title == tf.Text);
                                                      menuItemToSelect?.SetFocus ();
                                                  }
                                              };

        var mb = new MenuBar ([menuBarItem]) { CanFocus = true, Width = Dim.Auto (), Y = Pos.Top (tf), X = Pos.Right (tf) };

        // HACKS required to make this work:
        mb.Accepted += (s, e) =>
                       {
                           // BUG: This does not select menu item 0
                           // Instead what happens is the first keystroke the user presses
                           // gets swallowed and focus is moved to 0.  Result is that you have
                           // to press down arrow twice to select first menu item and/or have to
                           // press Tab twice to move focus back to TextField
                           mb.OpenMenu ();
                       };

        View superView = new () { CanFocus = true, Height = Dim.Auto (), Width = Dim.Auto () };
        superView.Add (tf, mb);

        return superView;
    }

    //private View  CreateDropDownTextFieldUsingListView ()
    //{
    //    TextField tf = new ()
    //    {
    //        Text = "item 1",
    //        Width = 10,
    //        Height = 1
    //    };

    //    ListView listView = new ()
    //    {
    //        Source = new ListWrapper<string> (["item 1", "item 2", "item 3", "item 4", "item 5"]),
    //    };

    //    MenuBarItem? menuBarItem = new ($"{Glyphs.DownArrow}",  )
    //    {
    //        MarginThickness = Thickness.Empty
    //    };

    //    menuBarItem.PopoverMenuOpenChanged += (s, e) =>
    //    {
    //        if (e.Value && s is MenuBarItem sender)
    //        {
    //            sender.PopoverMenu!.Root.X = tf.FrameToScreen ().X;
    //            sender.PopoverMenu.Root.Width = tf.Width + sender.Width;
    //            // Find the subview of Root whos Text matches tf.Text and setfocus to it
    //            var menuItemToSelect = sender.PopoverMenu.Root.SubViews.OfType<MenuItem> ().FirstOrDefault (mi => mi.Title == tf.Text.ToString ());
    //            menuItemToSelect?.SetFocus ();
    //        }
    //    };

    //    var mb = new MenuBar ([menuBarItem])
    //    {
    //        CanFocus = true,
    //        Width = Dim.Auto (),
    //        Y = Pos.Top (tf),
    //        X = Pos.Right (tf)
    //    };

    //    // HACKS required to make this work:
    //    mb.Accepted += (s, e) =>
    //    {
    //        // BUG: This does not select menu item 0
    //        // Instead what happens is the first keystroke the user presses
    //        // gets swallowed and focus is moved to 0.  Result is that you have
    //        // to press down arrow twice to select first menu item and/or have to
    //        // press Tab twice to move focus back to TextField
    //        mb.OpenMenu ();
    //    };

    //    View superView = new ()
    //    {
    //        CanFocus = true,
    //        Height = Dim.Auto (),
    //        Width = Dim.Auto ()
    //    };
    //    superView.Add (tf, mb);

    //    return superView;

    //}
}
