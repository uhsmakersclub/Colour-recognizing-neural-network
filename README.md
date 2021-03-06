# Colour-recognizing-neural-network
A modifiable neural network platform using gradient descent that is configured to recognize colours.

# Info on running the network
I made this network in VSCode, but it is compiled using the .NET Core, so you probably don't need to use VSCode to run it. The actual program is in the folder called "Neural_network". 
You can run the program in a UNIX-based terminal like this:
```
cd Neural_network
dotnet run
```

There is another program in the repository that makes it slightly easier to generate training data, which is made in (and should probably be run in) Processing 3. 

So far, I have tested this on macOS and Ubuntu machines, and both work correctly.

# Experimental version
The file named "Experimental-program.cs" is the experimental version of the network. It is testing some new ideas about how the neural network could function and how it can be improved (because as of when I'm writing this, it doesn't work). To test it, just copy the code inside it and paste it into a new C# project.
