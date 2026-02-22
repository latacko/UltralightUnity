using UltralightUnity;
using UltralightUnity.Enums;

public static unsafe class MouseManager
{
    public static ULMouseButton whichButtonIsPressed { get; private set; } = ULMouseButton.None;
    public static ULView view;
    public static void ReadEvent(MouseEvent* ev)
    {
        switch (ev->type)
        {
            case 1:
                view.FireMouseEvent(new(ULMouseEventType.MouseMoved, ev->x, ev->y, whichButtonIsPressed));
                break;
            case 2:
                RegisterPress(ULMouseButton.Left, ev);
                break;
            case 3:
                RegisterRelease(ULMouseButton.Left, ev);
                break;
            case 4:
                RegisterPress(ULMouseButton.Right, ev);
                break;
            case 5:
                RegisterRelease(ULMouseButton.Right, ev);
                break;
            case 6:
                RegisterPress(ULMouseButton.Middle, ev);
                break;
            case 7:
                RegisterRelease(ULMouseButton.Middle, ev);
                break;
            case 8:
                view.FireScrollEvent(new(UltralightUnity.Enums.ULScrollEventType.ScrollByPixel, ev->x, ev->y));
                break;
        }
    }

    static void RegisterPress(ULMouseButton whichButton, MouseEvent* ev)
    {
        view.FireMouseEvent(new(UltralightUnity.Enums.ULMouseEventType.MouseDown, ev->x, ev->y, whichButton));
        if (whichButtonIsPressed == ULMouseButton.None)
        {
            whichButtonIsPressed = whichButton;
        }
    }

    static void RegisterRelease(ULMouseButton whichButton, MouseEvent* ev)
    {
        view.FireMouseEvent(new(UltralightUnity.Enums.ULMouseEventType.MouseUp, ev->x, ev->y, whichButton));
        if (whichButtonIsPressed == whichButton)
        {
            whichButtonIsPressed = ULMouseButton.None;
        }
    }
}