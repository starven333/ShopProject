use bookstore;

INSERT INTO Users (login_name, login_password, user_name, phone, address, user_role)
VALUES 
('john', '123', 'John Doe', '1234567890', '123 Elm Street', 0),
('jane', '123', 'Jane Smith', '0987654321', '456 Oak Avenue', 0),
('admin1', '123', 'Jake Pear', '0987123321', '41 Oasis Street', 1);

INSERT INTO categories (category_id, category_name) VALUES
(1, 'Fiction'),
(2, 'Non-Fiction'),
(3, 'Science'),
(4, 'History'),
(5, 'Biography'),
(6, 'Fantasy'),
(7, 'Romance'),
(8, 'Thriller'),
(9, 'Mystery'),
(10, 'Self-Help');


INSERT INTO author (author_id, author_name) VALUES
(1, 'Author A'),
(2, 'Author B'),
(3, 'Author C'),
(4, 'Author D'),
(5, 'Author E'),
(6, 'Author F'),
(7, 'Author G'),
(8, 'Author H'),
(9, 'Author I'),
(10, 'Author J'),
(11, 'Author K'),
(12, 'Author L'),
(13, 'Author M'),
(14, 'Author N'),
(15, 'Author O');


INSERT INTO Book (isbn, category_id, author_id, book_name, release_date, price, quantity, product_des) VALUES
('978-3-16-148410-0', 1, 1, 'The Great Fiction', '2022-01-15', 15.99, 10, 'A captivating fiction book.'),
('978-1-40-289462-3', 2, 2, 'Non-Fiction Tales', '2021-03-20', 12.99, 15, 'An insightful non-fiction book.'),
('978-0-59-313913-4', 3, 3, 'Science Wonders', '2023-05-10', 18.50, 8, 'Exploring the wonders of science.'),
('978-1-50-113592-7', 4, 4, 'Historical Events', '2020-07-25', 20.00, 5, 'A journey through significant historical events.'),
('978-1-984879-13-7', 5, 5, 'Biography of a Legend', '2019-11-30', 22.75, 12, 'The life story of a legendary figure.'),
('978-0-7432-7350-4', 6, 6, 'Fantasy World', '2018-09-05', 16.99, 7, 'Dive into a magical fantasy world.'),
('978-1-4197-3433-0', 7, 7, 'Romantic Escapades', '2022-02-14', 14.99, 20, 'A heartwarming romance novel.'),
('978-1-59327-584-6', 8, 8, 'Thrilling Adventures', '2021-08-30', 17.50, 9, 'A book full of thrilling adventures.'),
('978-1-250-30170-3', 9, 9, 'Mystery Unfolds', '2023-06-20', 19.99, 6, 'A mystery that keeps you on the edge.'),
('978-1-5387-2197-3', 10, 10, 'Self-Help Guide', '2020-04-10', 13.50, 25, 'A guide to personal improvement.'),
('978-1-61695-317-7', 1, 11, 'Another Great Fiction', '2021-12-01', 15.99, 10, 'Another captivating fiction book.'),
('978-0-06-245779-0', 2, 12, 'More Non-Fiction Tales', '2020-06-22', 12.99, 15, 'Another insightful non-fiction book.'),
('978-0-14-312547-1', 3, 13, 'More Science Wonders', '2022-11-05', 18.50, 8, 'More wonders of science explored.'),
('978-1-5011-8057-4', 4, 14, 'More Historical Events', '2019-02-18', 20.00, 5, 'More significant historical events.'),
('978-0-553-39093-1', 5, 15, 'Another Biography of a Legend', '2018-07-09', 22.75, 12, 'Another life story of a legendary figure.'),
('978-1-4736-7626-5', 6, 1, 'More Fantasy World', '2021-01-03', 16.99, 7, 'More magical fantasy world adventures.'),
('978-1-4091-9455-6', 7, 2, 'More Romantic Escapades', '2020-11-11', 14.99, 20, 'Another heartwarming romance novel.'),
('978-0-099-57378-2', 8, 3, 'More Thrilling Adventures', '2023-03-15', 17.50, 9, 'More thrilling adventures to enjoy.'),
('978-0-553-87268-7', 9, 4, 'Another Mystery Unfolds', '2022-09-27', 19.99, 6, 'Another mystery that keeps you on the edge.'),
('978-1-250-03167-9', 10, 5, 'Another Self-Help Guide', '2019-10-20', 13.50, 25, 'Another guide to personal improvement.'),
('978-1-250-08134-6', 1, 6, 'Yet Another Great Fiction', '2018-05-10', 15.99, 10, 'Yet another captivating fiction book.'),
('978-0-399-59475-3', 2, 7, 'Yet More Non-Fiction Tales', '2020-07-19', 12.99, 15, 'Yet another insightful non-fiction book.'),
('978-1-479-61723-0', 3, 8, 'Yet More Science Wonders', '2021-12-09', 18.50, 8, 'Yet more wonders of science explored.'),
('978-1-250-02365-4', 4, 9, 'Yet More Historical Events', '2022-01-25', 20.00, 5, 'Yet more significant historical events.'),
('978-1-250-16289-0', 5, 10, 'Yet Another Biography of a Legend', '2023-05-07', 22.75, 12, 'Yet another life story of a legendary figure.'),
('978-0-759-87356-0', 6, 11, 'Yet More Fantasy World', '2021-08-02', 16.99, 7, 'Yet more magical fantasy world adventures.'),
('978-1-250-21121-6', 7, 12, 'Yet More Romantic Escapades', '2020-03-14', 14.99, 20, 'Yet another heartwarming romance novel.'),
('978-0-385-54102-5', 8, 13, 'Yet More Thrilling Adventures', '2023-02-18', 17.50, 9, 'Yet more thrilling adventures to enjoy.'),
('978-1-250-20214-1', 9, 14, 'Yet Another Mystery Unfolds', '2019-11-03', 19.99, 6, 'Yet another mystery that keeps you on the edge.'),
('978-1-250-13416-1', 10, 15, 'Yet Another Self-Help Guide', '2018-06-26', 13.50, 25, 'Yet another guide to personal improvement.'),
('978-0-399-59013-7', 1, 1, 'Final Great Fiction', '2020-08-18', 15.99, 10, 'The final captivating fiction book.'),
('978-0-307-26377-6', 2, 2, 'Final Non-Fiction Tales', '2021-05-12', 12.99, 15, 'The final insightful non-fiction book.'),
('978-0-399-58982-8', 3, 3, 'Final Science Wonders', '2022-04-17', 18.50, 8, 'The final wonders of science explored.'),
('978-0-06-231607-4', 4, 4, 'Final Historical Events', '2023-07-22', 20.00, 5, 'The final significant historical events.'),
('978-0-307-36169-2', 5, 5, 'Final Biography of a Legend', '2019-09-30', 22.75, 12, 'The final life story of a legendary figure.'),
('978-1-5011-9624-7', 6, 6, 'Final Fantasy World', '2018-12-25', 16.99, 7, 'The final magical fantasy world adventure.'),
('978-1-250-21706-4', 7, 7, 'Final Romantic Escapades', '2020-10-15', 14.99, 20, 'The final heartwarming romance novel.'),
('978-0-7653-9938-4', 8, 8, 'Final Thrilling Adventures', '2021-02-20', 17.50, 9, 'The final thrilling adventures to enjoy.'),
('978-0-399-59074-6', 9, 9, 'Final Mystery Unfolds', '2022-11-05', 19.99, 6, 'The final mystery that keeps you on the edge.'),
('978-0-307-34164-2', 10, 10, 'Final Self-Help Guide', '2019-12-14', 13.50, 25, 'The final guide to personal improvement.'),
('978-0-385-51732-3', 1, 11, 'Ultimate Great Fiction', '2021-01-19', 15.99, 10, 'The ultimate captivating fiction book.'),
('978-0-375-71060-6', 2, 12, 'Ultimate Non-Fiction Tales', '2020-09-08', 12.99, 15, 'The ultimate insightful non-fiction book.'),
('978-0-7653-6455-6', 3, 13, 'Ultimate Science Wonders', '2023-03-25', 18.50, 8, 'The ultimate wonders of science explored.'),
('978-0-399-59077-7', 4, 14, 'Ultimate Historical Events', '2022-06-17', 20.00, 5, 'The ultimate significant historical events.'),
('978-1-4767-2745-3', 5, 15, 'Ultimate Biography of a Legend', '2018-10-10', 22.75, 12, 'The ultimate life story of a legendary figure.');