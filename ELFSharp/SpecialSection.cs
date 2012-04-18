namespace ELFSharp
{
    // TODO: this is not section - counterintuitive
    public enum SpecialSection : ushort
    {
        Absolute = 0,
        Common = 0xFFF1,
        Undefined = 0xFFF2
    }
}