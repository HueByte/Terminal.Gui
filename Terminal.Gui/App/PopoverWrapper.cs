namespace Terminal.Gui.App;

/// <summary>
///     Wraps any <see cref="View"/> to make it a popover, similar to how
///     <see cref="RunnableWrapper{TView, TResult}"/> wraps views to make them runnable.
/// </summary>
/// <typeparam name="TView">The type of view being wrapped.</typeparam>
/// <remarks>
///     <para>
///         This class enables any View to be shown as a popover with
///         <see cref="ApplicationPopover.Show"/>
///         without requiring the View to implement <see cref="IPopover"/> or derive from
///         <see cref="PopoverBaseImpl"/>.
///     </para>
///     <para>
///         The wrapper automatically handles:
///         <list type="bullet">
///             <item>Setting proper viewport settings (transparent, transparent mouse)</item>
///             <item>Configuring focus behavior</item>
///             <item>Handling the quit command to hide the popover</item>
///             <item>Sizing to fill the screen by default</item>
///         </list>
///     </para>
///     <para>
///         Use <see cref="ViewPopoverExtensions.AsPopover{TView}"/> for a fluent API approach.
///     </para>
///     <example>
///         <code>
/// // Wrap a custom view to make it a popover
/// var myView = new View
/// {
///     X = Pos.Center (),
///     Y = Pos.Center (),
///     Width = 40,
///     Height = 10,
///     BorderStyle = LineStyle.Single
/// };
/// myView.Add (new Label { Text = "Hello Popover!" });
/// 
/// var popover = new PopoverWrapper&lt;View&gt; { WrappedView = myView };
/// app.Popover.Register (popover);
/// app.Popover.Show (popover);
/// </code>
///     </example>
/// </remarks>
public class PopoverWrapper<TView> : PopoverBaseImpl where TView : View
{
    /// <summary>
    ///     Initializes a new instance of <see cref="PopoverWrapper{TView}"/>.
    /// </summary>
    public PopoverWrapper ()
    {
        Id = "popoverWrapper";
        Width = Dim.Auto ();
        Height = Dim.Auto ();
    }

    private TView? _wrappedView;

    /// <summary>
    ///     Gets or sets the wrapped view that is being made into a popover.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This property must be set before the wrapper is initialized.
    ///         Access this property to interact with the original view or configure its behavior.
    ///     </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown if the property is set after initialization.</exception>
    public required TView WrappedView
    {
        get => _wrappedView ?? throw new InvalidOperationException ("WrappedView must be set before use.");
        init
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException ("WrappedView cannot be changed after initialization.");
            }

            _wrappedView = value;
        }
    }

    /// <inheritdoc/>
    public override void EndInit ()
    {
        base.EndInit ();

        // Add the wrapped view as a subview after initialization
        if (_wrappedView is { })
        {
            Add (_wrappedView);
        }
    }
}
