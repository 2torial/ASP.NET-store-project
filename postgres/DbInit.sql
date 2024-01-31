INSERT INTO "Customer" ("UserName", "PassWord")
VALUES ('user', 'user');

INSERT INTO "Customer" ("UserName", "PassWord", "IsAdmin")
VALUES ('root', 'root', 'true');

INSERT INTO "Category" ("Type", "Label")
VALUES
	('Laptops', 'Laptops/Notebooks/Ultrabooks'),
	('Headsets', 'Headsets'),
	('Microphones', 'Microphones');

INSERT INTO "Configuration" ("Label", "Parameter")
VALUES
	('RAM Memory', '4GB'),
	('RAM Memory', '8GB'),
	('RAM Memory', '16GB'),
	('RAM Memory', '32GB'),
	('RAM Memory', '64GB'),
	('System', 'Windows 10'),
	('System', 'Windows 11'),
	('System', 'No System'),
	('System', 'MacOS'),
	('Disk', 'SSD'),
	('Disk', 'HDD'),
	('Disk', 'No Disk'),
	('Disk Capacity', '512GB'),
	('Disk Capacity', '1024GB'),
	('Disk Capacity', '2048GB'),
	('Disk Capacity', '4096GB'),
	('Processor', 'Ryzen 3'),
	('Processor', 'Ryzen 5'),
	('Processor', 'Ryzen 7'),
	('Processor', 'Ryzen 9'),
	('Processor', 'Intel Core i3'),
	('Processor', 'Intel Core i5'),
	('Processor', 'Intel Core i7'),
	('Processor', 'Intel Core i9'),
	('Cord Length', '1m'),
	('Cord Length', '2m');

INSERT INTO "Item" ("CategoryId", "Name", "Price")
VALUES
	('Laptops', 'Laptop #1', 900),
	('Laptops', 'Laptop #2', 650),
	('Laptops', 'Laptop #3', 800),
	('Laptops', 'Laptop #4', 500),
	('Laptops', 'Laptop #5', 660),
	('Laptops', 'Laptop #6', 500),
	('Laptops', 'Laptop #7', 450),
	('Headsets', 'Headset #1', 100),
	('Headsets', 'Headset #2', 300),
	('Headsets', 'Headset #3', 50),
	('Microphones', 'Microphone #1', 50),
	('Microphones', 'Microphone #2', 20);

INSERT INTO "Image" ("Content", "ItemId")
VALUES
	('https://placehold.co/150x150', 1),
	('https://placehold.co/150x150', 1),
	('https://placehold.co/150x150', 2),
	('https://placehold.co/150x150', 2),
	('https://placehold.co/150x150', 3),
	('https://placehold.co/150x150', 4),
	('https://placehold.co/150x150', 5),
	('https://placehold.co/150x150', 6),
	('https://placehold.co/150x150', 7),
	('https://placehold.co/150x150', 8),
	('https://placehold.co/150x150', 9),
	('https://placehold.co/150x150', 9),
	('https://placehold.co/150x150', 10),
	('https://placehold.co/150x150', 11),
	('https://placehold.co/150x150', 12);

INSERT INTO "Configuration" ("Label", "Parameter")
VALUES
	('RAM Memory', '8GB'),
	('RAM Memory', '16GB'),
	('RAM Memory', '32GB'),
	('RAM Memory', '16GB'),
	('RAM Memory', '64GB'),
	('RAM Memory', '8GB'),
	('RAM Memory', '16GB'),
	('RAM Memory', '32GB'),
	('Processor', 'Intel Core i3'),
	('Processor', 'Intel Core i5'),
	('Processor', 'Intel Core i7'),
	('Processor', 'Intel Core i9'),
	('Processor', 'Ryzen 3'),
	('Processor', 'Ryzen 5'),
	('Processor', 'Ryzen 7'),
	('Processor', 'Ryzen 9'),
	('Disk Capacity', '512GB'),
	('Disk Capacity', '1024GB'),
	('Disk Capacity', '2048GB'),
	('Disk Type', 'SSD'),
	('Disk Type', 'HDD'),
	('System', 'No System'),
	('System', 'Windows 10'),
	('System', 'Windows 11'),
	('System', 'Windows 11'),
	('System', 'Windows 10'),
	('Cord Length', '1m'),
	('Cord Length', '2m'),
	('Cord Length', 'no cord'),
	('Disk Type', 'HDD'),
	('System', 'No System'),
	('System', 'Windows 10'),
	('System', 'Windows 11');

INSERT INTO "ItemConfiguration" ("ItemId", "ConfigurationId")
VALUES
	(1, 5),
	(1, 8),
	(1, 10),
	(1, 16),
	(1, 19),
	(2, 3),
	(2, 7),
	(2, 11),
	(2, 14),
	(2, 21),
	(3, 1),
	(3, 6),
	(4, 3),
	(4, 8),
	(5, 5),
	(5, 9),
	(6, 10),
	(6, 15),
	(7, 11),
	(7, 15),
	(7, 22);

INSERT INTO "Status" ("Code")
VALUES
	('Pending'),
	('Preparing'),
	('Awaiting Delivery'),
	('Sent'),
	('Delivered'),
	('Returned'),
	('Canceled');

INSERT INTO "SelectedItem" ("ItemId", "CustomerId", "Quantity")
VALUES
	(1, 'user', 1),
	(8, 'user', 1),
	(12, 'user', 1),
	(4, 'root', 10),
	(1, 'root', 1);

INSERT INTO "AdressDetails" ("Region", "City", "PostalCode", "Street")
VALUES
	('Śląsk', 'Bielsko-Biała', '43-300', '3 Maja 17/91'),
	('Dolny Śląsk', 'Wrocław', '50-383', 'Fryderyka Joliot-Curie 15');

INSERT INTO "CustomerDetails" ("Name", "Surname", "PhoneNumber", "Email")
VALUES
	('Stanisław', 'August', '03 05 1791 0', '3 Maja 17/91'),
	('Dolny Śląsk', 'Wrocław', '30 01 2024 0', 'Fryderyka Joliot-Curie 15');

INSERT INTO "Order" ("Id", "CustomerId")
VALUES
	(1, 'user'),
	(2, 'root');