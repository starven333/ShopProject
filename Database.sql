drop database bookstore;
create database bookstore;

USE BookStore;

CREATE TABLE Users
(
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    login_name VARCHAR(50) NOT NULL,
    login_password VARCHAR(50) NOT NULL,
    user_name VARCHAR(100) NOT NULL,
    phone VARCHAR(20),
    address VARCHAR(255),
    user_role int default 0,
    user_status int default 1
);

CREATE TABLE Categories
(
    category_id INT AUTO_INCREMENT PRIMARY KEY,
    category_name VARCHAR(100) NOT NULL
);

CREATE TABLE Author
(
    author_id INT AUTO_INCREMENT PRIMARY KEY,
    author_name VARCHAR(100) NOT NULL
);

CREATE TABLE Book
(
    isbn VARCHAR(20) PRIMARY KEY,
    category_id INT,
    author_id INT,
    book_name VARCHAR(255) NOT NULL,
    release_date DATE,
    price DECIMAL(10, 2),
    quantity INT,
    product_des TEXT default ('No Des'),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id),
    FOREIGN KEY (author_id) REFERENCES Author(author_id)
);

CREATE TABLE OrderDetails
(
    order_detail_id INT AUTO_INCREMENT PRIMARY KEY,
	user_id INT,
    order_date DATETIME default current_timestamp(),
    isbn VARCHAR(20),
    quantity INT NOT NULL,
    total_price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (isbn) REFERENCES Book(isbn)
);

INSERT INTO Users (login_name, login_password, user_name, phone, address, user_role)
VALUES 
('john', '123', 'John Doe', '1234567890', '123 Elm Street', 0),
('jane', '123', 'Jane Smith', '0987654321', '456 Oak Avenue', 0),
('admin1', '123', 'Jake Pear', '0987123321', '41 Oasis Street', 1);

INSERT INTO Categories (category_name)
VALUES 
('Fiction'),
('Non-fiction'),
('Science Fiction'),
('Biography');

INSERT INTO Author (author_name)
VALUES 
('J.K. Rowling'),
('George R.R. Martin'),
('Isaac Asimov'),
('Walter Isaacson');

INSERT INTO Book (isbn, category_id, author_id, book_name, release_date, price, quantity, product_des)
VALUES 
('book-01', 1, 1, 'Harry Potter and the Philosopher\'s Stone', '1997-06-26', 19.99, 50, 'A young boy discovers he is a wizard on his 11th birthday.'),
('book-02', 3, 2, 'A Game of Thrones', '1996-08-06', 29.99, 35, 'The first book in the Song of Ice and Fire series.'),
('book-03', 3, 3, 'Foundation', '1951-05-01', 14.99, 40, 'A science fiction novel about the fall and rise of a Galactic Empire.'),
('book-04', 4, 4, 'Steve Jobs', '2011-10-24', 24.99, 20, 'A biography of Steve Jobs, the co-founder of Apple Inc.');


INSERT INTO OrderDetails (user_id, isbn, quantity, total_price)
VALUES 
(1, 'book-01', 2, 2*19.99),
(1, 'book-02', 1, 14.99),
(2, 'book-03', 1, 29.99),
(2, 'book-04', 1, 24.99);