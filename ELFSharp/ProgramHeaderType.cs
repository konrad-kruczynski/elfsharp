using System;
namespace ELFSharp
{
	public enum ProgramHeaderType : uint
	{
		Null = 0,
		Load,
		Dynamic,
		Interpreter,
		Note,
		SharedLibrary,
		ProgramHeader		
	}
}

