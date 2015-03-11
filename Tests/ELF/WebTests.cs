using System;
using NUnit.Framework;
using System.Collections.Generic;
using ELFSharp.ELF.Sections;
using System.Linq;
using ELFSharp.ELF;

namespace Tests.ELF
{
	[TestFixture]
	public class WebTests
	{
		[Test]
		public void ListELFSectionHeaders()
		{
			var elf = ELFReader.Load(Utilities.GetBinaryLocation("hello64le"));
			var output = new List<string>();

			foreach(var header in elf.Sections)
			{
				output.Add(header.ToString());
			}
			var expectedOutput = @": Null, load @0x0, 0 bytes long
.interp: ProgBits, load @0x400200, 28 bytes long
.note.ABI-tag: Note, Type=1
.note.gnu.build-id: Note, Type=3
.gnu.hash: 1879048182, load @0x400260, 28 bytes long
.dynsym: DynamicSymbolTable, load @0x400280, 96 bytes long
.dynstr: StringTable, load @0x4002E0, 61 bytes long
.gnu.version: 1879048191, load @0x40031E, 8 bytes long
.gnu.version_r: 1879048190, load @0x400328, 32 bytes long
.rela.dyn: RelocationAddends, load @0x400348, 24 bytes long
.rela.plt: RelocationAddends, load @0x400360, 72 bytes long
.init: ProgBits, load @0x4003A8, 26 bytes long
.plt: ProgBits, load @0x4003D0, 64 bytes long
.text: ProgBits, load @0x400410, 420 bytes long
.fini: ProgBits, load @0x4005B4, 9 bytes long
.rodata: ProgBits, load @0x4005C0, 13 bytes long
.eh_frame_hdr: ProgBits, load @0x4005D0, 44 bytes long
.eh_frame: ProgBits, load @0x400600, 172 bytes long
.init_array: 14, load @0x6006B0, 8 bytes long
.fini_array: 15, load @0x6006B8, 8 bytes long
.jcr: ProgBits, load @0x6006C0, 8 bytes long
.dynamic: Dynamic, load @0x6006C8, 464 bytes long
.got: ProgBits, load @0x600898, 8 bytes long
.got.plt: ProgBits, load @0x6008A0, 48 bytes long
.data: ProgBits, load @0x6008D0, 16 bytes long
.bss: NoBits, load @0x6008E0, 8 bytes long
.comment: ProgBits, load @0x0, 56 bytes long
.shstrtab: StringTable, load @0x0, 264 bytes long
.symtab: SymbolTable, load @0x0, 1536 bytes long
.strtab: StringTable, load @0x0, 566 bytes long";
			var expectedOutputAsLines = expectedOutput.Split(new [] { "\n", "\r\n" }, StringSplitOptions.None);
			CollectionAssert.AreEqual(expectedOutputAsLines, output);
		}

		[Test]
		public void GetNamesOfFunctionSymbols()
		{
			var elf = ELFReader.Load(Utilities.GetBinaryLocation("hello64le"));
			var output = new List<string>();

			var functions = ((ISymbolTable)elf.GetSection(".symtab")).Entries.Where(x => x.Type == SymbolType.Function);
			foreach(var f in functions)
			{
				output.Add(f.Name);
			}

			var expectedOutput = @"deregister_tm_clones
register_tm_clones
__do_global_dtors_aux
frame_dummy
__libc_csu_fini
puts@@GLIBC_2.2.5
_fini
__libc_start_main@@GLIBC_2.2.5
__libc_csu_init
_start
main
_init";
			var expectedOutputAsLines = expectedOutput.Split(new [] { "\n", "\r\n" }, StringSplitOptions.None);
			CollectionAssert.AreEqual(expectedOutputAsLines, output);
		}

		[Test]
		public void WriteAllLoadableProgBitsToArray()
		{
			var elf = ELFReader.Load(Utilities.GetBinaryLocation("hello64le"));

			var sectionsToLoad = elf.GetSections<ProgBitsSection<long>>()
				.Where(x => x.LoadAddress != 0);
			foreach (var s in sectionsToLoad)
			{
				var array = new byte[s.Size];
				s.GetContents().CopyTo(array, 0);
			}
		}
	}
}

