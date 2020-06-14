--Team Members: 
--Al-Muatasim Al-Habsi
--Hongbo Wang
--Aldo Zepeda-Lopez 

CREATE TABLE Users(
	userID CHAR(22),
	firstName CHAR(20),
	lastName CHAR(20),
	joinDate DATE,
	latitude REAL,
	longitude REAL,
	avgStars REAL,
	numOfFans INTEGER Check(numOfFans>=0),
	numOfVotes INTEGER Check(numOfVotes>=0),
	PRIMARY KEY (userId),
	CHECK(avgStars>=0),
	CHECK(avgStars<=5)
);
--Note: Need average stars and number of votes to be derived

CREATE TABLE Business(
	businessID CHAR(22),
	name VARCHAR(50),
	address CHAR(50),
	city VARCHAR(20),
	state CHAR(2),
	zipCode CHAR(5),
	latitude REAL,
	longitude REAL,
	avgReview REAL,
	starRating REAL,
	numOfReviews INTEGER Check(numOfReviews>=0),
	numOfCheckIns INTEGER Check(numOfCheckIns>=0),
	PRIMARY KEY (businessID),
	CHECK(avgReview>=0),
	CHECK(avgReview<=5),
	CHECK(starRating>=0),
	CHECK(starRating<=5)
);
--Note: number of checkins,Reviews, and avg review should be derived

CREATE TABLE Review(
	reviewID CHAR(22),
	reviewDate DATE,
	reviewText VARCHAR(600),
    stars Real,
	userID CHAR(9),
	businessID CHAR(22),
	PRIMARY KEY (reviewID,userID,businessID),
	FOREIGN KEY (userID) REFERENCES Users(userID),
	FOREIGN KEY (businessID) REFERENCES Business(businessID),
	CHECK(stars>=0),
	CHECK(stars<=5)
);

CREATE TABLE Friends(
	userID CHAR(22),
	friendID CHAR(22),
	PRIMARY KEY (userID,friendID),
	FOREIGN KEY (userID) REFERENCES Users(userID),
	FOREIGN KEY (friendID) REFERENCES Users(userID)
);
--Note: should have derived attributes; latest Review and latest tip

CREATE TABLE CheckIn(
	businessID CHAR(22),
	weekday VARCHAR(9) CHECK(weekday IN ('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday','Sunday')),
	checkinTime TIME,
	numOfCheckIns INTEGER,
	PRIMARY KEY (weekday,checkinTime,businessID),
	FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE Favorite(
	userID CHAR(22),
	businessID CHAR(22),
	PRIMARY KEY (userID,businessID),
	FOREIGN KEY (userID) REFERENCES Users(userID),
	FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE Categories(
	category VARCHAR(30),
	PRIMARY KEY (category)
);

CREATE TABLE isCat(
	businessID CHAR(22),
	category VARCHAR(30),
	PRIMARY KEY (category,businessID),
	FOREIGN KEY (category) REFERENCES Categories(category),
	FOREIGN KEY (businessID) REFERENCES Business(businessID)
);
--Note: cant enforce total participation of restaraunts with some categorie

CREATE TABLE DayHours(
	weekday CHAR(9),
	openTime TIME,
	closeTime TIME,
	businessID CHAR(22),
	PRIMARY KEY (weekday,businessID),
	FOREIGN KEY (businessID) REFERENCES Business(businessID)
);