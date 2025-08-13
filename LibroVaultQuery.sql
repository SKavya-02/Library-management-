USE LibroVaultDB;

--ADMIN

INSERT INTO Admins(FullName, Email, Password, IsDeleted)
VALUES 
('Subha', 'subhaadmin@gmail.com', 'susiSK3143!', 0),
('Sakthi', 'sakthi@gmail.com', 'KannanSK5656', 0);

--USER

INSERT INTO Users
(FullName, Email, Password, Gender, Address, Phone, ProfilePicture, IsDeleted)
VALUES 
('Kavya', 'kavyauser@gmail.com', 'KavyaSK@123', 'Female', 'Madurai', '9876543210', 'kavya_profile.jpg', 0),
('Karthi', 'karthiuser@gmail.com', 'KarthiSK@55', 'Male', 'Madurai', '9123456780', 'karthi_profile.jpg', 0);

--CATEGORY

INSERT INTO Categories(Name, IsDeleted)
VALUES 
('Romantic Thriller', 0),  
('Science Fiction', 0);    

--BOOK

INSERT INTO Books 
(Title, Author, ISBN, Publisher, PublicationDate, Edition, Language, NumberOfPages, Description, Cost, ImageUrl, IsDeleted, CategoryId)
VALUES 
('Whispers of the Night', 'Aarav', '9781234567890', 'Dreamscape Publishing', '2021-08-15', '1st', 'English', 320, 
 'A haunting dark romance that pulls you into a world of secrets and passion.', 499.99, 'whispers.jpg', 0, 1),

('Code of Shadows', 'Liya', '9780987654321', 'TechVerse Press', '2023-02-10', '2nd', 'English', 280, 
 'A thrilling blend of technology and mystery with a heroine who hacks hearts.', 599.50, 'shadows.jpg', 0, 2);

 --BORROWED BOOK

 INSERT INTO BorrowedBooks 
(UserId, BookId, BorrowedDate, ReturnDate, IsReturned, IsLost, FineAmount, IsDeleted)
VALUES
(1, 1, '2025-07-10', '2025-07-20', 1, 0, 0.00, 0), 
(2, 2, '2025-07-01', NULL, 0, 1, 150.00, 0);      

--RESERVATION

INSERT INTO Reservations(UserId, BookId, ReservationDate, IsActive)
VALUES 
(1, 1, '2025-07-24', 1),  
(2, 2, '2025-07-23', 1); 

--REVIEW

INSERT INTO Reviews(UserId, BookId, Feedback, Rating, CreatedAt, IsDeleted)
VALUES 
(1, 1, 'Absolutely thrilling to read! Couldn’t put it down.', 5, '2025-07-24', 0),
(2, 2, 'Interesting concept but a bit slow in the middle.', 3, '2025-07-23', 0);

-- DISPLAY

SELECT * FROM Users;

SELECT * FROM Admins;

SELECT * FROM Categories;

SELECT * FROM Books;

SELECT * FROM BorrowedBooks;

SELECT * FROM Reservations;

SELECT * FROM Reviews;
