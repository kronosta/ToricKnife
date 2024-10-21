#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#ifdef TORICKNIFE_WINDOWS_MSVC
#include <windows.h>
#endif
#include "shader.h"

namespace toricknife {

    std::string MarchingCubesTemplate;

    void GetMarchingCubesTemplate() {
        std::ifstream file;
        file.exceptions(std::ifstream::failbit | std::ifstream::badbit);
        try {
            file.open("media/marching-cubes.comp");
            std::stringstream stream;
            stream << file.rdbuf();
            file.close();
            MarchingCubesTemplate = stream.str();
        }
        catch (std::ifstream::failure& e) {
            std::cout << "Error: could not read critical marching cubes shader template \
(located at media/marching-cubes.comp). While running: toricknife::GetMarchingCubesTemplate." << std::endl;
            std::cout << e.what() << std::endl;
        }
    }

    Shader MarchingCubesFromFunctions(std::string funcsurface, std::string funcnormal, std::string funchelpers) {
        std::string funcsurfaceInter = "@@FUNCSURFACE@@",
                    funcnormalsInter = "@@FUNCNORMALS@@",
                    funchelpersInter = "@@FUNCHELPERS@@";
        std::string newcode = MarchingCubesTemplate;
        newcode = newcode.replace(newcode.find(funcsurfaceInter), funcsurfaceInter.length(), funcsurface);
        newcode = newcode.replace(newcode.find(funcnormalsInter), funcnormalsInter.length(), funcsurface);
        newcode = newcode.replace(newcode.find(funchelpersInter), funchelpersInter.length(), funcsurface);
        char* cstr = (char*)(newcode.c_str());
        char** codes = &cstr;
        return Shader(GL_COMPUTE_SHADER, 1, codes);
    }

    // This code shamelessly ripped from: https://learnopengl.com/code_viewer_gh.php?code=includes/learnopengl/shader_s.h
    // constructor generates the shader on the fly
    // ------------------------------------------------------------------------
    Shader::Shader(const char* vertexPath, const char* fragmentPath) {
        // 1. retrieve the vertex/fragment source code from filePath
        std::string vertexCode;
        std::string fragmentCode;
        std::ifstream vShaderFile;
        std::ifstream fShaderFile;
        // ensure ifstream objects can throw exceptions:
        vShaderFile.exceptions(std::ifstream::failbit | std::ifstream::badbit);
        fShaderFile.exceptions(std::ifstream::failbit | std::ifstream::badbit);
        try {
            // open files
            vShaderFile.open(vertexPath);
            fShaderFile.open(fragmentPath);
            std::stringstream vShaderStream, fShaderStream;
            // read file's buffer contents into streams
            vShaderStream << vShaderFile.rdbuf();
            fShaderStream << fShaderFile.rdbuf();
            // close file handlers
            vShaderFile.close();
            fShaderFile.close();
            // convert stream into string
            vertexCode = vShaderStream.str();
            fragmentCode = fShaderStream.str();
        }
        catch (std::ifstream::failure& e) {
            std::cout << "Error: The shader file " << e.what() << " could not be read. While running: toricknife::Shader::Shader(const char* vertexPath, const char* fragmentPath)." << std::endl;
        }
        const char* vShaderCode = vertexCode.c_str();
        const char* fShaderCode = fragmentCode.c_str();
        // 2. compile shaders
        unsigned int vertex, fragment;
        // vertex shader
        vertex = glCreateShader(GL_VERTEX_SHADER);
        glShaderSource(vertex, 1, &vShaderCode, NULL);
        glCompileShader(vertex);
        CheckCompileErrors(vertex, "VERTEX");
        // fragment Shader
        fragment = glCreateShader(GL_FRAGMENT_SHADER);
        glShaderSource(fragment, 1, &fShaderCode, NULL);
        glCompileShader(fragment);
        CheckCompileErrors(fragment, "FRAGMENT");
        // shader Program
        ID = glCreateProgram();
        glAttachShader(ID, vertex);
        glAttachShader(ID, fragment);
        glLinkProgram(ID);
        CheckCompileErrors(ID, "PROGRAM");
        // delete the shaders as they're linked into our program now and no longer necessary
        glDeleteShader(vertex);
        glDeleteShader(fragment);
        IsCompute = false;
    }

    Shader::Shader(GLenum type, GLsizei count, char** sourceFiles) {
        ID = glCreateShaderProgramv(type, count, sourceFiles);
        if (type == GL_COMPUTE_SHADER)
            IsCompute = true;
        else
            IsCompute = false;
    }

    // activate the shader
    // ------------------------------------------------------------------------
    void Shader::Use() {
        glUseProgram(ID);
    }
    // utility uniform functions
    // ------------------------------------------------------------------------
    void Shader::SetBool(const std::string& name, bool value) const {
        glUniform1i(glGetUniformLocation(ID, name.c_str()), (int)value);
    }
    // ------------------------------------------------------------------------
    void Shader::SetInt(const std::string& name, int value) const {
        glUniform1i(glGetUniformLocation(ID, name.c_str()), value);
    }
    // ------------------------------------------------------------------------
    void Shader::SetFloat(const std::string& name, float value) const {
        glUniform1f(glGetUniformLocation(ID, name.c_str()), value);
    }

    void Shader::Delete() {
        glDeleteProgram(ID);
        Invalid = true;
    }

    void Shader::DispatchCompute(GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z) {
        if (IsCompute) {
            this->Use();
            glDispatchCompute(num_groups_x, num_groups_y, num_groups_z);
        }
    }
    // utility function for checking shader compilation/linking errors.
    // ------------------------------------------------------------------------
    void Shader::CheckCompileErrors(unsigned int shader, std::string type) {
        int success;
        char infoLog[1024];
        if (type != "PROGRAM") {
            glGetShaderiv(shader, GL_COMPILE_STATUS, &success);
            if (!success) {
                glGetShaderInfoLog(shader, 1024, NULL, infoLog);
                std::cout << "ERROR::SHADER_COMPILATION_ERROR of type: " << type << "\n" << infoLog << "\n -- --------------------------------------------------- -- " << std::endl;
            }
        }
        else {
            glGetProgramiv(shader, GL_LINK_STATUS, &success);
            if (!success) {
                glGetProgramInfoLog(shader, 1024, NULL, infoLog);
                std::cout << "ERROR::PROGRAM_LINKING_ERROR of type: " << type << "\n" << infoLog << "\n -- --------------------------------------------------- -- " << std::endl;
            }
        }
    }
}