# 📚 Library Management System

A C# Windows Forms desktop application for managing library operations, built as a semester project for **CSL 113 – Computer Programming** at Bahria University Karachi Campus.

---

## 📋 Overview

The Library Management System (LMS) replaces traditional paper-based library operations with a digital platform. It provides role-based access for administrators and regular members, covering everything from book cataloguing and user registration to borrowing, purchasing, and return tracking — all backed by a MySQL database.

---

## ✨ Features

### 🔐 Authentication
- Credential-based login using first name and password
- Role detection: automatically routes to Admin or Member dashboard on login
- Blocked user detection with informative messaging

### 📖 Book Management — Admin
- Add new books with ISBN, title, author, genre, location, price, and copy count
- Edit existing book records via a searchable DataGridView (search by Title, ISBN, Author, Genre, Location, or Price)
- Delete books with transactional safety — associated borrow requests are removed first to maintain referential integrity

### 🔍 Book Search — Member
- Search the full catalogue by Title, ISBN, Author, Genre, Location, or Price
- Results displayed in a DataGridView with auto-sized columns

### 🔄 Transaction Management — Member
- **Borrow Books:** Select from dropdown, view details (title, author, genre, copies available), set issue and due dates
- **Purchase Books:** Same flow as borrowing with a "Buy" request type
- Stock check before issuing — prevents borrowing out-of-stock books, decrements copy count on issue
- **Borrowed Books View:** Displays all active borrows for the logged-in user; overdue rows highlighted in red

### 🛠️ Admin Panel
- **User Settings:** Add new users (with admin permission and block toggles); edit or delete existing users via searchable grid
- **Request Management:** View all pending borrows and full borrow/return history; process returns (marks request as "Returned" and increments book copy count); search within pending and history tables independently

### 🗄️ Database
- MySQL backend with parameterized queries throughout to prevent SQL injection
- Transactional deletes to maintain data integrity

---

## 🛠️ Tech Stack

| | |
|-|---|
| Language | C# |
| Framework | .NET Framework — Windows Forms |
| Database | MySQL (via MySQL Connector/NET) |
| IDE | Visual Studio |

---

## 🗄️ Database Setup

1. Install **MySQL Server** and create a database named `librarydb`.
2. Create the following tables:

```sql
CREATE TABLE Users (
    ID          INT AUTO_INCREMENT PRIMARY KEY,
    FirstName   VARCHAR(50),
    LastName    VARCHAR(50),
    Password    VARCHAR(100),
    AdminPerm   VARCHAR(5)  DEFAULT 'false',
    IsBlocked   VARCHAR(5)  DEFAULT 'false',
    Phone       VARCHAR(20),
    Address     VARCHAR(100),
    Email       VARCHAR(100)
);

CREATE TABLE Books (
    BookID      INT AUTO_INCREMENT PRIMARY KEY,
    ISBN        VARCHAR(20),
    Title       VARCHAR(150),
    Author      VARCHAR(100),
    Location    VARCHAR(50),
    NoOfCopies  INT,
    Price       DECIMAL(10,2),
    Genre       VARCHAR(50)
);

CREATE TABLE Requests (
    RequestID   INT AUTO_INCREMENT PRIMARY KEY,
    BookID      INT,
    ISBN        VARCHAR(20),
    Title       VARCHAR(150),
    UserID      INT,
    DateOfIssue DATE,
    DateOfReturn DATE,
    Type        VARCHAR(20)
);

-- Default admin account
INSERT INTO Users (FirstName, LastName, Password, AdminPerm, IsBlocked, Phone, Address, Email)
VALUES ('admin', 'admin', 'admin123', 'true', 'false', '', '', 'admin@library.com');
```

3. Update the connection string in each form file to match your MySQL credentials:

```csharp
string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=YOUR_PASSWORD;";
```

---

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7+
- MySQL Server 8.x
- MySQL Connector/NET (NuGet package: `MySql.Data`)

### Running the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/ShayanShakeel/LibraryManagementSystem.git
   ```
2. Open `LibrarySystem.sln` in Visual Studio.
3. Restore NuGet packages (MySQL Connector/NET is in the `packages/` folder).
4. Set up the database as described above and update connection strings.
5. Build and run — the application starts at the Login form (`hello.cs`).

---

## 📁 Project Structure

```
LibraryManagementSystem/
│
├── LibrarySystem.sln
│
└── LibrarySystem/
    ├── hello.cs/.Designer.cs         # Login form
    ├── Main.cs/.Designer.cs          # Member main menu
    ├── AdminPage.cs/.Designer.cs     # Admin dashboard
    │
    ├── Books.cs/.Designer.cs         # Borrow/purchase — step 1 (book selection)
    ├── Requests.cs/.Designer.cs      # Borrow/purchase — step 2 (confirm request)
    ├── SearchBooks.cs/.Designer.cs   # Book search for members
    ├── Borrowed.cs/.Designer.cs      # View active borrows + overdue highlighting
    │
    └── Forms/
        └── Admin/
            ├── BooksSettings.cs      # Book management hub
            ├── AddNewBook.cs         # Add book form
            ├── editingbooks.cs       # Edit/delete books
            ├── requestSettings.cs    # Request/return management
            └── UserSettings/
                ├── UserEdittingOptions.cs  # User management hub
                ├── AddNewUser.cs           # Add user form
                └── editingusers.cs         # Edit/delete users
```

---

## 🔮 Future Improvements

- Password hashing (currently stored in plaintext)
- Mobile app integration
- AI-driven book recommendations
- Fine calculation for overdue books
- Email notifications for due dates

---

## 📄 References

- Microsoft — [Windows Forms Overview](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/)
- MySQL — [MySQL 8.0 Reference Manual](https://dev.mysql.com/doc/refman/8.0/en/)
- W3Schools — [SQL Tutorial](https://www.w3schools.com/sql/)
- W3Schools — [C# Tutorial](https://www.w3schools.com/cs/)

---

*Course: CSL 113 – Computer Programming · Bahria University Karachi Campus · BSE-1A Fall 2024*
