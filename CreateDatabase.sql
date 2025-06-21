-- Tạo database
CREATE DATABASE MyStoreDB;
GO

USE MyStoreDB;
GO

-- Bảng AccountMember
CREATE TABLE AccountMember (
    MemberID INT IDENTITY(1,1) PRIMARY KEY,
    MemberPassword NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100),
    EmailAddress NVARCHAR(100),
    MemberRole NVARCHAR(50)
);

-- Bảng Categories
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL
);

-- Bảng Products
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    CategoryID INT,
    UnitsInStock INT,
    UnitPrice DECIMAL(10, 2),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Insert sample data
INSERT INTO Categories (CategoryName) VALUES 
('Electronics'), ('Books'), ('Clothing');

INSERT INTO Products (ProductName, CategoryID, UnitsInStock, UnitPrice) VALUES 
('Laptop', 1, 10, 1500.00),
('Smartphone', 1, 20, 800.00),
('Programming Book', 2, 15, 50.00);

INSERT INTO AccountMember (MemberPassword, FullName, EmailAddress, MemberRole) VALUES 
('admin123', 'Administrator', 'admin@mystore.com', 'Admin'),
('user123', 'John Doe', 'john@email.com', 'User');