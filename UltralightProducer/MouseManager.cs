using UltralightUnity;
using UltralightUnity.Enums;

public static unsafe class MouseManager
{
    public static ULMouseButton whichButtonIsPressed { get; private set; } = ULMouseButton.None;
    public static void ReadEvent(MouseEvent* ev, ULView view)
    {
        switch (ev->type)
        {
            case 1:
                view.FireMouseEvent(new(ULMouseEventType.MouseMoved, ev->x, ev->y, whichButtonIsPressed));
                break;
            case 2:
                RegisterPress(ULMouseButton.Left, ev, view);
                break;
            case 3:
                RegisterRelease(ULMouseButton.Left, ev, view);
                break;
            case 4:
                RegisterPress(ULMouseButton.Right, ev, view);
                break;
            case 5:
                RegisterRelease(ULMouseButton.Right, ev, view);
                break;
            case 6:
                RegisterPress(ULMouseButton.Middle, ev, view);
                break;
            case 7:
                RegisterRelease(ULMouseButton.Middle, ev, view);
                break;
            case 8:
                view.FireScrollEvent(new(ULScrollEventType.ScrollByPixel, ev->x, ev->y));
                break;
        }
    }

    static void RegisterPress(ULMouseButton whichButton, MouseEvent* ev, ULView view)
    {
        view.FireMouseEvent(new(UltralightUnity.Enums.ULMouseEventType.MouseDown, ev->x, ev->y, whichButton));
        if (whichButtonIsPressed == ULMouseButton.None)
        {
            whichButtonIsPressed = whichButton;
        }
    }

    static void RegisterRelease(ULMouseButton whichButton, MouseEvent* ev, ULView view)
    {
        view.FireMouseEvent(new(UltralightUnity.Enums.ULMouseEventType.MouseUp, ev->x, ev->y, whichButton));
        if (whichButtonIsPressed == whichButton)
        {
            whichButtonIsPressed = ULMouseButton.None;
        }
    }
}