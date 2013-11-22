/*
 * Copyright 2013 SirsiDynix.  All rights reserved.
 */

exports.getships = function(req, res) {
    mongo_connect(function(db) {
        db.collection('ship', function(err, collection) {
            collection.find({}).toArray(function(err, items) {
                res.send(items);
            });
        });
    });
};

exports.getship = function(req, res){
    mongo_connect(function(db) {
        var id = req.params.id;
        db.collection('ship', function(err, collection) {
            collection.findOne({'_id': new BSON.ObjectID(id)}, function(err, item) {
                res.send(item);
            });
        });
    });
};

exports.newship = function(req, res){
    mongo_connect(function(db) {
        db.collection('ship', function(err, collection) {
            var ship = req.body;
            collection.insert(ship, {safe:true}, function(err, result) {
                if (err) {
                    res.send({'error':'An error has occurred'});
                } else {
                    res.send(result[0]);
                }
            });
        });
    });
};

exports.updateship = function (req, res) {
  res.send("respond with a resource");
};