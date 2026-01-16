using Terminal.Gui.App;
using Terminal.Gui.Views;

namespace ApplicationTests;

public class PopoverWrapperTests
{
    [Fact]
    public void Constructor_SetsDefaults ()
    {
        var wrapper = new PopoverWrapper<View> { WrappedView = new View () };

        Assert.Equal ("popoverWrapper", wrapper.Id);
        Assert.True (wrapper.CanFocus);
        //Assert.Equal (Dim.Fill (), wrapper.Width);
        //Assert.Equal (Dim.Fill (), wrapper.Height);
        Assert.True (wrapper.ViewportSettings.HasFlag (ViewportSettingsFlags.Transparent));
        Assert.True (wrapper.ViewportSettings.HasFlag (ViewportSettingsFlags.TransparentMouse));
    }

    [Fact]
    public void WrappedView_CanBeSet ()
    {
        var view = new View { Id = "testView" };
        var wrapper = new PopoverWrapper<View> { WrappedView = view };

        Assert.Same (view, wrapper.WrappedView);
        Assert.Equal ("testView", wrapper.WrappedView.Id);
    }

    [Fact]
    public void EndInit_AddsWrappedViewAsSubview ()
    {
        var view = new View { Id = "wrapped" };
        var wrapper = new PopoverWrapper<View> { WrappedView = view };

        wrapper.BeginInit ();
        wrapper.EndInit ();

        Assert.Contains (view, wrapper.SubViews);
        Assert.Same (wrapper, view.SuperView);
    }

    [Fact]
    public void CanBeRegisteredAndShown ()
    {
        var view = new View
        {
            X = Pos.Center (),
            Y = Pos.Center (),
            Width = 20,
            Height = 10
        };

        var wrapper = new PopoverWrapper<View> { WrappedView = view };
        var popoverManager = new ApplicationPopover ();

        popoverManager.Register (wrapper);
        Assert.Contains (wrapper, popoverManager.Popovers);

        popoverManager.Show (wrapper);
        Assert.Equal (wrapper, popoverManager.GetActivePopover ());
        Assert.True (wrapper.Visible);
    }

    [Fact]
    public void QuitCommand_HidesPopover ()
    {
        var view = new View ();
        var wrapper = new PopoverWrapper<View> { WrappedView = view };
        var popoverManager = new ApplicationPopover ();

        popoverManager.Register (wrapper);
        popoverManager.Show (wrapper);

        Assert.True (wrapper.Visible);

        wrapper.InvokeCommand (Command.Quit);

        Assert.False (wrapper.Visible);
    }

    [Fact]
    public void AsPopover_Extension_CreatesWrapper ()
    {
        var view = new View { Id = "testView" };

        PopoverWrapper<View> wrapper = view.AsPopover ();

        Assert.NotNull (wrapper);
        Assert.Same (view, wrapper.WrappedView);
    }

    [Fact]
    public void AsPopover_Extension_ThrowsIfViewIsNull ()
    {
        View? view = null;

        Assert.Throws<ArgumentNullException> (() => view!.AsPopover ());
    }

    [Fact]
    public void WrappedView_ReceivesInput ()
    {
        var textField = new TextField { Width = 20 };
        var wrapper = new PopoverWrapper<TextField> { WrappedView = textField };

        wrapper.BeginInit ();
        wrapper.EndInit ();

        var popoverManager = new ApplicationPopover ();
        popoverManager.Register (wrapper);
        popoverManager.Show (wrapper);

        Assert.True (wrapper.Visible);
        Assert.Contains (textField, wrapper.SubViews);
    }

    [Fact]
    public void Multiple_Types_CanBeWrapped ()
    {
        var label = new Label { Text = "Test" };
        var labelWrapper = new PopoverWrapper<Label> { WrappedView = label };

        var button = new Button { Title = "Click" };
        var buttonWrapper = new PopoverWrapper<Button> { WrappedView = button };

        var listView = new ListView ();
        var listViewWrapper = new PopoverWrapper<ListView> { WrappedView = listView };

        Assert.Same (label, labelWrapper.WrappedView);
        Assert.Same (button, buttonWrapper.WrappedView);
        Assert.Same (listView, listViewWrapper.WrappedView);
    }

    [Fact]
    public void Current_Property_CanBeSetAndGet ()
    {
        var view = new View ();
        var wrapper = new PopoverWrapper<View> { WrappedView = view };
        var runnable = new Runnable ();

        wrapper.Current = runnable;

        Assert.Same (runnable, wrapper.Current);
    }

    [Fact]
    public void Disposed_Wrapper_DisposesWrappedView ()
    {
        var view = new View ();
        var wrapper = new PopoverWrapper<View> { WrappedView = view };

        wrapper.BeginInit ();
        wrapper.EndInit ();

        bool viewDisposed = false;
        view.Disposing += (s, e) => viewDisposed = true;

        wrapper.Dispose ();

        Assert.True (viewDisposed);
    }
}
