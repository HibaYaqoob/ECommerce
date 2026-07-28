-- 1. Create the Database
CREATE DATABASE ECommerceDB;
GO

-- 2. Switch to the newly created database
USE ECommerceDB;
GO

-- 3. Create USER Table
CREATE TABLE [User] (
    User_Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL
);

-- 4. Create CATEGORY Table
CREATE TABLE Category (
    Category_Id INT IDENTITY(1,1) PRIMARY KEY,
    Category_Name NVARCHAR(100) NOT NULL
);

-- 5. Create PRODUCT Table (1:N with Category)
CREATE TABLE Product (
    Product_Id INT IDENTITY(1,1) PRIMARY KEY,
    Product_Name NVARCHAR(150) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Category_Id INT NOT NULL,
    CONSTRAINT FK_Product_Category FOREIGN KEY (Category_Id) 
        REFERENCES Category(Category_Id) ON DELETE CASCADE
);

-- 6. Create ORDER Table (1:N with User)
CREATE TABLE [Order] (
    Order_Id INT IDENTITY(1,1) PRIMARY KEY,
    Order_Date DATETIME NOT NULL DEFAULT GETDATE(),
    User_Id INT NOT NULL,
    CONSTRAINT FK_Order_User FOREIGN KEY (User_Id) 
        REFERENCES [User](User_Id) ON DELETE CASCADE
);

-- 7. Create ORDERPRODUCT Join Table (Explicit M:N with Quantity)
CREATE TABLE OrderProduct (
    Order_Id INT NOT NULL,
    Product_Id INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    CONSTRAINT PK_OrderProduct PRIMARY KEY (Order_Id, Product_Id),
    CONSTRAINT FK_OrderProduct_Order FOREIGN KEY (Order_Id) 
        REFERENCES [Order](Order_Id) ON DELETE CASCADE,
    CONSTRAINT FK_OrderProduct_Product FOREIGN KEY (Product_Id) 
        REFERENCES Product(Product_Id) ON DELETE CASCADE
);

-- 8. Create REVIEW Table (1:1 with Order via UNIQUE Foreign Key)
CREATE TABLE Review (
    Review_Id INT IDENTITY(1,1) PRIMARY KEY,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(MAX) NULL,
    Order_Id INT NOT NULL UNIQUE, -- Enforces the 1:1 relationship
    CONSTRAINT FK_Review_Order FOREIGN KEY (Order_Id) 
        REFERENCES [Order](Order_Id) ON DELETE CASCADE
);