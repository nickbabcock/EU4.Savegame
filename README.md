# EU4.Savegame

(.Net 4.5/Mono 3.x) [![Build status](https://ci.appveyor.com/api/projects/status/re6omq30w699ue7v)](https://ci.appveyor.com/project/nickbabcock/eu4-savegame) [![Build Status2](http://img.shields.io/travis/nickbabcock/EU4.Savegame.svg?branch=master&style=flat)](https://travis-ci.org/nickbabcock/EU4.Savegame)

If you're not a programmer, this is probably not what you're looking for (yet!).

EU4.Savegame is a C# library currently in the alpha stage designed to read and
write save games from Europa Universalis 4. A design goal of this library is to
be mod and savegame version agnostic, relying on the client to provide more
contextual information.

To integrate the library into your code, please download this repository and
build the project. From there, add a reference from your own solution to the
newly built library.

You can also skip creating your own solution and access the library directly by
using a third party solution. These would be easiest if your use case would be
writing scripts using the library.

The easiest way would probably be [CsharpRepl][].  The following instructions
for CsharpRepl are for Linux users.  Other users will have to modify them to
fit their needs.  

- Download and EU4.Savegame.dll and store them in ~/.config/csharp
- Start the repl by calling `csharp`
- Execute the code

```csharp
var save = new EU4.Savegame.Savegame(new FileStream("<path to file>"));

//Let's change a province's name!
save.Provinces.Get(1).Name = "My province";
save.Save(new FileStream("<new path to file>"));
```

Another example is with [LINQPad][].

- Download Pdoxcl2Sharp.dll (the parser) and EU4.Savegame.dll.  Store anywhere.
- Start LINQPad and in Additional References, choose Add, and browse to the
  downloaded files.
- Write program as above.

But I understand if there is resentment to downloading additional programs,
so there is a way that you can use pre-installed software!

- Download EU4.Savegame.dll.  Sorry couldn't get away with this
- Open your favorite text editor or Notepad if you don't have one
- Write the code snippet shown below
- Compile the code with %windir%\Microsoft.NET\Framework\v4.0.30319\csc.exe
  /reference:EU4.Savegame.dll
- Run the output executable

```csharp
using System.IO;
class Program 
{
    static void Main(string[] args)
    {
        //insert snippet from above
    }
}
```

## Contributing

So you want to help?  Great!  Here are a series of steps to get you on your way!
Let me first say that if you have any troubles, file an issue.

- Get a github account
- Fork the repo
- Add your changes
- Commit your changes in such a way there is only a single commit difference
  between the main branch here and yours.  If you need help, check out [git
  workflow][]
- Push changes to your repo
- Submit a pull request and I'll review it!
    
## License

EU4.Savegame is licensed under MIT, so feel free to do whatever you want, as
long as this license follows the code.

## Mod compatibility

Mods are of a huge importance to the community and likewise in this project.
Those who play mods are the same people who are most likely to use a tool like
this.  Therefore, it is the project's best interest to have 100% mod
compatilbity.  However, no one has the time to test all mods and ensure smooth
execution, so as a modder, it is your duty to contribute to the testing part of
this project.

## Who can contribute

Anyone!  This project is intended to be a low barrier for anyone, especially
those who are unaccostomed to open source projects.  We're all friendly here.
Make changes, add issues, start a discussion.

## Why not use .Net 4.5

Since EU4 can be ran on Windows XP and .Net 4.5 can't be installed on XP
machines.  .Net 4.0 is the lowest common denominator. This doesn't mean that
clients who use this library shouldn't use .Net 4.5, it is a great platform,
and use it if you can.

## What about Linux/Mac

This project works with [Mono][], which is an alternative .Net implementation.

[Mono]: http://www.mono-project.com/Main_Page

## Design Guidelines

Fail early, fail hard - The last desired outcome is for the user to believe the
savegame was parsed correctly, when the contrary is true.  After parsing or
updating each entity, a validation check must be performed and a failure results
in an exception being thrown.  This also alerts us to possible bugs in the
parsing sooner than if we allowed more flexible input.

Custom collections - throughout the code, the API exposes custom collections
and not traditional collections such as a `IList<T>`.  There are several
reasons to this, including some found in Framework Design Guidelines

- Exposes internal implementation 
- Exposes memebers not applicable to the class (Binary searching a list of
  provinces isn't very intuitive)
- Provinces are indexed at 1.  This might not seem like a huge deal, but I have
  been burned one too many times when looping through a collection of provinces
  and using the loop counter as an index only to get an out of range error (or
  worse, inaccurate results).  

[git workflow]: https://sandofsky.com/blog/git-workflow.html
[CsharpRepl]: http://www.mono-project.com/CsharpRepl
[LINQPad]: http://www.linqpad.net/
