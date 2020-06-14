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
            # sql_catg=""
            # for item in data["categories"]:
            #     if len(sql_catg)==0:
            #         sql_catg=sql_catg+str(item)
            #     else:
            #         sql_catg=sql_catg+","+str(item)
            #         # will return a string with a comma in it ex. resturant,sandwiches

            # sql_time=""
            # for key,value in data['hours'].items():
            #         g=value.split("-")
            #         if len(sql_time)==0:
            #             sql_time=sql_time+str(key+","+g[0]+","+g[1])
            #         else:
            #             sql_time=sql_time+";"+str(key+","+g[0]+","+g[1])
            #             # will return a string with a comma in it ex. monday,7,20;tuesday,8,20
            
            sql_str1 = "INSERT INTO Business (businessID, name, address,state,city,zipCode,latitude,longitude,starRating,numOfReviews,numOfCheckIns,openStatus) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["address"]) + "','" + \
                      cleanStr4SQL(data["state"]) + "','" + cleanStr4SQL(data["city"]) + "','" + cleanStr4SQL(data["postal_code"]) + "'," + str(data["latitude"]) + "," + \
                      str(data["longitude"]) + ",0.0,0,0,"  + \
                      int2BoolStr(data["is_open"]) +");"
            # print(sql_str1)
                      #categories might need to be varchar 

           
            try:
                # inserts business
                cur.execute(sql_str1)
                
                # insertes catagory and iscat
                for item in data['categories']:
                     
                    sql_str21 ="INSERT INTO Categories (category) " \
                    "VALUES ("+str(item)+");"
                    print(sql_str21)
                    sql_str22 ="INSERT INTO isCat (businessID,category) " \
                    "VALUES ('" + cleanStr4SQL(data["business_id"])+"',"+str(item)+");"
                    print(sql_str22)
                    cur.execute(sql_str21)
                    cur.execute(sql_str22)
                    #if we do this it will break when catagoru for 2.1 already exists 
                
                for key,value in data['hours'].items():
                    g=value.split("-")
                    (str(key+" from "+g[0]+" to "+g[1]))
                    sql_str3 ="INSERT INTO DayHours (weekday,openTime,closeTime,businessID) " \
                    "VALUES ('"+str(key)+"','"+str(g[0])+"','"+str(g[1])+"','"+cleanStr4SQL(data["business_id"])+"');"
                    print(sql_str3)
                    cur.execute(sql_str3)

            except Exception as error:
                print('ERROR: Insert to business table failed!')
                print('Error message:', error)

            # conn.commit()
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
                        str(data['average_stars'])+","+ str(data['fans'])+","+str(data['review_count'])+") ON CONFLICT (userID) DO UPDATE"+\
                        "SET firstName=excluded.firstName,joinDate=excluded.joinDate,avgStars=excluded.avgStars,numOfFans=excluded.numOfFans,numOfVotes=excluded.numOfVotes; "
            
            # print(sql_str1)
           
            try:
                # inserts business
                cur.execute(sql_str1)
                
                # cannot add friends if they dont exist in able might need a triger 
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
        # try:
        #     conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='AHA'")
        # except Exception as error:
        #     print('ERROR: Unable to connect to the database!')
        #     print('Error message:', error)

        # cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            

            # in the .json file only first name appears 
            
           
            try:
                
                
                
                for key,value in data['time'].items():  #day + time + amount of checkins 
                    for time,chekins in value.items():
                        sql_str1="INSERT INTO CheckIn (businessID,weekday,checkinTime,numOfCheckIns) " \
                                "VALUES ('"+ cleanStr4SQL(data['business_id'])+"',"+str(key)+","+str(time)+","+str(chekins)+");"
                        print(sql_str1)
                    

                # cur.execute(sql_str1)
        
                
                

            except Exception as error:
                print('ERROR: Insert to user table failed!')
                print('Error message:', error)

            # conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        # cur.close()
        # conn.close()

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
        # try:
        #     conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='AHA'")
        # except Exception as error:
        #     print('ERROR: Unable to connect to the database!')
        #     print('Error message:', error)

        # cur = conn.cursor()

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
                
                print(sql_str1)
                # cur.execute(sql_str1)

            except Exception as error:
                print('ERROR: Insert to user table failed!')
                print('Error message:', error)

            # conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        # cur.close()
        # conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


# insert2BusinessTable()
insert2UserTable()
# insert2CheckinTable()
# insert2reviewTable()
