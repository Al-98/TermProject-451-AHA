--Team Name: AHA
--Members: Almuatasim Alhabsi
--         Hongbo Wang
--         Aldo N Zepeda-Lopez

--Note if Just running Triggers comment out test statements below

-- Trigger for updating reviewCount, and stars(avg of Reviews) in business table whwn new review is added
CREATE OR REPLACE FUNCTION newReview() RETURNS trigger AS '
BEGIN
    UPDATE Business as b1
    SET numOfReviews = (SELECT COUNT(reviewID) FROM Review as r1 WHERE r1.businessID=b1.businessID)
    WHERE b1.businessID=NEW.businessID;
    UPDATE Business as b2
    SET avgReview = ((SELECT SUM(stars) FROM Review as r2 WHERE r2.businessID=b2.businessID)/numofReviews)
    WHERE b2.businessID=NEW.businessID AND numOfReviews>0;
    RETURN NEW;
END
'  LANGUAGE plpgsql;

CREATE TRIGGER addReview
AFTER INSERT ON Review
FOR EACH ROW 
WHEN (NEW.businessID is NOT NULL)
EXECUTE PROCEDURE newReview();


--Triggers for updating numOfCheckins in business table when a row is updated or added to checkin table
CREATE OR REPLACE FUNCTION updateCheckIns() RETURNS trigger AS '
BEGIN
    UPDATE Business as b1
    SET numOfCheckIns = (SELECT SUM(numOfCheckIns) FROM CheckIn as c1 WHERE c1.businessID=b1.businessID)
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


-- Trigger for adding categories that don't yet exist, to category table when needed for isertion of business info to iscat
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


--Trigger for adding users with only userid value to usertable for proper insertion of friends in isFriend table
--  other values will only be null until that users data is properly inserted into Users table
CREATE OR REPLACE FUNCTION addFriend() RETURNS trigger AS '
BEGIN
    INSERT INTO Users
    (SELECT NEW.friendID as userID, NULL as firstName, NULL as lastName, NULL as joinDate, NULL as latitude, NULL as longitude, NULL as avgSatrs, NULL as numOfFans,NULL as numOfVotes 
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
EXECUTE PROCEDURE addFriend();



--Note if Just running Triggers comment out test statements below

-- -------------------------------- Testing --------------------------------- --
-- Test 1: 
--          get number of checkins for this business then add one checkin then recheck
SELECT numOfCheckIns FROM business where businessID='SnlaPHqW3ksKfpH12pDGeg'

INSERT INTO checkin
Values ('SnlaPHqW3ksKfpH12pDGeg', 'Monday', '23:00:00', 1)

SELECT numOfCheckIns FROM business where businessID='SnlaPHqW3ksKfpH12pDGeg'

-- Test 2: 
--          Repeat above for same business but this time add 3 checkins on with diferent values then recheck
SELECT numOfCheckIns FROM business where businessID='SnlaPHqW3ksKfpH12pDGeg'

INSERT INTO checkin
Values ('SnlaPHqW3ksKfpH12pDGeg', 'Friday', '13:00:00', 3)

SELECT numOfCheckIns FROM business where businessID='SnlaPHqW3ksKfpH12pDGeg'

-- Test 3: 
--          get number of reviews and avg review for this business then add new review and recheck values
select numOfReviews, avgReview from business where businessid='SnlaPHqW3ksKfpH12pDGeg'
 
INSERT INTO review
Values ('0000000000000000000000','2020-03-18','TEST','1','zh9vXKAaUAErsqxY-0mHWw','SnlaPHqW3ksKfpH12pDGeg' )

select numOfReviews, avgReview from business where businessid='SnlaPHqW3ksKfpH12pDGeg'
