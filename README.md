# tofu3d
### A Testable Object-Oriented Framework for Unity3D 

v0.0.6
NOTE: tofu3d is currently in an unstable state as I am working on a game project using it and trickling functionality down as necessary. I intend to get it fully working when that project has concluded. If you are curious about tofu3d, please reach out here or on twitter, @joshuadotworks.

<img align="left" width=128 height=128 src="./TOFULogoSmall.png" alt="tofu3d">

tofu3d is a Unity3D framework built to be more unit-testable. We accomplish this by removing as much logic as possible from MonoBehaviours and putting them in well-behaving, well-tested, generalized non-MonoBehaviour C# classes. It also will contain some other core functionality for more test-based development, and a full plugin architecture that emphasizes maintainable, decoupled code and automated unit testing.

## Usage:
This repository can be included as a submodule in your repo and accessed within your project.
TOFU3D is built for games that isolate game logic from physics and graphics. It would be a poor fit for a physics sim or small arcade game. It's a better choice for projects that have complex behind-the-scenes interactions that require unit testing, such as strategy, simulation or RPG games.

## Dependencies:
TOFU3D is built with NSubstitute to aid testing. I will be adding a suite of helper methods that aid testing using NSubstitute subs.

## Status:
TOFU3D is still under construction, and somewhat unstable. Will be adding in-project tests, demos and documentation when I have time. I am developing this as a submodule inside a game project, so expect this to be updated as I need more features in the game.

Currently includes dependency injection, a resource library system, an entity control and storage layer, and an event system.

Documentation to come.

## Core Modules

### Services
A service (in either MonoBehaviour or plain flavours) can be bound to a ServiceContext and retrieved. It will also be injected into any other services bound in the same context.

### Events
Events are passed around in an EventContext with dynamic event payloads. Event listeners and triggers must all be bound in terms of an event context, which is a service in itself.

### Glops
A Glop (Generalized Local Object or Process) is a single object that is contained in a GlopContainer. Generally, Glop is subclassed to represent all live data in a tofu game.

GlopContainers are responsible for holding all Glops and acting as their interface to other services.

On data serialization, GlopContainers dump their content Glops to be serialized. On deserialization, each GlopContainer must call for its Glops to "resolve" after they receive data so that they can rebuild inter-Glop links and rebind any necessary services.

### Resource Libraries
Resource libraries allow the serving and storage of items of a specified type. Each resource library is a service. 
Use ResourceLibraries to load and store data from formats like XML or JSON, to load in prefabs, to maintain custom enum-like classes or to serve media like strings, sprites, textures, models, functions and sound.

Resource libraries are similar to GlopContainers in that they contain and serve objects; however, a ResourceLibrary does not ever update its objects after load and ResourceLibrary objects must be accessible in O(1) time by string lookup. GlopContainers are generally self-contained, and their contents must be accessed through queries through the GlopContainer service (or by a GlopId in O(1)). ResourceLibraries are intended to be used by many services to access necessary data.

### Command
Can store and execute player/AI commands.

### Configuration
Holds an array of key-value pairs for config that can be consumed and held in an object's Properties property.

### Agents
Agents are objects with a location, ID, renderable sprite/model etc., access to pathfinding, a configurable array of ResourceModules, a Properties collection, actions and AI behaviour. 

### Frame Update Service
A simple MonoBehaviour that sends an "Update" message with a delta time to all listeners.

### Resource Module
An object that contains some float amount of resource out of a possible max amount of resource, and includes methods for depleting/spending that resource and can trigger events from that. Could be used with any numerical resource, such as health in an RPG game, bullets in an FPS, or wood in an RTS.

## Included Plugins
Plugins are useful behaviour that relies on core functionality. Some plugins may have plugin dependencies and all plugins require core functionality. They are built to be easily replaced.

### Renderables
Renderables define animation, sprite and model properties to represent Glop and other object as visual GameObjects in a Unity scene.

### Positioning Service
A service that keeps track of the position of objects that take up space for collisions and jostling.

### Pathfinding
An implementation of A* pathfinding that samples from a provided data map.

### UI
TBA

## Disclaimer:
I'm a computer science student using this to get a good understanding of system design etc. My goal is to create a framework within Unity that facilitates quick game development using object oriented principles and unit testing, while not having a disastrous impact on game performance. This is obviously a poor fit for many types of games. I do not know everything, and appreciate any feedback on the decisions I've made.
