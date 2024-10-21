CC=gcc
CPP=g++
CPPFLAGS=-fno-use-cxa-atexit
CCFLAGS=
BUILDDIR=./bin
SRCDIR=./src
INCLUDEDIR=./include

LD=c++
DASH=-
LDFLAGS = -lGLEW -lglfw -lglut -lOpenGL -lGL -lc


OBJS= \
	$(BUILDDIR)/main.o \
	$(BUILDDIR)/shader.o \
	$(BUILDDIR)/glew.o

TARGETEXEC=toric-knife

$(BUILDDIR)/$(TARGETEXEC): $(OBJS)
	$(LD) $(OBJS) -o $(BUILDDIR)/$(TARGETEXEC) $(LDFLAGS)

$(BUILDDIR)/%.o: $(SRCDIR)/%.cpp
	$(CPP) $(CPPFLAGS) -I$(INCLUDEDIR) -c $< -o $@

$(BUILDDIR)/%.o: $(SRCDIR)/%.c
	$(CC) $(CCFLAGS) -I$(INCLUDEDIR) -c $< -o $@

clean:
	rm $(BUILDDIR)/$(TARGETEXEC) $(OBJS) ; true