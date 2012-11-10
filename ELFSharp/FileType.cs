namespace ELFSharp
{
	public enum FileType : ushort
	{
		None = 0,
		Relocatable,
		Executable,
		SharedObject,
		Core
	}
}