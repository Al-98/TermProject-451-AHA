UPDATE Business as b1
SET numOfCheckIns = (SELECT COUNT(*) FROM CheckIn as c1 WHERE c1.businessID=b1.businessID),
    numOfReviews = (SELECT COUNT(*) FROM Review as n1 WHERE n1.businessID=b1.businessID);

UPDATE Business as b2
SET avgReview = ((SELECT SUM(stars) FROM Review as r1 WHERE r1.businessID=b2.businessID)/b2.numOfReviews)
WHERE b2.numOfReviews>0;
