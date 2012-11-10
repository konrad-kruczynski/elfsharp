namespace ELFSharp
{
	public enum Machine : ushort
	{
		None = 0,
		M32,
		SPARC,
		Intel80386,
		M68K,
		M88K,
		Intel80860 = 7,
		MIPS,
		S370,
		MIPS_RS3000,
		PARISC = 15,
		VPP500 = 17,
		SPARC32Plus,
		Intel80960,
		PowerPC,
		PowerPC64,
		ARM = 0x28
	}
}
