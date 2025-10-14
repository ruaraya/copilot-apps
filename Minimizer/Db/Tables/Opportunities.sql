CREATE TABLE Opportunities (
    id BINARY(16) PRIMARY KEY,
    lead_id INT,
    value DECIMAL(12,2),
    stage VARCHAR(20),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (lead_id) REFERENCES Leads(id)
);