CREATE DATABASE IF NOT EXISTS UserStatsProject;

CREATE OR REPLACE USER 'UnityUser'@'localhost' IDENTIFIED BY 'Unity123*';

GRANT ALL PRIVILEGES ON UserStatsProject.* TO 'UnityUser'@'localhost';

USE UserStatsProject;

CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    account_creation_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE teams (
    id INT AUTO_INCREMENT PRIMARY KEY,
    score INT(8) DEFAULT 0
);

CREATE TABLE games (
    id INT AUTO_INCREMENT PRIMARY KEY,
    startDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    duration TIME DEFAULT '00:00:00',
    team0 INT NOT NULL,
    team1 INT NOT NULL,
    FOREIGN KEY (team0) REFERENCES teams(id),
    FOREIGN KEY (team1) REFERENCES teams(id)
);

CREATE TABLE Comments (
    id INT AUTO_INCREMENT PRIMARY KEY,
    content TEXT NOT NULL,
    user INT NOT NULL,
    FOREIGN KEY (user) REFERENCES users(id)
);

CREATE TABLE team_memberships (
    user INT NOT NULL,
    team INT NOT NULL,
    kills INT(8),
    deaths INT(8),
    assists INT(8),
    PRIMARY KEY (user, team),
    FOREIGN KEY (user) REFERENCES users(id),
    FOREIGN KEY (team) REFERENCES teams(id)
);

-- Insérer des utilisateurs
-- Pour tous les utilisateurs le mot de passe est 'SecurePassword1234'
INSERT INTO users (name, email, password)
VALUES 
('SigmaBoi', 'exemple@email.com', '$2y$10$KY/FrE57ZpWVsK3qCikVF.6AGl3ZPhkFNgnN3I8V89XelF2lsp6Du'),
('TheDestructor95', 'secundus@email.com', '$2y$10$KY/FrE57ZpWVsK3qCikVF.6AGl3ZPhkFNgnN3I8V89XelF2lsp6Du'),
('MAXlaMENACE', 'jiji@email.com', '$2y$10$KY/FrE57ZpWVsK3qCikVF.6AGl3ZPhkFNgnN3I8V89XelF2lsp6Du'),
('Fnrgfe', 'testy@email.com', '$2y$10$KY/FrE57ZpWVsK3qCikVF.6AGl3ZPhkFNgnN3I8V89XelF2lsp6Du'),
('Time', 'time@email.com', '$2y$10$KY/FrE57ZpWVsK3qCikVF.6AGl3ZPhkFNgnN3I8V89XelF2lsp6Du');

-- Insérer des équipes
INSERT INTO teams (score)
VALUES
(10),
(20),
(30),
(40),
(50);

-- Insérer des jeux
INSERT INTO games (startDate, duration, team0, team1)
VALUES
('2025-01-01 14:30:00', '00:30:00', 1, 2),
('2025-01-02 15:00:00', '00:45:00', 2, 3),
('2025-01-03 16:15:00', '00:20:00', 3, 4),
('2025-01-04 17:00:00', '01:00:00', 4, 5),
('2025-01-05 18:30:00', '00:35:00', 5, 1);

-- Insérer des adhésions d'équipe avec statistiques
INSERT INTO team_memberships (user, team, kills, deaths, assists)
VALUES
(1, 1, 5, 2, 3),
(2, 1, 4, 3, 2),
(3, 2, 8, 4, 1),
(4, 2, 7, 5, 3),
(5, 3, 6, 4, 2),
(1, 3, 3, 1, 4),
(2, 4, 9, 6, 2),
(3, 4, 10, 5, 4),
(4, 5, 4, 2, 6),
(5, 5, 8, 3, 5);
