# ToricKnife
An attempt at an N-Dimensional graphics library and physics engine that tries to circumvent the exponential inefficiencies

The idea is we use isosurfaces as objects, then use marching cubes to turn them into a triangle mesh because that is fast, then use OpenGL to render that (the original plan was to use Irrlicht but that seems like overkill).
This will allow us to render anything (with reasonable accurace) that can be expressed by a formula (possibly with custom 
functions which may even involve samplers to get shapes which are hard to make equations for),
such as toratopes, but also we can easily define custom formulas using loops, conditionals, functions, etc.

And for collision detection, the idea is for some lower dimensions we just check a bunch of points and if the values there are negative in both shapes there's an intersection and
therefore a collision, then just flip the velocity and backtrack until the shapes are no longer intersecting. Then we can transfer the momentum or whatever.

## Ideas/plans
- Shapes
    - All dimensions will support shapes calculatable by equations, such as toratopes and bracketopes, as well
        as toratope substitutions (toratopes normally use a square root of a sum of squares to represent something 
        n-spherical. Replacing one of these bits with another expression with the same axes will treat it as if it's equal to 
        zero and replace the n-sphere with something else.
        For example, the formula for a 3D torus is sqrt((sqrt(x^2 + y^2) - r_1)^2 + z^2) - r_2 = 0 .
        The formula for a diamond is abs(x) + abs(y) - r = 0.
        So sqrt((abs(x) + abs(y) - r_1)^2 + z^2) - r_2 = 0 produces a torus with diamond cross sections,
        and abs(sqrt(x^2 + y^2) - r_1) + abs(z) - r_2 = 0 produces a circle tracing the path of a diamond.
        Combining these gives abs(abs(x) + abs(y) - r_1) + abs(z) - r_2 = 0, which makes a diamond trace a diamond. 
        )
    - Calculate a field of inside vs. outside given a polytope (maybe a .off file?).
    - This can then be put in an image or sampler and sampled in a function, then the polytope can be used
        in other equations such as toratope replacements. So polytopes can be rendered.
    - At a certain point a reasonable sampler will no longer be able to fit a whole field.
        3D is the maximum dimension for a sampler, but it could be divided up into slices (like (25^2)^3 = 625x625x625, that's pretty massive though, 244140625 pixels to worry about storing). Slices would be too numerous to nest
        so I think 6D is the absolute limit for fully stored polytopes.
    - Cartesian products
    - After the polytope method fails, Cartesian products can help with faceted shapes. They produce n-prisms,
        which are generally more boring than full polytopes but can still have some decent variety.
    - Cartesian products require solid shapes (so just detect inside vs. outside or use an SDF). However,
        once you have that, checking if a point is in the shape is easy; just check all the component points
        and AND them together.
    - Compounds
        - For compounds, min produces a union and max produces an intersection. I believe there are also ones for XOR
            and subtract but I forget.
- Non-Euclidean ND geometries??? (maybe, if I can figure out what some of them even are or how to slice them)
- Physics
    - Collision detection in lower dimensions by checking points in a grid for both being inside the shape, then
        backtracking momentum little by little to undo the intersection, and transfer momentum.
    - Collision detection in higher dimensions by hypercube or hypersphere colliders (since the previous strategy
        would massively decrease in efficiency as the dimensionality increases. Some accuracy can be retained for
        a little while by decreasing grid resolution). Multiple colliders in one object could handle complex cases.
    - Velocity, acceleration, jerk/jolt, snap/jounce, and maybe crackle and pop
        - Also for rotation and scale

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