using System;
#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
#endif

namespace UltralightUnity.DataTypes;

public struct ULIntRect : IEquatable<ULIntRect>
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public readonly bool IsEmpty => (Left == Right) || (Top == Bottom);

    public readonly bool Equals(ULIntRect other) => Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
    
    public readonly override bool Equals(object? other) => other is ULIntRect rect ? Equals(rect) : false;
    public static bool operator ==(ULIntRect? left, ULIntRect? right) => left is not null ? (right is not null ? left.Equals(right) : false) : right is null;
    public static bool operator !=(ULIntRect? left, ULIntRect? right) => !(left == right);

    public readonly override int GetHashCode() => base.GetHashCode();

    public static explicit operator ULIntRect(ULRect rect) => new() 
    { 
        Left = (int)rect.Left, 
        Top = (int)rect.Top, 
        Right = (int)rect.Right, 
        Bottom = (int)rect.Bottom 
    };
}