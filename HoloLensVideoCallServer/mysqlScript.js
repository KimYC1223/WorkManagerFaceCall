var mysql      = require('mysql')
let queryString = require('querystring')
var connection = mysql.createConnection({
  host     : 'localhost',
  user     : 'root',
  password : 'zerostone1!',
  port     : 3306,
  database : 'holocalldb'
});

/*

CREATE DATABASE holocalldb;
USE holocalldb;
CREATE TABLE holocall (CallState int,CallerIP char(13));
ALTER TABLE holocall convert to charset utf8;
INSERT INTO holocall (CallState,CallerIP) VALUES (0,null);


*/

connection.connect();
module.exports = (function() {
  return{
    callState: function (req,res) {
      connection.query(`SELECT * FROM holocall;`, function(error,rows, fields) {
        res.writeHead(200,{'Content-Type':'text/plain;charset=utf-8'})
        try{
          let str ='{\n'
          str += `\t"CallState" : ${rows[0].CallState},\n`
          str += `\t"CallerIP" : "${rows[0].CallerIP}"\n}`
          res.write(str)
        } catch (e) {res.write('null')}
        res.end();
      });
      return;
    },
    calling: function (req,res) {
        let ipAddress = req.param('ip');

        connection.query(`SELECT * FROM holocall;`, function(error,rows, fields) {
          let queryNum = rows[0].CallState;
          console.log(rows[0].CallState);
          console.log(ipAddress);
          connection.query(`UPDATE holocall SET CallState = 1, CallerIP = '${ipAddress}' WHERE CallState =${queryNum};`, function(error2,rows2,fields2) {
            res.send('calling...!');
          })
          return;
        }
      )
    },
    connecting: function (req,res) {
        connection.query(`SELECT * FROM holocall;`, function(error,rows, fields) {
          let queryNum = rows[0].CallState;
          connection.query(`UPDATE holocall SET CallState = 2 WHERE CallState ='${queryNum}';`, function(error2,rows2,fields2) {
            res.send('connecting...!');
          })
          return;
        }
      )
    },
    hangUp: function (req,res) {
        connection.query(`SELECT * FROM holocall;`, function(error,rows, fields) {
          let queryNum = rows[0].CallState;
          connection.query(`UPDATE holocall SET CallState = 0, CallerIP = NULL WHERE CallState =${queryNum};`, function(error2,rows2,fields2) {
            res.send('HangUp...!');
          })
          return;
        }
      )
    }
  }
})()
