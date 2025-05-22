INSERT INTO Users (Email, Name, InitialDate, EndDate)
VALUES ('teste@sqr.com.br', 'Test User', '2021-01-01', '2025-12-31');

INSERT INTO Products (ProductCode, ProductDescription, Image, CycleTime)
VALUES ('abc', 'xxx', '0x000001', 30.3),
       ('def', 'yyy', '0x00000', 45.5);

INSERT INTO Orders (OrderId, Quantity, ProductCode)
VALUES ('111', 100.00, 'abc'),
       ('222', 200.00, 'def');

INSERT INTO Materials (MaterialCode, MaterialDescription)
VALUES ('aaa', 'desc1'),
       ('bbb', 'desc2'),
       ('ccc', 'desc3');

INSERT INTO ProductMaterials (ProductCode, MaterialCode)
VALUES ('abc', 'aaa'),
       ('abc', 'bbb'),
       ('def', 'ccc'),
       ('def', 'bbb');