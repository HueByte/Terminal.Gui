namespace Terminal.Gui.App;

/// <summary>
///     Extension methods for making any <see cref="View"/> into a popover.
/// </summary>
/// <remarks>
///     These extensions provide a fluent API for wrapping views in <see cref="PopoverWrapper{TView}"/>,
///     enabling any View to be shown as a popover without implementing <see cref="IPopover"/>.
/// </remarks>
public static class ViewPopoverExtensions
{
    /// <summary>
    ///     Converts any View into a popover.
    /// </summary>
    /// <typeparam name="TView">The type of view to make into a popover.</typeparam>
    /// <param name="view">The view to wrap. Cannot be null.</param>
    /// <returns>A <see cref="PopoverWrapper{TView}"/> that wraps the view.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="view"/> is null.</exception>
    /// <remarks>
    ///     <para>
    ///         This method wraps the view in a <see cref="PopoverWrapper{TView}"/> which automatically
    ///         handles popover behavior including transparency, focus, and quit key handling.
    ///     </para>
    ///     <para>
    ///         After creating the wrapper, register it with <see cref="ApplicationPopover.Register"/>
    ///         and show it with <see cref="ApplicationPopover.Show"/>.
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    /// // Make a custom view into a popover
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
    /// var popover = myView.AsPopover();
    /// app.Popover.Register (popover);
    /// app.Popover.Show (popover);
    /// 
    /// // The wrapped view can still be accessed
    /// Console.WriteLine ($"View id: {popover.WrappedView.Id}");
    /// </code>
    /// </example>
    public static PopoverWrapper<TView> AsPopover<TView> (this TView view)
        where TView : View
    {
        if (view is null)
        {
            throw new ArgumentNullException (nameof (view));
        }

        return new PopoverWrapper<TView> { WrappedView = view };
    }
}
