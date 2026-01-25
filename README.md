# ShoeBoard

A web app for shoe colectors.
GitHub repo: https://github.com/pikkoq/PracaInzynierska

## Project Structure


`ShoeBoardAPI/` - Backend REST API built with .NET (C#)
`shoe-board-web/` - Frontend React application
`ShoeBoardDataBase` - SQL Server database backup file (.bak) - restore this to set up the database
`LoginyTestowe.txt` - Test user credentials (username, email, password) - use these to populate the database with sample users for testing purposes

---



### Prerequisites

- **Backend:** .NET 8.0+ SDK, SQL Server
- **Frontend:** Node.js 18+, npm

---

### 1. Database Setup

1. Open SQL Server Management Studio (SSMS)
2. Right-click on "Databases" → "Restore Database..."
3. Select "Device" and browse to `ShoeBoardDataBase` file
4. Restore the database

---

### 2. Running the Backend (ShoeBoardAPI)

```bash
cd ShoeBoardAPI
dotnet restore
dotnet run
```

The API will start at `https://localhost:7117`

---

### 3. Running the Frontend (shoe-board-web)

```bash
cd shoe-board-web
npm install
npm start
```

The app will open at `http://localhost:3000`

---

## 📝 Test Users

The `LoginyTestowe.txt` file contains credentials for 19 pre-created test users. You can use these to log in and test the application without registering new accounts.

Example:
- **Username:** SneakerHead89
- **Email:** sneakerhead89@example.com
- **Password:** Kick$King89

---

## 🎨 Credits

### Icons

Some icons used in this project are provided by [Flaticon](https://www.flaticon.com)

---

## 📄 License

This project was created as an engineering thesis (Praca Inżynierska).
