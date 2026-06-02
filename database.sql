-- Written by Brian McCarthy
CREATE TABLE Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    Email TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    Role TEXT NOT NULL CHECK(Role IN ('admin', 'user'))
);

-- Secure query example: use parameters instead of string concatenation.
-- SELECT UserID, Username, Email, PasswordHash, Role FROM Users WHERE Username = @Username;
