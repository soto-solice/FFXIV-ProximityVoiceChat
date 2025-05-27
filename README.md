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
The Most Scuffed, probably dangerous, probably horrible workaround to the above problem: Just wipe the array,
in theory wiping the array right before reloading it should be fine. But im not smart enough to know that for sure, but for now I decided to risk it to test other components of the plugin

## Another Way?
Browsing through the dalamud addon inspector I found the struct which displays the member list, perhaps with some more digging I might be able to find where the actual length of the *current* list of players is and only read that far into the ContentMemberList array, removing the need for clearing (and thus editing game memory)

## Signaling Server

Logic added to signaling server to handle instance matching. Very rudimentary and fairly untested. Just checks all voice rooms to find the one with the most players from the current adventurer list. If no players from the current adventurer list can be found, creates a new on as usual.

Ideally I would like to change this to work on some proportion of matching so that weird edge cases where whole groups leave and join another instance don't cause issues, but this will do for testing.

TODO: Test with more players. Unfortunately I didn't get around to adding some dummy tests for selecting the correct instance with more players.

TODO: Some way to close the playersearch window from the plugin (need to spend some more time figuring out UI interraction automation, so far every attempt has led to a crash)

TODO: Automation, so far haven't been able to schedule anything correctly such that on clicking join voice room the game: Clears ContentMemberList (where adv list is stored) -> Opens AdventurerList (or close->open OR refresh its contents if its already open) -> Reads AdventurerList once open (or loaded) -> Sends Join request -> Close AdventurerList.

Currently the way to test the signaling server functionality is to first close the adventurer list then:
- Click Test Clear (Required if Adventurer List/ContentMemberList has been populated with more data prior)
- Click Test Open
- Click Test Read
- Join Public Voice Room

To force seperate instances I have been manually appending "Materia" to a portion of the room name in the MapManager GetCurrentMapPublicRoomName() method. there is a commented out line for this to instead append the current datacenter of the player.

Currently there is a new method in MapManager called inFieldOp(), however it is currently only configured for Eureka (where I was testing). This would need cases added for each other Field Op/Exploratory Zone.
