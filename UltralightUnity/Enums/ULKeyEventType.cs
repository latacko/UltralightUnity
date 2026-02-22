using System;

namespace UltralightUnity.Enums;

public enum ULKeyEventType: byte
{
    /// <summary>
    /// Key-Down event type. This type does **not** trigger accelerator commands in WebCore (eg, 
    /// Ctrl+C for copy is an accelerator command).
    ///
    /// @warning  You should probably use kKeyEventType_RawKeyDown instead. This type is only here for
    ///           historic compatibility with WebCore's key event types.
    /// </summary>
    [Obsolete]
    KeyDown,

    /// <summary>
    /// Key-Up event type. Use this when a physical key is released.
    /// </summary>
    KeyUp,

    /// <summary>
    /// Raw Key-Down type. Use this when a physical key is pressed.
    /// </summary>
    RawKeyDown,

    /// <summary>
    /// Character input event type. Use this when the OS generates text from
    /// a physical key being pressed (eg, WM_CHAR on Windows).
    /// </summary>
    Char,
}