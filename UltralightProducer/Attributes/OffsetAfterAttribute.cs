[AttributeUsage(AttributeTargets.Field)]
public class OffsetAfterAttribute(string sizeField) : Attribute
{
    public string SizeField { get; } = sizeField;
}
