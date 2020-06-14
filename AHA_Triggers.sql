CREATE OR REPLACE FUNCTION newReview() RETURNS trigger AS '
BEGIN
    UPDATE Bsiness as b1
    SET numOfReviews = (SELECT COUNT(reviewID) FROM Review as r1 WHERE r1.businessID=b1.businessID)
    WHERE b1.businessID=NEW.businessID;
    UPDATE Business as b2
    SET avgReview = ((SELECT SUM(stars FROM Review as r2 WHERE r2.businessID=b2.businessID))/numofReviews)
    WHERE b2.businessID=NEW.businessID;
    RETURN NEW;
END
'  LANGUAGE plpgsql;

CREATE TRIGGER addReview
AFTER INSERT ON Review
FOR EACH ROW 
WHEN (NEW.businessID is NOT NULL)
EXECUTE PROCEDURE newReview();


CREATE OR REPLACE FUNCTION updateCheckIns() RETURNS trigger AS '
BEGIN
    UPDATE Business as b1
    SET Business.numOfCheckIns = (SELECT SUM(numOfCheckIns) FROM CheckIn as c1 WHERE c1.businessID=b1.businessID)
    WHERE b1.businessID=NEW.businessID;    
    RETURN NEW;
END
'  LANGUAGE plpgsql;

CREATE TRIGGER updateCheckin
AFTER UPDATE ON CheckIn
FOR EACH ROW 
WHEN (NEW.businessID is NOT NULL)
EXECUTE PROCEDURE updateCheckIns();

CREATE TRIGGER addCheckIn
AFTER INSERT ON CheckIn
FOR EACH ROW 
WHEN (NEW.businessID is NOT NULL)
EXECUTE PROCEDURE updateCheckIns();


CREATE OR REPLACE FUNCTION addCategory() RETURNS trigger AS '
BEGIN
    INSERT INTO Categories
    (SELECT NEW.category AS category
    WHERE NOT EXISTS (SELECT *
                      FROM Categories
                      WHERE Categories.category=NEW.category));    
    RETURN NEW;
END
'  LANGUAGE plpgsql;

CREATE TRIGGER insertCategories
BEFORE INSERT ON isCat
FOR EACH ROW 
WHEN (NEW.businessID is NOT NULL)
EXECUTE PROCEDURE addCategory();

CREATE TRIGGER updateCategories
BEFORE UPDATE ON isCat
FOR EACH ROW 
WHEN (NEW.businessID is NOT NULL)
EXECUTE PROCEDURE addCategory();

CREATE OR REPLACE FUNCTION addFriend() RETURNS trigger AS '
BEGIN
    INSERT INTO Users
    (SELECT NEW.friendID as userID, NULL as firstName, NULL as lastName, NULL as joinDate, NULL as latitude, NULL as longitude, NULL as avgSatrs, NULL as numOfFans,NULL as numOfVotes, NULL as numOfCheckIns
    WHERE NOT EXISTS (SELECT *
                      FROM Users
                      WHERE Users.userID=NEW.friendID));    
    RETURN NEW;
END
'  LANGUAGE plpgsql;

CREATE TRIGGER insertFriend
BEFORE INSERT ON Friends
FOR EACH ROW 
WHEN (NEW.userID is NOT NULL)
EXECUTE PROCEDURE addFriend());
