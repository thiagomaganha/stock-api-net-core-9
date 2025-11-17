CREATE DATABASE StockMarket;

GO

use StockMarket;

CREATE TABLE Stock (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Symbol NVARCHAR(MAX) NOT NULL,
    CompanyName NVARCHAR(MAX) NOT NULL,
    Purchase DECIMAL(18,2) NOT NULL,
    Dividend DECIMAL(18,2) NOT NULL,
    Industry NVARCHAR(MAX) NOT NULL,
    MarketCap BIGINT NOT NULL
);


CREATE TABLE Comment (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(MAX) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    StockId INT NOT NULL,
    CONSTRAINT FK_Comment_Stock FOREIGN KEY (StockId) REFERENCES Stock(Id) ON DELETE CASCADE
);


INSERT INTO Stock (Symbol, CompanyName, Purchase, Dividend, Industry, MarketCap) VALUES
('AAPL', 'Apple Inc.', 150.00, 0.88, 'Technology', 2500000000000),
('MSFT', 'Microsoft Corporation', 280.50, 1.20, 'Technology', 2300000000000),
('TSLA', 'Tesla Inc.', 720.00, 0.00, 'Automotive', 900000000000),
('JNJ', 'Johnson & Johnson', 165.75, 2.50, 'Healthcare', 450000000000),
('KO', 'Coca-Cola Co.', 60.25, 1.64, 'Consumer Goods', 260000000000);


INSERT INTO Comments (Title, Content, CreatedOn, StockId) VALUES
('Strong Buy', 'Apple is showing strong growth potential.', GETDATE(), 1),
('Solid Investment', 'Microsoft continues to dominate the enterprise market.', GETDATE(), 2),
('Volatile but Promising', 'Tesla is risky but has great upside.', GETDATE(), 3),
('Stable Dividend', 'JNJ is a reliable dividend stock.', GETDATE(), 4),
('Classic Brand', 'Coca-Cola has strong brand loyalty.', GETDATE(), 5),
('Innovation Leader', 'Apple’s innovation keeps it ahead.', GETDATE(), 1),
('Cloud Growth', 'Azure is growing rapidly.', GETDATE(), 2),
('EV Market Leader', 'Tesla is leading the electric vehicle revolution.', GETDATE(), 3),
('Healthcare Giant', 'JNJ has a strong product pipeline.', GETDATE(), 4),
('Dividend King', 'KO has a long history of dividend payments.', GETDATE(), 5);

--DROP TABLE Comments;

--DROP TABLE Stock;