var express       = require('express')        // call express
var app           = express()                  // define our app using express
var http          = require('http')
var path          = require('path')
var mysql         = require('mysql')


let mysqlScript = require('./mysqlScript.js')

app.use(express.static(path.join(__dirname, './')));

// Save our port
var port = 8000;

app.set('view engine','ejs');
app.get('/', (req,res) => {
  res.render(__dirname+'/index.ejs')
})

app.get('/Calling.jsp', (req,res) => {
  mysqlScript.calling(req,res)
})

app.get('/CallState.jsp', (req,res) => {
  mysqlScript.callState(req,res)
})

app.get('/Connecting.jsp', (req,res) => {
  mysqlScript.connecting(req,res)
})

app.get('/HangUp.jsp', (req,res) => {
  mysqlScript.hangUp(req,res)
})

app.get('/callingPage.jsp', (req,res) => {
  res.render(__dirname+'/cam.ejs')
})


// Start the server and listen on port
app.listen(port,function(){
  console.log("Web on port: " + port);
});
