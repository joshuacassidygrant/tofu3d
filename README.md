# tofu3d
### A Testable Object-Oriented Framework for Unity3D 

v0.0.3

<img align="left" width=128 height=128 src="./TOFULogoSmall.png" alt="tofu3d">

tofu3d is a Unity3D framework built to be more unit-testable. We accomplish this by removing as much logic as possible from MonoBehaviours and putting them in well-behaving, well-tested, generalized non-MonoBehaviour C# classes. It also will contain some other core functionality for more test-based development, and a full plugin architecture that emphasizes maintainable, decoupled code and automated unit testing.

## Usage:
Can be included as a submodule and accessed within your project.
tofu3d is built for games that are able to put their game logic separate from Unity's physics and graphics. It would be a poor fit for a physics sim or small arcade game.

## Status:
tofu3d is still under construction, and totally unstable. Will be adding in-project tests, demos and documentation when I have time. I am developing this as a submodule inside a game project, so expect this to be updated as I need more features in the game.

Currently includes dependency injection, a resource library system, and an event system.

Documentation to come.

## Core Modules

### Services
A service (in either MonoBehaviour or plain flavours) can be bound to a ServiceContext and retrieved. It will also be injected into any other services bound in the same context.

### Events
Events are passed around in an EventContext with dynamic event payloads. Event listeners and triggers must all be bound in terms of an event context, which is a service in itself.

### Resource Libraries
Resource libraries allow the serving and storage of items of a specified type. Each resource library is a service.

### Command
Can store and execute player/AI commands.

### Configuration
Holds an array of key-value pairs for config.

## Included Plugins
Plugins are useful behaviour that relies on core functionality. Some plugins may have plugin dependencies.

### Frame Update Service
A simple MonoBehaviour that sends an "Update" message with a delta time to all listeners.

### Agents
Agents are objects with a location, ID, renderable sprite/model etc. that can have AI behaviour.


## Disclaimer:
I'm a computer science student using this to get a good understanding of system design etc. I do not know everything, and appreciate any feedback on the decisions I've made.
