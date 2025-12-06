// Example demonstrating how to make ANY View into a popover without implementing IPopover

using Terminal.Gui;
using Terminal.Gui.App;
using Terminal.Gui.Configuration;
using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using Attribute = Terminal.Gui.Drawing.Attribute;

IApplication app = Application.Create ();
app.Init ();

// Create a main window with some buttons to trigger popovers
Window mainWindow = new ()
{
    Title = "PopoverWrapper Example - Press buttons to show popovers",
    X = 0,
    Y = 0,
    Width = Dim.Fill (),
    Height = Dim.Fill ()
};

Label label = new ()
{
    Text = "Click buttons below or press their hotkeys to show different popovers.\nPress Esc to close a popover.",
    X = Pos.Center (),
    Y = 1,
    Width = Dim.Fill (),
    Height = 2,
    TextAlignment = Alignment.Center
};

mainWindow.Add (label);

// Example 1: Simple view as popover
Button button1 = new ()
{
    Title = "_1: Simple View Popover",
    X = Pos.Center (),
    Y = Pos.Top (label) + 3
};

button1.Accepting += (s, e) =>
{
    IApplication? application = (s as View)?.App;

    if (application is null)
    {
        return;
    }

    View simpleView = new ()
    {
        Title = "Simple Popover",
        Width = Dim.Auto (),
        Height = Dim.Auto (),
        SchemeName = SchemeManager.SchemesToSchemeName (Schemes.Menu)
    };

    simpleView.Add (
                    new Label
                    {
                        Text = "This is a simple View wrapped as a popover!\n\nPress Esc or click outside to dismiss.",
                        X = Pos.Center (),
                        Y = Pos.Center (),
                        TextAlignment = Alignment.Center
                    });

    PopoverWrapper<View> popover = simpleView.AsPopover ();
    popover.X = Pos.Center ();
    popover.Y = Pos.Center ();
    application.Popover?.Register (popover);
    application.Popover?.Show (popover);

    e.Handled = true;
};

mainWindow.Add (button1);

// Example 2: ListView as popover
Button button2 = new ()
{
    Title = "_2: ListView Popover",
    X = Pos.Center (),
    Y = Pos.Bottom (button1) + 1
};

ListView listView = new ()
{
    Title = "Select an Item",
    X = Pos.Center (),
    Y = Pos.Center (),
    Width = Dim.Percent (30),
    Height = Dim.Percent (40),
    BorderStyle = LineStyle.Single,
    Source = new ListWrapper<string> (["Apple", "Banana", "Cherry", "Date", "Elderberry", "Fig", "Grape"]),
    SelectedItem = 0
};

PopoverWrapper<ListView> listViewPopover = listView.AsPopover ();

listView.SelectedItemChanged += (sender, args) =>
{
    listViewPopover.Visible = false;

    if (listView.SelectedItem is { } selectedItem && selectedItem >= 0)
    {
        MessageBox.Query (app, "Selected", $"You selected: {listView.Source.ToList () [selectedItem]}", "OK");
    }
};

button2.Accepting += (s, e) =>
{
    IApplication? application = (s as View)?.App;

    if (application is null)
    {
        return;
    }

    listViewPopover.X = Pos.Center ();
    listViewPopover.Y = Pos.Center ();
    application.Popover?.Register (listViewPopover);
    application.Popover?.Show (listViewPopover);

    e.Handled = true;
};

mainWindow.Add (button2);

// Example 3: Form as popover
Button button3 = new ()
{
    Title = "_3: Form Popover",
    X = Pos.Center (),
    Y = Pos.Bottom (button2) + 1
};

button3.Accepting += (s, e) =>
{
    IApplication? application = (s as View)?.App;

    if (application is null)
    {
        return;
    }

    View formView = CreateFormView (application);
    PopoverWrapper<View> popover = formView.AsPopover ();
    popover.X = Pos.Center ();
    popover.Y = Pos.Center ();
    application.Popover?.Register (popover);
    application.Popover?.Show (popover);
    e.Handled = true;
};

mainWindow.Add (button3);

// Example 4: ColorPicker as popover
Button button4 = new ()
{
    Title = "_4: ColorPicker Popover",
    X = Pos.Center (),
    Y = Pos.Bottom (button3) + 1
};

button4.Accepting += (s, e) =>
{
    IApplication? application = (s as View)?.App;

    if (application is null)
    {
        return;
    }

    ColorPicker colorPicker = new ()
    {
        Title = "Pick a Border Color",
        BorderStyle = LineStyle.Single
    };

    colorPicker.Selecting += (sender, args) =>
    {
        ColorPicker? picker = sender as ColorPicker;

        if (picker is { })
        {
            Scheme old = application.TopRunnableView.Border.GetScheme ();
            application.TopRunnableView.Border.SetScheme (old with { Normal = new Attribute (picker.SelectedColor, Color.Black) });
        }
        args.Handled = true;
    };

    PopoverWrapper<ColorPicker> popover = colorPicker.AsPopover ();
    popover.X = Pos.Center ();
    popover.Y = Pos.Center ();
    application.Popover?.Register (popover);
    application.Popover?.Show (popover);

    e.Handled = true;
};

mainWindow.Add (button4);

// Example 5: Custom position and size
Button button5 = new ()
{
    Title = "_5: Positioned Popover",
    X = Pos.Center (),
    Y = Pos.Bottom (button4) + 1
};

button5.Accepting += (s, e) =>
{
    IApplication? application = (s as View)?.App;

    if (application is null)
    {
        return;
    }

    View customView = new ()
    {
        Title = "Custom Position",
        X = Pos.Percent (10),
        Y = Pos.Percent (10),
        Width = Dim.Percent (50),
        Height = Dim.Percent (60),
        BorderStyle = LineStyle.Double
    };

    customView.Add (
                    new Label
                    {
                        Text = "This popover has a custom position and size.\n\nYou can set X, Y, Width, and Height\nusing Pos and Dim to position it anywhere.",
                        X = 2,
                        Y = 2
                    });

    Button closeButton = new ()
    {
        Title = "Close",
        X = Pos.Center (),
        Y = Pos.AnchorEnd (1)
    };

    closeButton.Accepting += (sender, args) =>
    {
        if (customView.SuperView is PopoverWrapper<View> wrapper)
        {
            wrapper.Visible = false;
        }

        args.Handled = true;
    };

    customView.Add (closeButton);

    PopoverWrapper<View> popover = customView.AsPopover ();
    application.Popover?.Register (popover);
    popover.X = Pos.Center ();
    popover.Y = Pos.Center ();
    application.Popover?.Show (popover);

    e.Handled = true;
};

mainWindow.Add (button5);

// Quit button
Button quitButton = new ()
{
    Title = "_Quit",
    X = Pos.Center (),
    Y = Pos.AnchorEnd (1)
};

quitButton.Accepting += (s, e) =>
{
    app.RequestStop ();
    e.Handled = true;
};

mainWindow.Add (quitButton);

app.Run (mainWindow);
mainWindow.Dispose ();
app.Dispose ();

// Helper method to create a form view
View CreateFormView (IApplication application)
{
    View form = new ()
    {
        Title = "User Registration Form",
        X = Pos.Center (),
        Y = Pos.Center (),
        Width = Dim.Percent (60),
        Height = Dim.Percent (50),
        BorderStyle = LineStyle.Single
    };

    Label nameLabel = new ()
    {
        Text = "Name:",
        X = 2,
        Y = 1
    };

    TextField nameField = new ()
    {
        X = Pos.Right (nameLabel) + 2,
        Y = Pos.Top (nameLabel),
        Width = Dim.Fill (2)
    };

    Label emailLabel = new ()
    {
        Text = "Email:",
        X = Pos.Left (nameLabel),
        Y = Pos.Bottom (nameLabel) + 1
    };

    TextField emailField = new ()
    {
        X = Pos.Right (emailLabel) + 2,
        Y = Pos.Top (emailLabel),
        Width = Dim.Fill (2)
    };

    Label ageLabel = new ()
    {
        Text = "Age:",
        X = Pos.Left (nameLabel),
        Y = Pos.Bottom (emailLabel) + 1
    };

    TextField ageField = new ()
    {
        X = Pos.Right (ageLabel) + 2,
        Y = Pos.Top (ageLabel),
        Width = Dim.Percent (20)
    };

    CheckBox agreeCheckbox = new ()
    {
        Title = "I agree to the terms and conditions",
        X = Pos.Left (nameLabel),
        Y = Pos.Bottom (ageLabel) + 1
    };

    Button submitButton = new ()
    {
        Title = "Submit",
        X = Pos.Center () - 8,
        Y = Pos.AnchorEnd (2),
        IsDefault = true
    };

    submitButton.Accepting += (s, e) =>
    {
        if (string.IsNullOrWhiteSpace (nameField.Text))
        {
            MessageBox.ErrorQuery (application, "Error", "Name is required!", "OK");
            e.Handled = true;

            return;
        }

        if (agreeCheckbox.CheckedState != CheckState.Checked)
        {
            MessageBox.ErrorQuery (application, "Error", "You must agree to the terms!", "OK");
            e.Handled = true;

            return;
        }

        MessageBox.Query (
                          application,
                          "Success",
                          $"Registration submitted!\n\nName: {nameField.Text}\nEmail: {emailField.Text}\nAge: {ageField.Text}",
                          "OK");

        if (form.SuperView is PopoverWrapper<View> wrapper)
        {
            wrapper.Visible = false;
        }

        e.Handled = true;
    };

    Button cancelButton = new ()
    {
        Title = "Cancel",
        X = Pos.Center () + 4,
        Y = Pos.Top (submitButton)
    };

    cancelButton.Accepting += (s, e) =>
    {
        if (form.SuperView is PopoverWrapper<View> wrapper)
        {
            wrapper.Visible = false;
        }

        e.Handled = true;
    };

    form.Add (nameLabel, nameField);
    form.Add (emailLabel, emailField);
    form.Add (ageLabel, ageField);
    form.Add (agreeCheckbox);
    form.Add (submitButton, cancelButton);

    return form;
}
