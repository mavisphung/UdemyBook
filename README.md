# UdemyBook
A training project using C#
# How to run
- Install [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) if have not installed yet
	- In Visual Studio Installer, please tick **ASP.NET and Web Development**
- Install [Microsoft SQL Server 2018](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) if have not installed yet
	- The first run project maybe occur database error because of desktop's name.
	- To fix this, please follow these steps:
		- Open project in VS2019
		- In Solution Explorer tab, open **appsettings.json**
		- In *DefaultConnection*, change Server=\<your computer name\> instead of *SE140675*
		- *Notice*: Always connect **Microsoft SQL Server Management Studio 18** *(MSSMS)* with **Windows Authentication** option
	- If the project is ran and there is **no any books or categories, follow these steps to fix this**
		- Double click **udemy.sql**
		- In *udemy.sql* file, *Ctrl + A* to choose all code
		- Press F5 after choosing. (At this step, MSSMS will automatically generate database with data)
- When installation is finished, in VS2019, File > Open > Project/Solution
- Choose the file name **UdemyBook.sln**
- In Solution Explorer, right click on "Solution UdemyBook....", choose **Rebuild solution**
- Ctrl + F5 to run project

# Before evaluating, PLEASE READ THIS
To evaluate this project, please follow these directory:
 - For static file such as images, css, js, jquery: **UdemyBook/wwwroot**
 - For HTML code: **Areas/Customer/Views**
 - Warning: There are some scripts that I use via cdn and unpkg so that this project need internet connection to run correctly


# Description
- The main purpose is only for training. Not for work or eCommerce.
- The idea is based on [Udemy](https://www.udemy.com/)'s Design.
- Submit to TTTH KHTN.

# Technologies
1. ASP.NET 5
2. Entity Framework Core
3. HTML/CSS/JS and JQuery

# Contact via if something is wrong
- [Facebook](https://facebook.com/mavisphung43)
- Phone number: `0349797318`
