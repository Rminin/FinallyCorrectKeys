### [1.6.0]
- Reworked storing of Bindings to be more Enum like and refactored places they where they were used
- Optimized updating of the Interaction Action Displaystring which was performant heavy (but only when cursor tooltip was shown and contained the keybind). Implemented a time based cooldown on updating the displaystring.

### [1.5.0]
- Overworked how word wrap is applied to the control tips
- Added a config for choosing a word wrap mode
- Added a config for the word wrap limit
- Cleaned up Debug Log messages

### [1.4.0]
- Removed ShotgunItemPatches class because the ForceChangeText corountine changes the lines correctly
- Cleaned up Debug Log messages

### [1.3.0]
- Added config for disabling word wrapping
- Added option and config for hiding control tips entirely
- Added compatibility with LethalConfig

### [1.2.2]
- Disabled word wrapping on the control tips
- Updated README

### [1.2.1]
- Initial release

### [1.2.0]
- Integration of LobbyCompatibility

### [1.1.0]
- Fixed Shotgun control tips
- Adapted interaction keybinds
- Adapted furniture placing keybinds

### [1.0.0]
- Initial creation of repo (not released)
- Adapted basic item control tips 
