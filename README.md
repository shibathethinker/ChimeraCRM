# ChimeraCRM
Open source CRM

ChimeraCRM is a web-based enterprise grade CRM. This project is created using C#.NET and can run on any SQL database. This CRM provides some additional features which standard CRM vendors don't provide with their off-the shelf version.
This was a home project built over leisure time with the goal in mind to make a CRM product which offers all standard features of a CRM and also adds new flavours. The CRM in its current version does not offer any API, which is planned to be released while working with Apache community. 
Key features and differentiators of this CRM product are – 
 Standard CRM features like - lead and potential details maintenance, defect tracking, dashboards, contact and account details, reporting etc. 
The CRM can be used in "online" or "offline" mode. 
In an Offline mode, the CRM is used by other standard CRMs where one organization keeps all its details by itself.
 In an Online mode, the CRM can be used like a marketplace. In this mode, the organization hosting the CRM can invite other organizations to join the CRM. Multiple partner organizations can use this system as their single CRM "bus". In this mode the CRM can be used for - 1 Tendering 2 Short listing Vendors 3 Finalizing Vendors 4 Issuing Purchase Order 5 Issuing Sales Order 6 Invoicing 7 Defect tracking
 
 To start using this project, replicate the project on your desktop. The main user -interface related components of the project is in the Online Folder. This folder has a C# solution file which should open in Visual studio. In this solution (Online), there are references to other components used in this project - BackendObjects, ActionLibrary and CustomExceptions.
 Backendobject is the database interaction layer whose only job is to to talk to the underlying database. Actionlibrary implments some custom classes/functions to be used across the entire project. Customexception is what its name signifies, custome exception classes.
 To create the database model, use the database snapshot I have created in the DBBackups folder. This snaphost can be directly restored into a SQL server instance. While restoring use the database name as 'BEOBJ'. You can ofcourse, change the database connection settings in the Web.Config file present in the Online solution.

If you have successfully configured everything and restored the database which I have provided, then you can launch the Online solution from Visual studio. Upon launch this will take you to the login page, here you can login using the userid: 

