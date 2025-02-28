# CS3502 - Operating Systems 

## Class Project 1: Multi-Threading Programming, Synchronization, and Deadlock Management

### Overview
This project explores multithreaded programming and interprocess communication. The multithreading component simulates a banking system that demonstrates concurrency through transactions on a single account as well as transactions between mulitple accounts. It includes examples of thread management, synchronization techniques for handling race conditions, and implements deadlock detection and resolution.


This project can be found within :
```
-- Account/           # Main Program
-- AccountTesting     #Unit Tests
```
Additionally, the project implements an interprocess communication system, where a process reads files from a folder and transmits data through pipes to be read and then displayed to the console.

This project can be found within: 
```
-- InterprocessCommunications/    # main program
```

### Setup
The program is built using C#

To run the program locally, fork this repo. Open the root directory in a IDE that supports C# and run ```dotnet build``` to initiate the program
