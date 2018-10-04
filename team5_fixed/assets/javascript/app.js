/* var webSocket = $.simpleWebSocket({ url: 'http://alan.cs.gsu.edu:3030/' });
    
    // reconnected listening
    webSocket.listen(function(message) {
        console.log(message.text);
    });

    webSocket.send({ 'text': 'hello' }).done(function() {
        // message send
    }).fail(function(e) {
        // error sending
    }); */


/* 
const mysql = require('mysql');
const connection = mysql.createConnection({
    host: "http://alan.cs.gsu.edu",
    user: "deployme",
    password: "Vdq124uiyR4"
  database: 'database name'
});
connection.connect((err) => {
  if (err) throw err;
  console.log('Connected!');
}); */

$("#submit-btn").on("click", function(){
    let firstName = $("#first-name").val().trim();
    let lastName = $("#last-name").val().trim();

    $.ajax({
        url: "/deployme/test.py",
        type: "post",
        datatype: "json",
        async: false,
        data: {'firstName': firstName,'lastName': lastName, 'key':'dummyvalue'},

        success: function(response){
            console.log("yay!");
            console.log(response);
            alert(response.message);
            alert(response.keys);
        }
    });
});

$("#add-language-btn").on("click", function(){
    $(".language-box").append(
        `
        <div class="col-6 form-group">
            <label class="form-label" for="language">Languages</label>
            <select id="language" class="form-control">
                <option></option>
                <option>French</option>
                <option>Russian</option>
                <option>Korean</option>
            </select>
        </div>
        <div class="col-6 form-group">
            <label class="form-label" for="proficiency">Language Proficiency</label>
            <select id="proficiency" class="form-control">
                <option></option>
                <option>Novice</option>
                <option>Intermediate</option>
                <option>Advanced</option>
            </select>
        </div>
        `
    )
});

$("#add-skill-btn").on("click", function(){
    $(".health-skills-box").append(
        `
        <div class="col-6 form-group">
            <label class="form-label" for="health-comm-skills">Health Communication Skills</label>
            <select id="health-comm-skills" class="form-control form-check-label">
                <option></option>
                <option>Developing social media content and strategy</option>
                <option>Strategic communication planning</option>
                <option>Writing, editing, or creating content</option>
            </select>
        </div>
        <div class="col-6 form-group">
            <label class="form-label" for="proficiency">Skill Proficiency</label>
            <select id="proficiency" class="form-control">
                <option></option>
                <option>Novice</option>
                <option>Intermediate</option>
                <option>Advanced</option>
            </select>
        </div>
        `
    )
});

$("#add-language-btn-admin").on("click", function(){
    $(".language-box-admin").append(
        `
        <div class="col-6 form-group">
            <label class="form-label" for="language">Languages</label>
            <select id="language" class="form-control">
                <option></option>
                <option>French</option>
                <option>Russian</option>
                <option>Korean</option>
            </select>
        </div>
        <div class="col-6 form-group">
            <label class="form-label" for="proficiency">Language Proficiency</label>
            <select id="proficiency" class="form-control">
                <option></option>
                <option>Novice</option>
                <option>Intermediate</option>
                <option>Advanced</option>
            </select>
        </div>
        `
    )
});

$("#add-skill-btn").on("click", function(){
    $(".health-skills-box-admin").append(
        `
        <div class="col-6 form-group">
            <label class="form-label" for="health-comm-skills">Health Communication Skills</label>
            <select id="health-comm-skills" class="form-control form-check-label">
                <option></option>
                <option>Developing social media content and strategy</option>
                <option>Strategic communication planning</option>
                <option>Writing, editing, or creating content</option>
            </select>
        </div>
        <div class="col-6 form-group">
            <label class="form-label" for="proficiency">Skill Proficiency</label>
            <select id="proficiency" class="form-control">
                <option></option>
                <option>Novice</option>
                <option>Intermediate</option>
                <option>Advanced</option>
            </select>
        </div>
        `
    )
});
