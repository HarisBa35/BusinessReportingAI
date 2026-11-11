CREATE DATABASE PoslovnoIzvjestavanjeDb;
GO

USE PoslovnoIzvjestavanjeDb;
GO

-- 1. Tabela Artikli
CREATE TABLE Artikli (
    ArtikalID INT IDENTITY(1,1) PRIMARY KEY,
    Naziv NVARCHAR(100) NOT NULL,
    Kategorija NVARCHAR(50) NOT NULL,
    Cijena DECIMAL(18,2) NOT NULL
);

-- 2. Tabela Kupci
CREATE TABLE Kupci (
    KupacID INT IDENTITY(1,1) PRIMARY KEY,
    NazivFirme NVARCHAR(100) NOT NULL,
    Grad NVARCHAR(50) NOT NULL
);

-- 3. Tabela Prodaja
CREATE TABLE Prodaja (
    ProdajaID INT IDENTITY(1,1) PRIMARY KEY,
    ArtikalID INT FOREIGN KEY REFERENCES Artikli(ArtikalID),
    KupacID INT FOREIGN KEY REFERENCES Kupci(KupacID),
    DatumProdaje DATE NOT NULL,
    Kolicina INT NOT NULL,
    UkupanIznos DECIMAL(18,2) NOT NULL
);

-- Unos testnih podataka
INSERT INTO Artikli (Naziv, Kategorija, Cijena) VALUES 
('Laptop ThinkPad', 'Elektronika', 1500.00),
('Monitor 27 Dell', 'Elektronika', 400.00),
('Kancelarijska Stolica', 'Namjestaj', 250.00),
('Mehanicka Tastatura', 'Periferija', 120.00);

INSERT INTO Kupci (NazivFirme, Grad) VALUES 
('TechSolutions d.o.o.', 'Sarajevo'),
('Balkan IT', 'Mostar'),
('Financ d.o.o.', 'Banja Luka');

INSERT INTO Prodaja (ArtikalID, KupacID, DatumProdaje, Kolicina, UkupanIznos) VALUES 
(1, 1, '2024-01-15', 3, 4500.00),
(2, 1, '2024-01-20', 5, 2000.00),
(1, 2, '2024-02-10', 2, 3000.00),
(3, 3, '2024-02-18', 10, 2500.00),
(4, 2, '2024-03-01', 4, 480.00),
(2, 3, '2024-03-12', 2, 800.00);
