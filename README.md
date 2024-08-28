# Windows Sensors Database Service

## Overview

This project is a Windows Service designed to collect sensor data from various sources on a Windows PC and store it in a database. It runs in the background, ensuring continuous data collection even when the user is not logged in.

## Features

- **Automatic Data Collection:** Collects sensor data at regular intervals.
- **Database Integration:** Stores collected data in a SQL Server database.
- **Customizable Configuration:** Users can modify settings such as data collection frequency and database connection details.

## Installation

### Pre-requisites

- Windows operating system
- .NET Framework 4.7.2 or later
- Microsoft SQL Server

### Configuration

1. **Create the Configuration File**

   Before installing the service, create a configuration file named `Config.xml` in `C:\WindowsSensorsDbService` with the following content (replace placeholders with actual database and server details):

   ```xml
   <?xml version="1.0" encoding="utf-8" standalone="yes"?>
   <config>
       <settings>
           <setting name="db_server" value="" />
           <setting name="db_name" value="WindowsSensorsDbService" />

           <!-- The user must already exist and must have sufficient permissions -->
           <setting name="db_user" value="" />
           <setting name="db_password" value="" />
   
           <setting name="computer_name" value="" />
           <setting name="logging_interval_seconds" value="60" />
       </settings>
   </config>
   ```
   
### Installation Steps

1. **Download the Pre-compiled Executable**
   - Obtain the pre-compiled `.exe` file from the project repository.

2. **Open Command Prompt as Administrator**
   - Right-click on **Command Prompt** in the Start menu.
   - Select **Run as administrator**.

3. **Navigate to the Directory Containing the Executable**
   ```sh
   cd C:\Path\To\Directory\Containing\WindowsSensorsDbService.exe
   ```

4. **Install the Service**
   - Execute the following command to install the Windows Service:
   ```sh
   "C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" WindowsSensorsDbService.exe
   ```
   
5. **Verify Installation**
   - Open **Services** (press **Win + R**, type `services.msc`, and press **Enter**).
   - Look for `WindowsSensorsDbService` in the list of services.
   - Ensure the service status is **Running**.

## Usage

- Once installed and configured, the service will automatically collect sensor data based on the configured settings.
