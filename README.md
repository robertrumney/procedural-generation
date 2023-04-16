# ProceduralMachine

ProceduralMachine is a C# script for the Unity game engine that enables procedural generation of objects within specified volumes. It includes a number of configurable parameters for controlling the range and number of objects to spawn, as well as minimum distance requirements for player camera and NavMesh.

## Installation

To use ProceduralMachine in your Unity project, simply add the `ProceduralMachine.cs` script to your project's Assets folder. You can then attach the script to a game object in your scene and configure the parameters as desired.

## Usage

ProceduralMachine provides several methods for spawning objects within optimization volumes, as well as setting the active state of all spawned objects. There are also methods for spawning objects at a specified point within an optimization volume.

To use ProceduralMachine, first create an optimization volume game object in your scene. This can be any shape or size you like, and can be positioned anywhere within your scene. Next, add the `OptimizationVolume.cs` script to the game object, and set its parameters as desired.

You can then add a ProceduralMachine game object to your scene and attach the `ProceduralMachine.cs` script to it. Configure the parameters as desired, and set the `volumes` list to include the optimization volume(s) you created earlier.

You can then call the `Do` method on the ProceduralMachine object to spawn objects within the optimization volume(s), or call the `SetActiveAllObjects` method to set the active state of all spawned objects.

For more information on how to use ProceduralMachine, refer to the comments in the script itself.

## License

ProceduralMachine is released under the MIT license. See the `LICENSE` file for more information.

## Credits

ProceduralMachine was created by Robert Rumney for Ingozi Games. If you find this script useful, consider [buying me a beer](https://www.buymeacoffee.com/rumnizzle) or [following me on Twitter](https://twitter.com/rumnizzle).
