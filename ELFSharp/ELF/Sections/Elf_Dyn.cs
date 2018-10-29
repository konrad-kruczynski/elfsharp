using System;
using System.IO;

namespace ELFSharp.ELF.Sections
{
    /// <summary>
    /// Dynamic table entries are made up of a 32 bit or 64 bit "tag"
    /// and a 32 bit or 64 bit union (val/pointer in 64 bit, val/pointer/offset in 32 bit).
    /// 
    /// See LLVM elf.h file for the C/C++ version.
    /// </summary>
    internal class ELF_Dyn : IELF_Dyn
    {
        private readonly ulong tagValue;
        private readonly ulong unionValue;

        public ELF_Dyn(ulong tagValue, ulong unionValue)
        {
            this.tagValue = tagValue;
            this.unionValue = unionValue;
        }

        public DynamicTag Tag 
        {
            get 
            {
                return (DynamicTag) tagValue;
            }
        }

        public ulong Union {
            get 
            {
                return (ulong) unionValue;
            }
        }

        public override string ToString()
        {
            string unionStr = unionValue.ToString("x16");
            return $"{Tag} \t 0x{unionStr}";
        }
    }
}