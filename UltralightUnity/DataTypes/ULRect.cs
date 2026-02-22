using System;

namespace UltralightUnity.DataTypes;

public struct ULRect : IEquatable<ULRect>
{
	public float Left;
	public float Top;
	public float Right;
	public float Bottom;

	public readonly bool IsEmpty => (Left == Right) || (Top == Bottom);

	public readonly bool Equals(ULRect other) => Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
	
    public readonly override bool Equals(object? other) => other is ULRect rect ? Equals(rect) : false;
	public static bool operator ==(ULRect? left, ULRect? right) => left is not null ? (right is not null ? left.Equals(right) : false) : right is null;
	public static bool operator !=(ULRect? left, ULRect? right) => !(left == right);

	public readonly override int GetHashCode() => base.GetHashCode();

	public static explicit operator ULRect(ULIntRect rect) => new() { Left = (float)rect.Left, Top = (float)rect.Top, Right = (float)rect.Right, Bottom = (float)rect.Bottom };
}