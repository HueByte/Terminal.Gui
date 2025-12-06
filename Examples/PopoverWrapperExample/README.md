# PopoverWrapper Example

This example demonstrates how to use `PopoverWrapper<TView>` to make any View into a popover without implementing the `IPopover` interface.

## Overview

`PopoverWrapper<TView>` is similar to `RunnableWrapper<TView, TResult>` but for popovers instead of runnables. It wraps any View and automatically handles:

- Setting proper viewport settings (transparent, transparent mouse)
- Configuring focus behavior
- Handling the quit command to hide the popover
- Sizing to fill the screen by default

## Key Features

- **Fluent API**: Use `.AsPopover()` extension method for a clean, fluent syntax
- **Any View**: Wrap any existing View - Button, ListView, custom Views, forms, etc.
- **Automatic Management**: The wrapper handles all the popover boilerplate
- **Type-Safe**: Generic type parameter ensures type safety when accessing the wrapped view

## Usage

### Basic Usage

```csharp
// Create any view
var myView = new View
{
    X = Pos.Center (),
    Y = Pos.Center (),
    Width = 40,
    Height = 10,
    BorderStyle = LineStyle.Single
};

// Wrap it as a popover
PopoverWrapper<View> popover = myView.AsPopover ();

// Register and show
app.Popover.Register (popover);
app.Popover.Show (popover);
```

### With ListView

```csharp
var listView = new ListView
{
    Title = "Select an Item",
    X = Pos.Center (),
    Y = Pos.Center (),
    Width = 30,
    Height = 10,
    Source = new ListWrapper<string> (["Apple", "Banana", "Cherry"])
};

PopoverWrapper<ListView> popover = listView.AsPopover ();
app.Popover.Register (popover);
app.Popover.Show (popover);
```

### With Custom Forms

```csharp
View CreateFormView ()
{
    var form = new View
    {
        Title = "User Form",
        X = Pos.Center (),
        Y = Pos.Center (),
        Width = 60,
        Height = 16
    };
    
    // Add form fields...
    
    return form;
}

View formView = CreateFormView ();
PopoverWrapper<View> popover = formView.AsPopover ();
app.Popover.Register (popover);
app.Popover.Show (popover);
```

## Comparison with RunnableWrapper

| Feature | RunnableWrapper | PopoverWrapper |
|---------|----------------|----------------|
| Purpose | Make any View runnable as a modal session | Make any View into a popover |
| Blocking | Yes, blocks until stopped | No, non-blocking overlay |
| Result Extraction | Yes, via typed Result property | N/A (access WrappedView directly) |
| Dismissal | Via RequestStop() or Quit command | Via Quit command or clicking outside |
| Focus | Takes exclusive focus | Shares focus with underlying content |

## Running the Example

```bash
dotnet run --project Examples/PopoverWrapperExample
```

## See Also

- [Popovers Deep Dive](../../docfx/docs/Popovers.md)
- [RunnableWrapper Example](../RunnableWrapperExample/)
- `Terminal.Gui.App.PopoverBaseImpl`
- `Terminal.Gui.App.IPopover`
