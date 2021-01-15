# FlashPatch!

[![Patreon](https://img.shields.io/badge/Kofi-donate-purple.svg)](https://ko-fi.com/disyer) [![MIT license](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/darktohka/FlashPatch/blob/master/LICENSE)

![Image of FlashPatch](https://i.imgur.com/0Sijhao.png)

[Download latest version](https://github.com/darktohka/FlashPatch/releases/latest)

## What's this?

FlashPatch! is a tool that modifies the Flash Player installation on your computer, making it possible to play games in the browser again.

It bypasses the January 12st, 2021 killswitch that prevents you from playing any Flash Player game or animation after January 12st.

## Compatibility

| Browser           | Plugin API       | Version    | 64-bit supported?  | 32-bit supported?  | 64-bit tested?     | 32-bit tested?           |
| ----------------- | ---------------- | ---------- | ------------------ | ------------------ | ------------------ | ------------------------ |
| Google Chrome     | PPAPI (Pepper)   | 32.0.0.465 | :heavy_check_mark: | :x:                | :heavy_check_mark: | :heavy_multiplication_x: |
| Mozilla Firefox   | NPAPI (Netscape) | 32.0.0.465 | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark:       |
| Internet Explorer | ActiveX (OCX)    | 32.0.0.445 | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark:       |

## Flash Player

**But I want to play games using the standalone Flash Player!**

Adobe ships a current version of the Flash Player projector without the killswitch built-in [on their debug downloads page](https://adobe.com/support/flashplayer/debug_downloads.html).

**I do not have Adobe Flash Player installed, where can I get it?**

You can download Adobe Flash Player 32.0.0.465 from [this archived link](https://web.archive.org/web/20210112063313/http://fpdownload.adobe.com/get/flashplayer/pdc/32.0.0.465/install_flash_player.exe). You might need to turn back your system clock to January 11th, 2021 or earlier to install it.

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
