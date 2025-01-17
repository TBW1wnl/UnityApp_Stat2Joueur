CREATE DATABASE IF NOT EXISTS UserStatsProject;

CREATE USER 'UnityUser'@'localhost' IDENTIFIED BY 'Unity123*';

GRANT ALL PRIVILEGES ON UserStatsProject.* TO 'UnityUser'@'localhost';

USE UserStatsProject;

CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    winRate DECIMAL(2,2) NOT NULL,
    account_creation_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

