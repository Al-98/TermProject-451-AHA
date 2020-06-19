import json
import sys
import psycopg2

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def int2BoolStr (value):
    if value == 0:
        return 'False'
    else:
        return 'True'

def insert2BusinessTable():
    #reading the JSON file
    with open('yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
       
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='AHA'")
        except Exception as error:
            print('ERROR: Unable to connect to the database!')
            print('Error message:', error)

        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            # include values for all businessTable attributes
            
            value=data['name']
            parser=value.split("'")
            text1=""
            for piece in parser:
                text1=text1+(str("'"+piece+"'"))

            # print(text1)
            # creats inser statment for Business
            sql_str1 = "INSERT INTO Business (businessID, name, address,state,city,zipCode,latitude,longitude,starRating,numOfReviews,numOfCheckIns) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "'," + text1 + ",'" + cleanStr4SQL(data["address"]) + "','" + \
                      cleanStr4SQL(data["state"]) + "','" + cleanStr4SQL(data["city"]) + "','" + cleanStr4SQL(data["postal_code"]) + "'," + str(data["latitude"]) + "," + \
                      str(data["longitude"]) + ",0.0,0,0" + ");"
                      
            # int2BoolStr(data["is_open"]) +
            # print(sql_str1)
                       
            try:
                # inserts business
                cur.execute(sql_str1)
                
                # creats inser statment for iscat
                for item in data['categories']:
                    value=item
                    spliter=value.split("'")
                    text2=""
                    for splice in spliter:
                        text2=text2+(str("'"+splice+"'"))

                    
                    sql_str22 ="INSERT INTO isCat (businessID,category) " \
                    "VALUES ('" + cleanStr4SQL(data["business_id"])+"',"+text2+");"
                    # insertes iscat and using a triger it will insert into category 
                    cur.execute(sql_str22)

                # creats inser statment for DayHours
                for key,value in data['hours'].items():
                    g=value.split("-")
                    (str(key+" from "+g[0]+" to "+g[1]))
                    sql_str3 ="INSERT INTO DayHours (weekday,openTime,closeTime,businessID) " \
                    "VALUES ('"+str(key)+"','"+str(g[0])+"','"+str(g[1])+"','"+cleanStr4SQL(data["business_id"])+"');"
                    # print(sql_str3)
                    #inserts dyahourse 
                    cur.execute(sql_str3)

            except Exception as error:
                print('ERROR: Insert to business table failed!')
                print('Error message:', error)

            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        # cur.close()
        # conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2UserTable():
    #reading the JSON file
    with open('yelp_user.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
       
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='AHA'")
        except Exception as error:
            print('ERROR: Unable to connect to the database!')
            print('Error message:', error)

        cur = conn.cursor()

        while line:
            data = json.loads(line)
             # Generate the INSERT statement for the cussent business
             # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            

            # long and lat dont exist and the re is only name no last name in data
            sql_str1 = "INSERT INTO Users (userID,firstName,joinDate,avgStars,numOfFans,numOfVotes) " \
                        "VALUES ('" + cleanStr4SQL(data['user_id'])+"','"+cleanStr4SQL(data['name'])+"','"+str((data['yelping_since']))+"',"+\
                        str(data['average_stars'])+","+ str(data['fans'])+","+str(data['review_count'])+") ON CONFLICT (userID) DO UPDATE "+\
                        "SET firstName=excluded.firstName,joinDate=excluded.joinDate,avgStars=excluded.avgStars,numOfFans=excluded.numOfFans,numOfVotes=excluded.numOfVotes; "
            
            # print(sql_str1)
           
            try:
                # inserts reviews
                cur.execute(sql_str1)
                
                # cannot add friends if they dont exist need a triger. triger in sql that inserts an user id with nulll attrivutes and then fills it
                for item in data['friends']:
                    sql_str2 ="INSERT INTO Friends (userID,friendID) " \
                    "VALUES ('"+cleanStr4SQL(data['user_id'])+"','"+str(item)+"');"
                    # print(sql_str2)
                    
                    cur.execute(sql_str2)
           
            except Exception as error:
                print('ERROR: Insert to user table failed!')
                print('Error message:', error)

            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

    

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2CheckinTable():
    #reading the JSON file
    with open('yelp_checkin.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
       
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='AHA'")
        except Exception as error:
            print('ERROR: Unable to connect to the database!')
            print('Error message:', error)

        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            

            # in the .json file only first name appears 
            
           
            try:
                
                
                
                for key,value in data['time'].items():  #day + time + amount of checkins 
                    for time,chekins in value.items():
                        sql_str1="INSERT INTO CheckIn (businessID,weekday,checkinTime,numOfCheckIns) " \
                                "VALUES ('"+ cleanStr4SQL(data['business_id'])+"','"+str(key)+"','"+str(time)+"',"+str(chekins)+");"
                        # print(sql_str1)
                        cur.execute(sql_str1)
        
                
                

            except Exception as error:
                print('ERROR: Insert to user table failed!')
                print('Error message:', error)

            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2reviewTable():
    #reading the JSON file
    with open('yelp_review.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
       
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='AHA'")
        except Exception as error:
            print('ERROR: Unable to connect to the database!')
            print('Error message:', error)

        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            #    needs a bit of fixing 
            value=data['text']
            g=value.split("'")
            text=""
            for a in g:
                text=text+(str("'"+a+"'"))
            #issuse including the text since it cintains ' inside the text maybe parse?

            # in the .json file only first name appears 
            sql_str1 = "INSERT INTO Review (reviewID,reviewDate,reviewText,stars,userID,businessID) " \
                        "VALUES ('" + cleanStr4SQL(data['review_id'])+"','"+cleanStr4SQL(data['date'])+"',"+text+","+\
                        str(data['stars'])+",'"+ str(data['user_id'])+"','"+str(data['business_id'])+"');"
             
            try: 
                
                # print(sql_str1)
                cur.execute(sql_str1)

            except Exception as error:
                print('ERROR: Insert to user table failed!')
                print('Error message:', error)

            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        # cur.close()
        # conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


insert2BusinessTable()
insert2UserTable()
insert2CheckinTable()
insert2reviewTable()
