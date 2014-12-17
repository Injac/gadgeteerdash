gadgeteerdash
=============

Simple sample how to user Gadgeteer, Web-API and  SignalR to create a simple dashboard using a free Azure-Website.

Solution Structure
==================

There are two projects within this solution:

* Gadgeteer Sender - The Gadgeteer app for the Fez Spider 1.0 mainboard (NETMF 4.3)
* iotdash - Simple Web-API project that contains the Web-API that is called from FEZ Spider and a simple page with a Gauge and a div that contains the current time on the board

The **Index.cshtml** view, that contains the "dashboard" can be found within the *iotdash* project within the folder **/Views/Home/index.cshtml**. It contains the client-side Javascript that is used to listen to requests on the SignalR pipeline and to modify the Gauge value as well as the div that contains the actual time of the FEZ Spider board.

The SignalR-Hub that conatins the backend "logic" (LOL) can be found in the project-root of the **iotdash** project and is called **GadgeteerHub.cs** - it contains the two SignalR-Methods that are invoked from the Web-API.

NETMF on the Spider simply calls the two Web-API get actions defined within the **/iotdash/Controller/ValuesController.cs** file. Both methods simply call the pre-defined client-side "hello" or "systemInfo" methods on the hub.
