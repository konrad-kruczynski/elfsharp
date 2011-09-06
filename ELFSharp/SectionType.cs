namespace ELFSharp
{
    public enum SectionType : uint
    {
        Null = 0,
        ProgBits,
        SymbolTable,
        StringTable,
        RelocationAddends,
        HashTable,
        Dynamic,
        Note,
        NoBits,
        Relocation,
        Shlib,
        DynamicSymbolTable,
    }
}