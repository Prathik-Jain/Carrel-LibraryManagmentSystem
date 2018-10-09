# Carrel - Library Management System
_Not ready for production_

Carrel is an Open Source application designed to manage any library. Given the flow of books each day keeping track of it  is a hectic task. In addition to flow of books; maintaining data about each student, staff and all the heavy load of books makes it even more difficult. Carrel helps to provide information on any book present in the library. It keeps track of books issued, returned and added to the library. The application makes use of QR codes for ease of the process.It provides a simple graphical user interface for the Library Staff to manage the functions of the library effectively.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites
* Software
  - Windows 10
  - .NET framework - `4.6.1`
  - Visual Studio 2017 or later
  - Microsoft SQL Management Server 2017
* Hardware
  - Processor with speed of `1GHz` or more.
  - RAM : `4GB` or more
  - Webcam

### Installing

A step by step series of examples that tells you how to get a development environment running

Clone the GitHub repository
```
git clone https://github.com/Prathik-Jain/Carrel-LibraryManagmentSystem.git
```
Create local database
- Open `CARREL.sql` in Database folder with SQL Server Management Studio and  Execute it.

Build the solution
- Open `LibraryManagement.sln` in Visual Studio
- Click on `Build` tab -> `Build - LibraryManagement.sln`
- Click on `Start` to start working with the project.

When the projecte is executed - It will ask you to scan the QR to login.
Initially a `test` Admin is created when we create the local Database.

Scan the follwing QR to login - the deault PIN is `0000`

![Test QR](TestQR.png)

Once logged in you can scan the `TEST` Admin QR to access the Admin Option - Where you can create a new admin and also print card for the same.

## Built With

* [Aforge](http://www.aforgenet.com/) - Framework to work with camera.
* [ZXing.Net](https://www.nuget.org/packages/ZXing.Net/) - Image processing library to decode and generate QR Codes.
* [MaterialDesignInXamlToolkit](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit) - Library used to integrate Google's Material Design in XAML.
* [NewtonSoft.JSON](https://newtonsoft.com/josn) - Library used to serialize and deserialize JSON objects

## Authors

* **[Prathik Jain](https://github.com/Prathik-Jain)**
* **[Shreya S U](https://github.com/Shrey98)**
* **[Shreyas M U](https://github.com/shreyasmu)**

See also the list of [contributors](https://github.com/Prathik-Jain/Carrel-LibraryManagmentSystem/graphs/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
