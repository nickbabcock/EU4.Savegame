# EUIVSavegame

## What happens when a new patch is released

- Add a reprseentative savegame to the test suite
- Try to fix breaking changes such that backward compatibility is still preserved
- If compatbility canoot be saved, branch off old version
  - Fix only major bugs in old version
  - Main development continues with new version

## Mod compatibility

Mods are of a huge importance to the community and likewise in this project.
Those who play mods are the same people who are most likely to use a tool like
this.  Therefore, it is the project's best interest to have 100% mod
compatilbity.  However, no one has the time to test all mods and ensure smooth
execution, so as a modder, it is your duty to contribute to the testing part of
this project.  Add a representative savegame to the repository and write a
series of tests to ensure compatibility.  If everything works, great.  If not,
file an issue and someone can take a look at it.

## Who can contribute

Anyone!  This project is intended to be a low barrier for anyone, especially
those who are unaccostomed to open source projects.  We're all friendly here.
Make changes, add issues, start a discussion.

## Why not use .Net 4.5

Since EU4 can be ran on Windows XP and .Net 4.5 can't be installed on XP
machines.  .Net 4.0 is the lowest common denominator.

## What about Linux/Mac

This project's first priority is Windows; however, Linux/Mac may be reached one
day with Mono.  Caveat, I do not know Mono, so this is a great opportunity to
help!

## Design Guidelines

Fail early, fail hard - The last desired outcome is for the user to believe the
savegame was parsed correctly, when the contrary is true.  After parsing or
updating each entity, a validation check must be performed and a failure results
in an exception being thrown.  This also alerts us to possible bugs in the
parsing sooner than if we allowed more flexible input.

StyleCop - A (minor) goal of this project is to make it appear that one
developer wrote it.  This ensures consistency and a high level of programming.
While I can scan through all changes, I will make mistakes.  StyleCop doesn't.
