# ELFSharp Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.17.2] - 2023-06-09
### Fixed
- MachO: fix for reading LC_UUID command.

## [2.17.1] - 2023-04-25
### Fixed
- ELF: fix for `SHN_LORESERVE` edge case. 

## [2.17] - 2023-04-03
### Added
- MachO: Added a command to parse LC_UUID and ability to get the UUID.

## [2.16.1] - 2023-01-11
### Fixed
- ELF: `ELFReader.TryLoad` now closes the underlying stream if it owns it and loading fails.

## [2.16] - 2022-12-04
### Added
- Support for multi-file uImage binaries.

## [2.15.2] - 2022-11-14
### Fixed
- ELF: case when section number is larger than `SHN_LORESERVE` and string table is present is now handled properly.

## [2.15.1] - 2022-11-12
### Fixed
- ELF: case when section number is larger than `SHN_LORESERVE` is now handled properly.

## [2.15] - 2022-04-11
### Added
- ELF: multiple note data are now supported in `NoteSegment`.

## [2.14] - 2022-04-08
### Added
- MachO: Symbol now knows to which section it belongs.
- MachO: Added constraint to GetCommandsOfType.

### Fixed
- Parameter shouldOwnStream was incorrectly implemented (as its opposite, actually).

## [2.13.3] - 2022-02-17
### Maintenance
- License keyword is now included in the package.

## [2.13.2] - 2021-11-10
### Fixed
- NOBITS ELF file section is now correctly read.

## [2.13.1] - 2021-11-02
### Fixed
- 64-bit big endian Mach-O binaries were treated as 32-bit. This is now fixed.

## [2.13] - 2021-07-20
### Added
- Mach-O file header flags.

## [2.12.1] - 2021-06-12
### Fixed
- InvalidOperationException when parsing intermediate object file.

## [2.12] - 2020-12-18
### Added
- Support for MachO commands:
  + LC_ID_DYLIB
  + LC_LOAD_DYLIB
  + LC_LOAD_WEAK_DYLIB
  + LC_REEXPORT_DYLIB
