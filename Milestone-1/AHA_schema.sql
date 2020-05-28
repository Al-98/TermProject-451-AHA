Team Members: 
Al-muatasim Al-habsi
Hongbo Wang
Aldo Zepeda-Lopez 

CREATE TABLE Users(
	userID CHAR(9),
	firstName CHAR(15) NOT NULL,
	lastName CHAR(20) ) NOT NULL,
	joinDate DATE,
	latitude REAL,
	longitude REAL,
	avgStars REAL,
	numOfFans INTEGER Check(numOfFans>=0),
	numOfVotes INTEGER Check(numOf Check(numOfVotes>=0),
	PRIMARY KEY (userId)
	CHECK(avgStars>=0),
	CHECK(avgStars<=5)
);
--Note: Need average stars and number of votes to be derived

CREATE TABLE Business(
	name VARCHAR(50),
	zipCode CHAR(5),
	city VARCHAR(20),
	state CHAR(2),
	address CHAR(50),
	avgReview REAL,
	numOfReviews INTEGER Check(numOfReviews>=0),
	numOfCheckIns INTEGER Check(numOfCheckIns>=0),
	starRating REAL,
	PRIMARY KEY (name,zipCode,address),
	CHECK(avgReview>=0),
	CHECK(avgReview<=5),
	CHECK(starRating>=0),
	CHECK(starRating<=5)
);
--Note: number of checkins,Reviews, and avg review should be derived

CREATE TABLE Review(
	reviewDate DATE,
	userID CHAR(9),
	zipCode CHAR(5),
	address CHAR(50),
	reviewText VARCHAR(500),
	stars Real,
	name VARCHAR(50),
	PRIMARY KEY (reviewDate,userID,name,zipCode,address),
	FOREIGN KEY (userID) REFERENCES Users(userID),
	ON DELETE CASCADE,
	ON UPDATE CASCADE,
	FOREIGN KEY (name,zipCode,address) REFERENCES Business(name,zipCode,address),
	ON DELETE CASCADE,
	ON UPDATE CASCADE.
	CHECK(stars>=0),
	CHECK(stars<=5)
);

CREATE TABLE Friends(
	userID CHAR(9),
	friendID CHAR(9),
	PRIMARY KEY (userID,friendID),
	FOREIGN KEY (userID) REFERENCES Users(userID),
	ON DELETE CASCADE,
	ON UPDATE CASCADE,
	FOREIGN KEY (friendID) REFERENCES Users(userID),
	ON DELETE CASCADE,
	ON UPDATE CASCADE
);
--Note: should have derived attributes; latest Review and latest tip

CREATE TABLE CheckIn(
	userID CHAR(9),
	zipCode CHAR(5),
	address CHAR(50),
	name VARCHAR(50),
	checkInDate DATE,
	PRIMARY KEY (userID,zipCode,name,address,checkInDate),
	FOREIGN KEY (userID) REFERENCES User(userID),
	ON DELETE CASCADE,
	ON UPDATE CASCADE,
	FOREIGN KEY (name,zipCode,address) REFERENCES Business(name,zipCode,address),
	ON DELETE CASCADE,
	ON UPDATE CASCADE
);

CREATE TABLE Tip(
	userID CHAR(9),
	zipCode CHAR(5),
	address CHAR(50),
	tipText VARCHAR(250)
	name VARCHAR(50),
	PRIMARY KEY (userID,name,zipCode,address),
	FOREIGN KEY (userID) REFERENCES User(userID),
	ON DELETE CASCADE,
	ON UPDATE CASCADE,
	FOREIGN KEY (name,zipCode,address) REFERENCES Business(name,zipCode,address),
	ON DELETE CASCADE,
	ON UPDATE CASCADE
);

CREATE TABLE Favorite(
	userID CHAR(9),
	zipCode CHAR(5),
	address CHAR(50),
	numOfCheckIns INTEGER  Check(numOfCheckIns>=0),
	name VARCHAR(50),
	PRIMARY KEY (userID,name,zipCode,address),
	FOREIGN KEY (userID) REFERENCES User(userID),
	ON DELETE CASCADE,
	ON UPDATE CASCADE,
	FOREIGN KEY (name,zipCode,address) REFERENCES Business(name,zipCode,address),
	ON DELETE CASCADE,
	ON UPDATE CASCADE,
);
--Note: numOfCheckIns should be derived

CREATE TABLE Categories(
	category CHAR(20),
	PRIMARY KEY (category)
);

CREATE TABLE isCat(
	zipCode CHAR(5),
	address CHAR(50),
	category CHAR(20),
	name VARCHAR(50),
	PRIMARY KEY (category,name,zipCode,address),
	FOREIGN KEY (category) REFERENCES Categories(category),
	ON DELETE CASCADE,
	ON UPDATE CASCADE,
	FOREIGN KEY (name,zipCode,address) REFERENCES Business(name,zipCode,address),
	ON DELETE CASCADE,
	ON UPDATE CASCADE
);
--Note: cant enforce total participation of restaraunts with some categorie

CREATE TABLE DayHours(
	weekday CHAR(9),
	openTime TIME,
	closeTime TIME,
	zipCode CHAR(5),
	address CHAR(50),
	name VARCHAR(50),
	PRIMARY KEY (weekday,category,name,zipCode,address),
	FOREIGN KEY (name,zipCode,address) REFERENCES Business(name,zipCode,address),
	ON DELETE CASCADE,
	ON UPDATE CASCADE
);