## Cake.PathTildeHelper

[Cake](https://cakebuild.net/) add-in to provide aliases to help translate tilde (`~`) characters to absolute paths usable in locations requiring a full path.

### When do I use it?

Most of the Cake path systems don't seem to handle the tilde (`~`) character. They typically make them a path relative to the working directory with the name of `~`. If you ever want to provide input paths to a Cake script that have a leading tilde (`~`) in them, such as `~/Projects/my-cool-file.txt` or `~/Documents`, these helpers will translate those values to an absolute path that other Cake aliases and add-ins will be able to handle.

### How do I use it?

Add a directive to the top of your **build.cake** file to include the project NuGet package.

```
#addin nuget:?package=Cake.PathTildeHelper
```

After that, you can call the `PathReplaceTilde` alias from your Cake script and any tasks within it.

```csharp
Information(PathReplaceTilde(new FilePath("~/Documents/awesome-stuff.txt")));
```

On Windows, that will translate the path to something like `C:\Users\yourusername\Documents\awesome-stuff.txt` (though Cake will keep that path with `/` slashes instead).

On macOS and friends, that will translate the path to something like `/Users/yourusername/Documents/awesome-stuff.txt`.

You can also use it for directories, swapping the `FilePath` for a `DirectoryPath`.

```csharp
Information(PathReplaceTilde(new DirectoryPath("~/Documents")));
```
