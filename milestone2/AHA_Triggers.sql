CREATE OR REPLACE FUNCTION newReview() RETURNS trigger AS '
BEGIN
    UPDATE Business as b1
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