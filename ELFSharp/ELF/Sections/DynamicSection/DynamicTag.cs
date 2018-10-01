namespace ELFSharp.ELF.Sections
{
    /// <summary>
    /// This enum holds some of the possible values for the DynamicTag value (dropping platform
    /// specific contents, such as MIPS flags.)
    /// 
    /// Values are coming from LLVM's elf.h headers.
    /// 
    /// File can be found in LLVM 3.8.1 source at:
    /// ../include/llvm/support/elf.h
    /// 
    /// License of the original C code is LLVM license.
    /// </summary>
    public enum DynamicTag : ulong
    {  
        DT_NULL = 0,          // Marks end of dynamic array.
        DT_NEEDED = 1,        // String table offset of needed library.
        DT_PLTRELSZ = 2,      // Size of relocation entries in PLT.
        DT_PLTGOT = 3,        // Address associated with linkage table.
        DT_HASH = 4,          // Address of symbolic hash table.
        DT_STRTAB = 5,        // Address of dynamic string table.
        DT_SYMTAB = 6,        // Address of dynamic symbol table.
        DT_RELA = 7,          // Address of relocation table (Rela entries).
        DT_RELASZ = 8,        // Size of Rela relocation table.
        DT_RELAENT = 9,       // Size of a Rela relocation entry.
        DT_STRSZ = 10,        // Total size of the string table.
        DT_SYMENT = 11,       // Size of a symbol table entry.
        DT_INIT = 12,         // Address of initialization function.
        DT_FINI = 13,         // Address of termination function.
        DT_SONAME = 14,       // String table offset of a shared objects name.
        DT_RPATH = 15,        // String table offset of library search path.
        DT_SYMBOLIC = 16,     // Changes symbol resolution algorithm.
        DT_REL = 17,          // Address of relocation table (Rel entries).
        DT_RELSZ = 18,        // Size of Rel relocation table.
        DT_RELENT = 19,       // Size of a Rel relocation entry.
        DT_PLTREL = 20,       // Type of relocation entry used for linking.
        DT_DEBUG = 21,        // Reserved for debugger.
        DT_TEXTREL = 22,      // Relocations exist for non-writable segments.
        DT_JMPREL = 23,       // Address of relocations associated with PLT.
        DT_BIND_NOW = 24,     // Process all relocations before execution.
        DT_INIT_ARRAY = 25,   // Pointer to array of initialization functions.
        DT_FINI_ARRAY = 26,   // Pointer to array of termination functions.
        DT_INIT_ARRAYSZ = 27, // Size of DT_INIT_ARRAY.
        DT_FINI_ARRAYSZ = 28, // Size of DT_FINI_ARRAY.
        DT_RUNPATH = 29,      // String table offset of lib search path.
        DT_FLAGS = 30,        // Flags.
        DT_ENCODING = 32,     // Values from here to DT_LOOS follow the rules
                            // for the interpretation of the d_un union.

        DT_PREINIT_ARRAY = 32,   // Pointer to array of preinit functions.
        DT_PREINIT_ARRAYSZ = 33, // Size of the DT_PREINIT_ARRAY array.
        DT_LOOS = 0x60000000,   // Start of environment specific tags.
        DT_HIOS = 0x6FFFFFFF,   // End of environment specific tags.
        DT_LOPROC = 0x70000000, // Start of processor specific tags.
        DT_HIPROC = 0x7FFFFFFF, // End of processor specific tags.
        DT_GNU_HASH = 0x6FFFFEF5, // Reference to the GNU hash table.
        DT_TLSDESC_PLT = 0x6FFFFEF6, // Location of PLT entry for TLS descriptor resolver calls.
        DT_TLSDESC_GOT = 0x6FFFFEF7, // Location of GOT entry used by TLS descriptor resolver PLT entry.
        DT_RELACOUNT = 0x6FFFFFF9,   // ELF32_Rela count.
        DT_RELCOUNT = 0x6FFFFFFA,    // ELF32_Rel count.
        DT_FLAGS_1 = 0X6FFFFFFB,    // Flags_1.
        DT_VERSYM = 0x6FFFFFF0,     // The address of .gnu.version section.
        DT_VERDEF = 0X6FFFFFFC,     // The address of the version definition table.
        DT_VERDEFNUM = 0X6FFFFFFD,  // The number of entries in DT_VERDEF.
        DT_VERNEED = 0X6FFFFFFE,    // The address of the version Dependency table.
        DT_VERNEEDNUM = 0X6FFFFFFF // The number of entries in DT_VERNEED.
    }
}