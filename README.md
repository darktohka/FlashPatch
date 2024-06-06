# FlashPatch!

[![Patreon](https://img.shields.io/badge/Kofi-donate-purple.svg)](https://ko-fi.com/disyer) [![MIT license](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/darktohka/FlashPatch/blob/master/LICENSE)

![Image of FlashPatch](https://i.imgur.com/0yfudr1.png)

[Download latest version](https://github.com/darktohka/FlashPatch/releases/latest)

**[Check out Clean Flash Player, the brand new distribution of Flash Player that keeps Flash Player alive in the browser!](https://gitlab.com/CleanFlash/installer) Clean Flash is built using the proven FlashPatch utility, and is the de-facto best and user-friendliest way to install Flash Player on Windows to date!**

## What's this?

FlashPatch! is a tool that modifies the Flash Player installation on your computer, making it possible to play games in the browser again.

It bypasses the January 12th, 2021 killswitch that prevents you from playing any Flash Player game or animation after January 12th.

## Compatibility

| Browser           | Plugin API       | Version    | Windows                  | Linux                    | Mac                      | 64-bit             | 32-bit                   |
| ----------------- | ---------------- | ---------- | ------------------------ | ------------------------ | ------------------------ | ------------------ | ------------------------ |
| Google Chrome     | PPAPI (Pepper)   | 32.0.0.465 | :heavy_check_mark:       | :heavy_check_mark:       | :heavy_check_mark:       | :heavy_check_mark: | :x:                      |
| Mozilla Firefox   | NPAPI (Netscape) | 32.0.0.465 | :heavy_check_mark:       | :heavy_check_mark:       | :heavy_check_mark:       | :heavy_check_mark: | :heavy_check_mark:       |
| Safari            | NPAPI (Netscape) | 32.0.0.465 | :heavy_multiplication_x: | :heavy_multiplication_x: | :heavy_check_mark:       | :heavy_check_mark: | :heavy_multiplication_x: |
| Internet Explorer | ActiveX (OCX)    | 32.0.0.465 | :heavy_check_mark:       | :heavy_multiplication_x: | :heavy_multiplication_x: | :heavy_check_mark: | :heavy_check_mark:       |

✔️ means that the plugin is officially supported.

✖️ means that the plugin is unavailable on the platform (for example, Safari plugins are unavailable on Windows).

❌ means that a specific patch does not exist for that plugin on that platform. These plugins may still be patched using the `Patch File...` button.

**A generic patch is also available that is version independent. To try this patch, use the `Patch File...` button and manually choose the Flash binary you wish to patch.**

## China-specific Flash

| Browser           | Plugin API       | Version    | Windows                  | Linux                    | Mac                      | 64-bit             | 32-bit                   |
| ----------------- | ---------------- | ---------- | ------------------------ | ------------------------ | ------------------------ | ------------------ | ------------------------ |
| Google Chrome     | PPAPI (Pepper)   | 34.0.0.315 | :heavy_check_mark:       | :heavy_check_mark:       | :heavy_check_mark:       | :heavy_check_mark: | :heavy_check_mark:       |
| Mozilla Firefox   | NPAPI (Netscape) | 34.0.0.315 | :heavy_check_mark:       | :heavy_check_mark:       | :heavy_check_mark:       | :heavy_check_mark: | :heavy_check_mark:       |
| Safari            | NPAPI (Netscape) | 34.0.0.308 | :heavy_multiplication_x: | :heavy_multiplication_x: | :heavy_check_mark:       | :heavy_check_mark: | :heavy_multiplication_x: |
| Internet Explorer | ActiveX (OCX)    | 34.0.0.315 | :heavy_check_mark:       | :heavy_multiplication_x: | :heavy_multiplication_x: | :heavy_check_mark: | :heavy_check_mark:       |

Old Chinese Flash versions supported:

- Version 34.0.0.301
- Version 34.0.0.289
- Version 34.0.0.282
- Version 34.0.0.277
- Version 34.0.0.267
- Version 34.0.0.251
- Version 34.0.0.242
- Version 34.0.0.231
- Version 34.0.0.211
- Version 34.0.0.209
- Version 34.0.0.201
- Version 34.0.0.192
- Version 34.0.0.184
- Version 34.0.0.175
- Version 34.0.0.164
- Version 34.0.0.155
- Version 34.0.0.137
- Version 34.0.0.118

While Adobe has completely stopped updating the global version of Adobe Flash Player, they are still maintaining a special version of Adobe Flash for Mainland China only. This version is completely compatible with the global version of Flash, but contains a non-closable process, known as the "Flash Helper Service", that collects private information and pops up advertisement window contents.

This version of Flash normally only works within Mainland China. Furthermore, in order to let business users to purchase a China-specific enterprise edition of Adobe Flash, if it detects an enterprise environment (an Active Directory environment) it refuses to start. It also contains a dormant killswitch.

FlashPatch fully supports the latest version of the Chinese Flash Player browser plugin: 34.0.0.315.

FlashPatch provides the following patches for this version:

- Patch Chinese region lock on runtime (geo2.adobe.com)
- Remove dependence on adware "Flash Helper Service"
- Deactivate dormant OOD Macromedia XML killswitch
- Patch Chinese Enterprise phone-home service
- Patch activation check

Download clean builds of Flash 34.0.0.315:

- [For Windows - Chrome/Firefox/IE](https://github.com/darktohka/clean-flash-builds/releases/tag/v1.40)
- [For Linux - Chrome/Firefox](https://github.com/darktohka/clean-flash-builds/releases/tag/v1.7)
- [For Mac - Chrome/Firefox](https://github.com/darktohka/clean-flash-builds/releases/tag/v1.38)

## Flash Player

**But I want to play games using the standalone Flash Player!**

Adobe ships a current version of the Flash Player projector without the killswitch built-in [on their debug downloads page](https://adobe.com/support/flashplayer/debug_downloads.html).

**I do not have Adobe Flash Player installed, where can I get it?**

You can download Adobe Flash Player 32.0.0.465 from [this archived link](https://web.archive.org/web/20210112063313/http://fpdownload.adobe.com/get/flashplayer/pdc/32.0.0.465/install_flash_player.exe). You might need to turn back your system clock to January 11th, 2021 or earlier to install it.

## Linux

**I would like to use Flash Player on Linux!**

Unfortunately, the tool only works on Windows right now, but if you have a Windows machine, feel free to manually patch version 32.0.0.465 of `libpepflashplayer.so` or `libflashplayer.so`. You can then use these binaries on your Linux machine.

Notice: Starting with Adobe Flash Player version 34, video playing function on Linux has dropped. If you need this feature please use version 32.0.0.465 instead.

## Windows XP

**I need to use Flash Player on Windows XP!**

FlashPatch is not compatible with Windows XP. FlashPatch relies on .NET Framework 4.5, but the last supported version of .NET Framework on Windows XP is [4.0.3](https://docs.microsoft.com/en-us/dotnet/framework/install/on-windows-xp#net-framework-403). Regardless, there is a way to keep using Flash on Windows XP. [Please check out the following instructions on GitHub.](https://github.com/darktohka/FlashPatch/issues/7#issuecomment-785096536)

Notice: Starting with Adobe Flash Player version 34, video playing function on Windows XP has dropped. If you need this feature please use version 32.0.0.465 instead.

## Usage

- Extract `FlashPatch.exe` into a new folder.
- Run `FlashPatch.exe` and allow it to run as administrator (necessary to change system Flash Player files)
- Press the "Patch" button and agree to the warning message
- Your Flash Player is now usable once again!

To restore the old, unpatched binaries:

- The unpatched binaries are saved into a `Backup` folder when you patch Flash Player
- Run `FlashPatch.exe` and allow it to run as administrator (necessary to change system Flash Player files)
- Press the "Restore" button and agree
- Your Flash Player is now back to its factory defaults.
