MONGO = require('mongodb').MongoClient;
BSON = require('mongodb').BSONPure;

var mongo = {
    "hostname":'paulo.mongohq.com',
    "port":10053,
    "username":"youdonotexist",
    "password":'AShipWithTimeToKill',
    "name":"",
    "db":"spacetime_me"
}

var generate_mongo_url = function(obj){
    obj.hostname = (obj.hostname || 'localhost');
    obj.port = (obj.port || 27017);
    obj.db = (obj.db || 'test');

    if(obj.username && obj.password){
        return 'mongodb://' + obj.username + ':' + obj.password + '@' + obj.hostname + ':' + obj.port + '/' + obj.db;
    }
    else{
        return "mongodb://" + obj.hostname + ":" + obj.port + "/" + obj.db;
    }
}

mongourl = generate_mongo_url(mongo);
connection = null;

mongo_connect = function(callback) {
    if (connection) {
        callback(connection);
        return;
    }

    MONGO.connect(mongourl, function(err, conn){
        if (conn && !err) {
            connection = conn;
            callback(conn);
        }
        else {
            console.log("Couldn't connect to DB: ");
            console.log(err);
        }

    });
}