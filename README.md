# Fork of ProximityVoiceChat

This Fork attempts to coordinate matching of exploratory content instances

You will need to build your own version to use

# Current State

## Test Buttons

### Test Open
Sends command "/search" to game which opens the Adventurer List when in Field Op Content
Uses ECommmons Library from NightmareXIV for its simple command handler

### Test Read
Reads ContentMemberList Array data. Unfortunately this array is not cleared automatically by the game, and is just written over when it is next loaded by UI elements
This means that going from an instance of 30 players to one of 20 would result in the last 10 players from the first instance remaining in this array upon reading it in the new instance

### Test Clear
The Most Scuffed, probably dangerous, probably horrible workaround to the above problem: Just wipe the array
in theory wiping the array right before reloading it should be fine. But im not smart enough to know that for sure, but for now I decided to risk it to test other components of the plugin

## Signaling Server

Logic added to signaling server to handle instance matching. Very rudimentary and fairly untested. Just checks all voice rooms to find the one with the most players from the current adventurer list. If no players from the current adventurer list can be found, creates a new on as usual.

Ideally I would like to change this to work on some proportion of matching so that weird edge cases where whole groups leave and join another instance don't cause issues, but this will do for testing
