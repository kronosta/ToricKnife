# ToricKnife
An attempt at an N-Dimensional graphics library and physics engine that tries to circumvent the exponential inefficiencies

The idea is we use isosurfaces as objects, then use marching cubes to turn them into a triangle mesh because that is fast, then use OpenGL to render that (the original plan was to use Irrlicht but that seems like overkill).
This will allow us to render anything (with reasonable accurace) that can be expressed by a formula (possibly with custom 
functions which may even involve samplers to get shapes which are hard to make equations for),
such as toratopes, but also we can easily define custom formulas using loops, conditionals, functions, etc.

And for collision detection, the idea is for some lower dimensions we just check a bunch of points and if the values there are negative in both shapes there's an intersection and
therefore a collision, then just flip the velocity and backtrack until the shapes are no longer intersecting. Then we can transfer the momentum or whatever.

# Progress
The project is in VERY early development. It doesn't render anything currently because the vertex and fragment shaders
refuse to run for whatever reason, but I can confirm the marching cubes algorithm is running (though I don't have 
evidence to say whether it's doing well)

# Compilation
Currently this compiles on WSL. I ran into all sorts of trouble trying to get the GLEW functions to link properly,
this might just be a quirk of my system but it was annoying and I wouldn't wish it upon my worst enemy.

# Licensing
As much as I would like to make this a (mostly) free-for-all endeavor with MIT or BSD, I realize that this project (if finished) is going to involve a lot of effort and time. 
So I've put it under CC-BY-NC-SA 4.0 (read the license for details, but in general you must credit me, indicate if changes were made, and provide a link to the license; distribute
modifications under the same license; and commercial use is not allowed). The intention of this is that if someone wants to use this commercially (doubt this will happen but maybe)
then I would be able to get paid for it.

The marching cubes shader was created by Michael Walczyk, licensed under CC-BY 4.0
(https://creativecommons.org/licenses/by/4.0/). It is modified from its original form.