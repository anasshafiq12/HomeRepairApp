--PROJECT OVERVIEW

--The HomeRepairApp is an ASP.NET web application designed to facilitate home repair services. It aims to connect homeowners with professional repair service providers, streamlining the process of booking and managing repair tasks. Also users can buy electronic appliances for their home.

--Features
    Security: Idenity Framework by Microsoft
    User Authentication: Secure login and registration for users.
    Service Listings: Browse and search for available repair services.
    Booking System: Schedule and manage repair appointments.
    Profile Management: Users can update their personal information and view booking history.
    Admin Panel: For management of items, processing of orders, bookings etc.

--Tech Stack
    Frontend: HTML, CSS, JavaScript
    Backend: ASP.NET with C#
    Database: Entity Framework 

--Installation & Setup
    Clone the Repository:
    git clone https://github.com/anasshafiq12/HomeRepairApp.git
    cd HomeRepairApp

--Open in Visual Studio: Open the HouseRepairApp.sln solution file in Visual Studio.
Restore NuGet Packages: Visual Studio should automatically restore the necessary NuGet packages. If not, go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution and restore them manually.

--Update Database: Ensure that the database connection string in appsettings.json is configured correctly. Then, run the following commands in the Package Manager Console:
    Update-Database
    Run the Application: Press F5 or click on the Start button in Visual Studio to run the application.

--How It Works
Upon launching the application, users can register or log in to their accounts. Authenticated users can browse through a list of available home repair services, view detailed information, and book appointments. The application provides a user-friendly interface to manage bookings and view service history.

--Future Enhancements
    Service Provider Dashboard: Implement a dashboard for service providers to manage their services and view bookings.
    Payment Integration: Integrate online payment gateways for seamless transactions.
    Rating & Reviews: Allow users to rate and review services to enhance trust and service quality.
