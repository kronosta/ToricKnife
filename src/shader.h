#ifndef TORICKNIFE_INCLUDE_SHADER
#define TORICKNIFE_INCLUDE_SHADER

#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#ifdef TORICKNIFE_WINDOWS_MSVC
#include <windows.h>
#endif
#define GLEW_STATIC
#include <GL/glew.h>

namespace toricknife {

    class Shader;

    extern std::string MarchingCubesTemplate;
    
    extern void GetMarchingCubesTemplate();
    extern Shader MarchingCubesFromFunctions(std::string funcsurface, std::string funcnormal, std::string funchelpers);

    
    class Shader {
    public:
        // the handle for the program that gets generated by the constructor
        unsigned int ID;
        // will be true if the program represented by ID has been deleted
        char Invalid = false;
        // is true if this is a compute shader
        char IsCompute;
        // constructor generates vertex and fragment shaders from the path
        // ------------------------------------------------------------------------
        Shader(const char* vertexPath, const char* fragmentPath);
        // constructor generates a program from a string using glCreateShaderProgramv
        Shader(GLenum type, GLsizei count, char** sourceFiles);
        // activate the shader
        void Use();
        // utility uniform functions
        // Sets a bool uniform;
        void SetBool(const std::string& name, bool value) const;
        // Sets an int uniform
        void SetInt(const std::string& name, int value) const;
        // Sets a float uniform
        void SetFloat(const std::string& name, float value) const;
        // deletes the program associated with this object and sets the Invalid flag on this object
        void Delete();
        // dispatches the shader as a compute shader if it is one
        void DispatchCompute(GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z);
    private:
        // utility function for checking shader compilation/linking errors.
        // ------------------------------------------------------------------------
        void CheckCompileErrors(unsigned int shader, std::string type);
    };
}

#endif