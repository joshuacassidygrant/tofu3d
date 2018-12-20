# tofu3d
*Version 0.0.1*

tofu3d is a Unity3D framework built to be more unit-testable. We accomplish this by removing as much logic as possible from MonoBehaviours and putting them in well-behaving, well-tested, generalized non-MonoBehaviour C# classes. It also will contain some other core functionality for more test-based development, and a full plugin architecture that emphasizes maintainable, decoupled code and automated unit testing.

##Usage:
Can be included as a submodule and accessed within your project.
tofu3d is built for games that are able to put their game logic separate from Unity's physics and graphics. It would be a poor fit for a physics sim or small arcade game.

##Status:
tofu3d is still under construction, and totally unstable. Will be adding in-project tests, demos and documentation when I have time. I am developing this as a submodule inside a game project, so expect this to be updated as I need more features in the game.

Currently includes dependency injection, a resource library system, and an event system.

Documentation to come.
