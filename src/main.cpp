#ifdef TORICKNIFE_WINDOWS_MSVC
#include <windows.h>
#endif
#include <iostream>
#include "shader.h"

#ifdef _IRR_WINDOWS_
#pragma comment(lib, "Irrlicht.lib")
#pragma comment(linker, "/subsystem:windows /ENTRY:mainCRTStartup")
#endif

using namespace toricknife;

int main() {
    GetMarchingCubesTemplate();
    std::cout << "Testing" << std::endl; 
    return 0;
}