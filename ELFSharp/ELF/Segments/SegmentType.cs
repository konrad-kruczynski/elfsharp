namespace ELFSharp.ELF.Segments
{
	public enum SegmentType : uint
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

