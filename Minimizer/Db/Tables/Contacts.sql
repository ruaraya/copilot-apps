CREATE TABLE Contacts (
    id BINARY(16) PRIMARY KEY,
    lead_id INT,
    name VARCHAR(100) NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(100),
    position VARCHAR(50),
    FOREIGN KEY (lead_id) REFERENCES Leads(id)
);