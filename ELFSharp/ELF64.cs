using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ELFSharp
{
	public class ELF64 : ELF
	{
		internal ELF64(string fileName) : base(fileName)
		{
			
		}
		
		public UInt64 EntryPoint
		{
			get
			{
				return EntryPointLong;
			}
		}
		
		public UInt64 MachineFlags
		{
			get
			{
				return MachineFlagsLong;
			}
		}
		
		protected override void CheckClass ()
		{
			if(Class != Class.Bit64)
			{
				throw new ArgumentException("Given ELF file is not 64 bit as you assumed.");
			}
		}
	}
}
