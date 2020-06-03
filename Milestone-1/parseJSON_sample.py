import json

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def parseBusinessData():
    #read the JSON file
    with open('yelp_business.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('business.txt', 'w') #<--stores data in new file named 
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        #(business_id, name; address; city; state; postal_code; latitude; longitude; stars; review_count, is_open)
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id'])+'\t') #business id
            outfile.write(cleanStr4SQL(data['name'])+'\t') #name
            outfile.write(cleanStr4SQL(data['address'])+'\t') #full_address
            outfile.write(cleanStr4SQL(data['state'])+'\t') #state
            outfile.write(cleanStr4SQL(data['city'])+'\t') #city
            outfile.write(cleanStr4SQL(data['postal_code']) + '\t')  #zipcode
            outfile.write(str(data['latitude'])+'\t') #latitude
            outfile.write(str(data['longitude'])+'\t') #longitude
            outfile.write(str(data['stars'])+'\t') #stars
            outfile.write(str(data['review_count'])+'\t') #reviewcount
            outfile.write(str(data['is_open'])+'\t') #openstatus
            
            for item in  data['categories']:
                outfile.write(str(item)+'\t')   # code to process catagories 
           
            outfile.write('\n')
            outfile.write("hours:")
            outfile.write('\n')
            for key,value in data['hours'].items():
                    g=value.split("-")
                    outfile.write(str(key+" from "+g[0]+" to "+g[1])+'\t') # code to process hours
                    outfile.write('\n')

            outfile.write('\n')
            outfile.write('\n')

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

def parseUserData():
    # code to parse yelp_user.JSON
   
    with open('yelp_user.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('users.txt', 'w') # <--stores data in new file named 
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        #(user_id; name; yelping_since; review_count; fans; average_stars; funny; useful; cool; fans)
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['user_id'])+', ') #userid
            outfile.write(cleanStr4SQL(data['name'])+', ') #name 
            outfile.write(cleanStr4SQL(data['yelping_since'])+', ') #since
            outfile.write(str(data['review_count'])+', ') #review count
            outfile.write(str(data['fans'])+', ') #fans
            outfile.write(str(data['average_stars'])+', ') #average stars
            outfile.write(str(data['funny'])+', ') #funny
            outfile.write(str(data['useful'])+', ') #useful
            outfile.write(str(data['cool'])+', ') #cool            
            outfile.write(str(data['fans'])+', ') #fans
            outfile.write('\n')
            outfile.write(str("Friends: ")) #fans
            #outfile.write(str([item for item in  data['friends']])+'\t') #friends list
            outfile.write('\n')
            for item in data['friends']:
                outfile.write(str(item)+', ' )
                outfile.write('\n')

            outfile.write('\n')
            outfile.write('\n')
            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()
    pass

def parseCheckinData():
    #code to parse yelp_checkin.JSON
    with open('yelp_checkin.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('checkin.txt', 'w')#<--stores data in new file named 
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        #(business_id; dayofweek; hour; checkin count)
        while line:
            data = json.loads(line)
            outfile.write(str(data['business_id'])+'\t') #business_id 

            outfile.write('\n') #new line

            for key,value in data['time'].items():  #day + time + amount of checkins 
                for time,chekins in value.items():
                    outfile.write(str(key+","+time+","+str(chekins)))
                    outfile.write('\n')      

            outfile.write('\n')   
            line = f.readline()
            count_line +=1

    print(count_line)
    outfile.close()
    f.close()
    pass


def parseReviewData():
    
    with open('yelp_review.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('review.txt', 'w')#<--stores data in new file named 
        line = f.readline()
        count_line = 0
        
        #(review_id, user_id; business_id; stars; date; text; useful; funny; cool)
        while line:
            data = json.loads(line)
            outfile.write(str(data['review_id'])+'\t') #review_id 
            outfile.write(str(data['user_id'])+'\t') #user_id
            outfile.write(str(data['business_id'])+'\t') #business_id
            outfile.write(str(data['stars'])+'\t') #stars double
            outfile.write(str(data['date'])+'\t') #date
            outfile.write(str(data['text'])+'\t') #txt str
            outfile.write(str(data['useful'])+'\t') #usefull int
            outfile.write(str(data['funny'])+'\t') #funny int
            outfile.write(str(data['cool'])+'\t') #cool int
            outfile.write('\n')
            outfile.write('\n')

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()
    pass
    

# parseBusinessData()
# parseUserData()
# parseCheckinData()
# parseReviewData()
