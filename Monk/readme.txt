1. Place the Slimes Arena Disc folder inside your Shaperbot/Rotations folder.
2. Open up Keybinds.cfg file in the Slimes Arena Disc/Settings folder and set your hotkeys for those spells.
	--for example, if you have Power Word: Shield on Shift-1 in WoW, you would put Power Word: Shield=s1 
	--you cannot use F1-F12 keys or numpad keys in game, those need to be reserved for Shaperbot to target/assist and other macros.
	--keys you can use are any of the alphanumeric keys with or without shift, ctrl, alt.  Z would be Z, ctrl-Z would be cZ, alt-Z would be aZ etc...
3. Open up Settings.cfg file in the Slimes Arena Disc/Settings folder and set your desired settings.
4. Save everything and then make sure your Shaperbot's config file (config.cfg inside your Shaperbot main directory) is configured to load the rotation.
	--Rotation=Slimes Arena Disc
5. Make sure your WoW and Blizzard app are closed and start the bot by running the .exe file.
6. Rotation should be good to go!

In game:
The rotation does not auto target for you, so you will need to target who you want to heal.
The rotation does DPS by casting damage spells on your focus target.  Make sure you can set/swap your focus target quickly in game!

You can also create some macros to pause Shaper Bot and manually cast a spell.
For example, this macro will pause Shaper Bot for 1.5 seconds and manually cast Power Word: Barrier at your mouse cursor:

/shaperwait 1.5
/cast [@cursor] Power Word: Barrier