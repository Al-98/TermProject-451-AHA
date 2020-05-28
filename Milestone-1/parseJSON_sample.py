import json

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def parseBusinessData():
    #read the JSON file
    with open('yelp_business.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('business.txt', 'w')
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
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
            outfile.write(str([item for item in  data['categories']])+'\t') #category list
            # no need for attribute  outfile.write(str([])) # write your own code to process attributes
            outfile.write(str(data['hours'])+'\t') # write your own code to process hours
            outfile.write('\n')

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

def parseUserData():
    #write code to parse yelp_user.JSON
    #might need re-arangeing 
    
    with open('yelp_user.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('users.txt', 'w')
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
            outfile.write(str([item for item in  data['friends']])+'\t') #friends list
            outfile.write('\n')

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()
    pass

def parseCheckinData():
    #write code to parse yelp_checkin.JSON
    with open('yelp_checkin.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('checkin.txt', 'w')
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(str(data['business_id'])+'\t') #business_id 
            outfile.write(str(data['time'])+'\t')
            #might need loop to print key then get value and print inside keys and then print values so if X is dict u print key and go in value
            # days = ["Monday", "Tuesday", "Friday", "Wednesday", "Thursday", "Sunday", "Saturday"]
            # time=data['time']
            # seven=-1
            # checkins=time[days[seven]]
            # while checkins:
                
            #     checkins=time[days[seven]]
            #     outfile.write(str(checkins)+'\t')
            #     seven = seven +1
            #     outfile.write('\n')

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()
    pass


def parseReviewData():
    #write code to parse yelp_review.JSON
    with open('yelp_review.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('review.txt', 'w')
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        #(review_id, user_id; business_id; stars; date; text; useful; funny; cool)
        while line:
            data = json.loads(line)
            outfile.write(str(data['review_id'])+'\t') #average stars
            outfile.write(str(data['user_id'])+'\t') #cool
            outfile.write(str(data['business_id'])+'\t') #funny
            outfile.write(str(data['stars'])+'\t') #useful
            outfile.write(str(data['date'])+'\t') #userid
            outfile.write(str(data['text'])+'\t') #since
            outfile.write(str(data['useful'])+'\t') #review count
            outfile.write(str(data['funny'])+'\t') #name 
            outfile.write(str(data['cool'])+'\t') #fans
            outfile.write('\n')

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()
    pass
    

#parseBusinessData()
#parseUserData()
parseCheckinData()
#parseReviewData()
