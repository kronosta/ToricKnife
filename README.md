# ToricKnife
An attempt at an N-Dimensional graphics library and physics engine that tries to circumvent the exponential inefficiencies

The idea is we use isosurfaces as objects, then use marching cubes to turn them into a triangle mesh because that is fast, then use OpenGL to render that (the original plan was to use Irrlicht but that seems like overkill).
This will allow us to render anything that can be expressed by a formula,
such as toratopes, but also we can easily define custom formulas using loops, conditionals, functions, etc.

And for collision detection, the idea is for some lower dimensions we just check a bunch of points and if the values there are negative in both shapes there's an intersection and
therefore a collision, then just flip the velocity and backtrack until the shapes are no longer intersecting. Then we can transfer the momentum or whatever.

# Compilation
Currently this compiles on WSL. I ran into all sorts of trouble trying to get the GLEW functions to link properly,
this might just be a quirk of my system but it was annoying and I wouldn't wish it upon my worst enemy.