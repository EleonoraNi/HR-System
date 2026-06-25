# HR Management Application

A desktop HR system for managing employee and employment data. The application allows users to add, edit, and delete employee records, as well as manage multiple employment relationships.

## 📅 Background

This project is my first larger software application, developed in 2023 as part of my studies. It represents the starting point of my journey in software development and demonstrates my understanding of core programming concepts.

## 🔧 Features

- User login functionality
- Add, edit, and delete employees
- Support for multiple employment periods per employee
- Input validation for form fields
- Sorting by last name, preferred name, and job title
- Logging of user actions
- File-based data storage
- Data encryption

## 🛠️ Technologies

- C#
- .NET
- XAML
- File-based storage

## 🧠 Implementation

The application consists of a user interface and supporting classes responsible for data handling, validation, and security.

Key components include:

- `Employee.cs` – data model for employee and employment information
- `FileHelper.cs` – handles saving and loading data
- `Encryption.cs` & `EncryptionHelper.cs` – data encryption and decryption
- `SSNCheck.cs` – validation for personal identity numbers
- UI forms with real-time input validation

## 📸 Screenshots

## 📸 Screenshots

### Login view
![Login](images/login.png)

### Main view
![Main](images/start-view.png)

### Employee form
![Form](images/form.png)


## 🚀 Future Improvements

- Improve user interface and usability
- Replace file-based storage with a database (e.g., SQL)
- Enhance date handling (e.g., DatePicker support)
- Extend user management features

## 🎯 What I Learned

During this project, I learned:

- Combining UI and application logic
- Input validation techniques
- File handling and serialization
- Structuring a software project
- Debugging and problem-solving in practice

## 👤 Author

Eleonora Niskanen
