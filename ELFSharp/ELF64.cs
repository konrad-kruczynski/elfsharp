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
	}
}
