# RandomisedReminder
A meditation tool that gives you random gentle reminders to return to your breathing.

## Function
Some audio cues are combined in an audio file of otherwise silence, possibly with an opening and closing sound.
The audio cues are randomly distributed in the time period possibly with some limitation on proximity to each other.
This is then played.
There are very few options:
- an overall duration
- how sparse the reminders should be. 3 levels

The play button takes you to a second scene that plays the audio and quites when it reaches the end.
Possibly with a button to quit.

## Structural details

The Canvas will hold the probably one Monobehaviour that prepares the audio.
3 UI elements:
An intfield for the overall duration
An enum field for reminder sparsity
Play button: it invokes the audio file preparation method. Once that succeeds it invokes scene 2