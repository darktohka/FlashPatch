# FlashPatch!

[![Patreon](https://img.shields.io/badge/Kofi-donate-purple.svg)](ko-fi.com/disyer) [![MIT license](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/darktohka/FlashPatch/blob/master/LICENSE)

## What's this?

FlashPatch! is a tool that modifies the Flash Player installation on your computer, making it possible to play games in the browser again.

It bypasses the January 12nd, 2021 killswitch that prevents you from playing any Flash Player game or animation after January 12nd.

## Compatibility

| Browser           | Plugin API       | 64-bit supported?  | 32-bit supported?  | 64-bit tested?     | 32-bit tested?           |
|-------------------|------------------|--------------------|--------------------|--------------------|--------------------------|
| Google Chrome     | PPAPI (Pepper)   | :heavy_check_mark: | :x:                | :heavy_check_mark: | :heavy_multiplication_x: |
| Mozilla Firefox   | NPAPI (Netscape) | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark:       |
| Internet Explorer | ActiveX (OCX)    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :x:                      |

## Flash Player

**But I want to play games using the standalone Flash Player!**

Adobe ships a current version of the Flash Player projector without the killswitch built-in [on their debug downloads page](https://adobe.com/support/flashplayer/debug_downloads.html).

## Usage

- Extract `FlashPlayer.exe` into a new folder.
- Run `FlashPlayer.exe` and allow it to run as administrator (necessary to change system Flash Player files)
- Press the "Patch" button and agree to the warning message
- Your Flash Player is now usable once again!

To restore the old, unpatched binaries:
- The unpatched binaries are saved into a `Backup` folder when you patch Flash Player
- Run `FlashPlayer.exe` and allow it to run as administrator (necessary to change system Flash Player files)
- Press the "Restore" button and agree
- Your Flash Player is now back to its factory defaults.