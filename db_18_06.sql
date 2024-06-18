CREATE DATABASE BookStore;

USE BookStore;

CREATE TABLE User
(
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    login_name VARCHAR(50) NOT NULL,
    login_password VARCHAR(50) NOT NULL,
    user_name VARCHAR(100) NOT NULL,
    phone VARCHAR(20),
    address VARCHAR(255)
);

CREATE TABLE Categories
(
    category_id INT AUTO_INCREMENT PRIMARY KEY,
    category_name VARCHAR(100) NOT NULL
);

CREATE TABLE Author
(
    author_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL
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
    product_des TEXT,
    FOREIGN KEY (category_id) REFERENCES Categories(category_id),
    FOREIGN KEY (author_id) REFERENCES Author(author_id)
);

CREATE TABLE Orders
(
    order_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT,
    order_date DATE NOT NULL,
    FOREIGN KEY (user_id) REFERENCES User(user_id)
);

CREATE TABLE OrderDetails
(
    order_detail_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT,
    isbn VARCHAR(20),
    quantity INT NOT NULL,
    unit_price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (isbn) REFERENCES Book(isbn)
);


INSERT INTO User (login_name, login_password, user_name, phone, address)
VALUES 
('john', '123', 'John Doe', '1234567890', '123 Elm Street'),
('jane', '123', 'Jane Smith', '0987654321', '456 Oak Avenue');


INSERT INTO Categories (category_name)
VALUES 
('Fiction'),
('Non-fiction'),
('Science Fiction'),
('Biography');

INSERT INTO Author (name)
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

INSERT INTO Orders (user_id, order_date)
VALUES 
(1, '2024-06-15'),
(2, '2024-06-16');

INSERT INTO OrderDetails (order_id, isbn, quantity, unit_price)
VALUES 
(1, 'book-01', 2, 19.99),
(1, 'book-02', 1, 14.99),
(2, 'book-03', 1, 29.99),
(2, 'book-04', 1, 24.99);