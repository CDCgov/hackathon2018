import requests

## hardcoded column IDs from sample data set
epiInfoCols = ["recordID", "caseID", "interviewerID", "interviewDate", "participation?","age", "sex", "homeless", "pwid", "otherDrugs", "DM"]
redCapCols = ["record_id", "case_id", "interviewer_id", "date_interview", "agreed_to_participate", "age", "gender", "riskFactors", "other_illicit_drugs", "dm"]

for redcapIndex in range(0, len(redCapCols)):
   redCapElement = redCapCols[redcapIndex]

   ## construct question query
   questionRequest = 'https://sdp-v.services.cdc.gov/api/questions?limit=200&search='
   userQuestionQuery = redCapElement
   questionQuery = questionRequest + userQuestionQuery

   ## submit query to SDP-vocabulary and structure as hierchical JSON
   questions = requests.get(questionQuery)
   questionJSON = questions.json()

   ## loop through responses to identify best match
   for index in range(0, len(questionJSON)):
       questionText = questionJSON[index]['questionText']
       responseType = questionJSON[index]['responseType']
