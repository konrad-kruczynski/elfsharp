using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ELFSharp
{
    public class ELF32 : ELF
    {
        internal ELF32(string fileName) : base(fileName)
        {
            
        }
		
        public UInt32 EntryPoint 
		{ 
			get
			{
				return unchecked((uint) EntryPointLong);
			}
		}
		
        public UInt32 MachineFlags
		{
			get
			{
				return unchecked((uint) MachineFlagsLong);
			}
		}
    }
	

}
