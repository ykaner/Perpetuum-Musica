# Perpetuum-Musica

Perpetuum Musica is a proffessional Musica Management Software. 

![alt Perpetuum Musica Design](http://harshuv.com/devresume/img/PerpetuumMusica.jpg)

## How to run

Just run the PepetuumMusica.exe in the main repository folder.

## Documentation

The application is built with the MVVM design pattern recomanded for WPF applications. Which consists of 3 parts:
* **Model** All actuall logic (like "backend") in C#.
* **ViewModel** Connecting between the Model and View in C#.
* **The View** in XAML, description of the appearance of the application. Xaml is an xml language for WPF for creating GUI to C# applications. 

For each one of those we have a main class with its name as well as some other classes for dialogs and such. 

The player itslef is on a different visual studio project.

We also have a class for the data base connections. 
