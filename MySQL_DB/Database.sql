drop database bookstore;
create database bookstore;

USE BookStore;

CREATE TABLE Users
(
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    login_name VARCHAR(50) NOT NULL UNIQUE,
    login_password VARCHAR(50) NOT NULL,
    user_name VARCHAR(100) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    address VARCHAR(255) NOT NULL,
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
    release_date DATE NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    quantity INT NOT NULL,
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


