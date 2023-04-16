# OptimizationVolume

`OptimizationVolume` is a C# script for the Unity game engine that enables control of the active state and spawning of GameObjects within specified volumes. It includes a number of configurable parameters for controlling auto-spawn on start, use of spawning, and the array of GameObjects to control.

## Installation

To use `OptimizationVolume` in your Unity project, simply add the `OptimizationVolume.cs` script to your project's Assets folder. You can then attach the script to a game object in your scene and configure the parameters as desired.

## Usage

`OptimizationVolume` provides several methods for controlling the active state and spawning of GameObjects within specified volumes. It is intended to be used in conjunction with the `ProceduralMachine` class to enable fine-grained control over procedural generation of objects within a Unity scene.

To use `OptimizationVolume`, first create an optimization volume game object in your scene. This can be any shape or size you like, and can be positioned anywhere within your scene. Next, add the `OptimizationVolume.cs` script to the game object, and set its parameters as desired.

You can then configure the `items` array to include the GameObjects you want to control within the volume. You can also set the `autoSpawnOnStart` parameter to true to enable auto-spawning of GameObjects within the volume on start.

You can then add a `ProceduralMachine` game object to your scene and attach the `ProceduralMachine.cs` script to it. Configure the parameters as desired, and set the `volumes` list to include the optimization volume(s) you created earlier.

You can then call the `Do` method on the `ProceduralMachine` object to spawn objects within the optimization volume(s), or call the `SetActiveAllObjects` method to set the active state of all spawned objects.

For more information on how to use `OptimizationVolume` and `ProceduralMachine`, refer to the comments in the scripts themselves.

## License

`OptimizationVolume` is released under the MIT license. See the `LICENSE` file for more information.

## Credits

`OptimizationVolume` was created by Robert Rumney for Ingozi Games. If you find this script useful, consider [buying me a beer](https://www.buymeacoffee.com/rumnizzle) or [following me on Twitter](https://twitter.com/rumnizzle).
