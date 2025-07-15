# Local Development Environment Setup Guide

## 📧 SMTP4DEV Setup (Version 3.1.4)

I use **smtp4dev v3.1.4** for local email testing (e.g., email confirmations, order notifications, cancellations).

### Download Options:
1. **Direct Download**:
   - Navigate to [smtp4dev v3.1.4 release page](https://github.com/rnwood/smtp4dev/releases?page=4)
   - Download the appropriate binary:
     - **Windows**: `smtp4dev-win-x64.zip`
     - **Linux/macOS**: Select relevant binary (e.g., `smtp4dev-linux-x64.zip`, `smtp4dev-osx-x64.zip`)

2. **NuGet Installation**:
   ```bash
   dotnet tool install -g Rnwood.Smtp4dev --version 3.1.4

### Running SMTP4DEV

After downloading and extracting (or installing via NuGet), run `smtp4dev` from your terminal or by executing the downloaded binary. It will typically open a web interface in your browser (usually at `http://localhost:5000` or `http://localhost:5001`).

## 🔑 API Key for Geocoding Service

Our application utilizes a geocoding service that requires an API key. Please follow these steps to obtain and configure your API key:

1.  **Obtain an API Key:**
    * Go to [https://geocode.maps.co/](https://geocode.maps.co/).
    * Follow the instructions on the website to register and obtain your personal API key.

2.  **Configure `appsettings.json`:**
    * Locate the `appsettings.json` file in the project's root directory (MarketCatalogue).
    * Replace `"ApiKey"` with the actual API key you obtained from `geocode.maps.co/`.

    Example `appsettings.json` snippet:

    ```json
    {
      "GeocodingService": {
        "BaseUrl": "[https://geocode.maps.co/api/v1/](https://geocode.maps.co/api/v1/)",
        "ApiKey": "YOUR_ACTUAL_API_KEY_HERE" // Replace "YOUR_ACTUAL_API_KEY_HERE"
      },
      // ... other settings
    }
    ```

## 🗄️ Database Setup: Restoring a Bacpac File

To set up your local database, you will need to restore a Bacpac file using SQL Server Management Studio (SSMS) or a similar tool.

### Prerequisites

* Ensure you have SQL Server (Express, Developer, or Enterprise edition) installed on your local machine.
* Download and install SQL Server Management Studio (SSMS).

### Obtain the Bacpac File

The Bacpac file (e.g., `YourDatabaseName.bacpac`) should be provided separately. Place it in an easily accessible location on your machine.

### Restore the Bacpac in SSMS

1. Open **SQL Server Management Studio (SSMS)**.
2. Connect to your SQL Server instance.
3. In the **Object Explorer**, right-click on **Databases**.
4. Select **Import Data-tier Application...**
5. Follow the **Import Data-tier Application Wizard**:
    - **Introduction:** Click **Next**.
    - **Import Settings:**
      - Click **Browse...** next to "Import from local disk" and navigate to the `.bacpac` file.
      - Click **Next**.
    - **Database Settings:**
      - Specify a new database name (e.g., `YourApplicationDB_Dev`).
      - Click **Next**.
    - **Summary:** Review the settings.
    - Click **Finish** to start the import process.

### Verify Restoration

* Once the import is complete, refresh the **Databases** node in SSMS Object Explorer.
* You should see the newly restored database listed.

### Update Connection String

After restoring the database, open the `appsettings.ConnectionString.json` file in the project and substitute the connection strings with the correct credentials and database name.

Example with SQL authentication:

```json
{
  "ConnectionStrings": {
    "AuthenticationDbContext": "Server=YOUR_SQL_SERVER_INSTANCE;Database=YourApplicationDB_Dev;User ID=YOUR_USERNAME;Password=YOUR_PASSWORD;MultipleActiveResultSets=true",
    "CommerceDbContext": "Server=YOUR_SQL_SERVER_INSTANCE;Database=YourApplicationDB_Dev;User ID=YOUR_USERNAME;Password=YOUR_PASSWORD;MultipleActiveResultSets=true"
  }
}
