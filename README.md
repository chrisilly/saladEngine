# How to Get and Run the Project

1. Install Unity version `6000.0.36f1`
2. Clone [the repo](https://github.com/chrisilly/saladEngine)
    - Save it somewhere you'll remember
3. Import the cloned repository into Unity Hub
    1. In Unity Hub, go to `Projects`
    2. Click `add project`
    3. In the drop-down menu, click `add project from disk`
    4. Navigate to and select the cloned git repository
4. Launch the project via Unity Hub

**NOTE!** If you're using Visual Studio as your IDE, make sure that the Unity package `Visual Studio Editor` is installed to get working intellisense and project file generation. After opening the project via Unity Hub, navigate to `Window`>`Package Manager` using the menu bar before searching for and installing the `Visual Studio Editor` package.

# About the Project

We use our own rigidbodies utilising the semi-implicit Euler method to calculate and update their positions.

$$
v_2 = v_1 + h\frac{F_1}{m}
$$

$$
x_2 = x_1 + hv_2
$$