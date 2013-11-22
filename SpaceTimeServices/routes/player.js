/*
 * Copyright 2013 SirsiDynix.  All rights reserved.
 */

/***
 *  Player Structure
 *
 *  _id {String}
    user_id {String}
 *  last_zone_id {String}
 *  last_location_id {String}
 *  last_location_local_pos {Object}
 *      x {double}
 *      y {double}
 */

exports.getplayers = function(req, res) {
    mongo_connect(function(db) {
        var id = req.query.userId;
        db.collection('player', function(err, collection) {
            collection.find({'user_id': id}).toArray(function(err, items) {
                res.send(items);
            });
        });
    });
};

exports.getplayer = function(req, res){
    mongo_connect(function(db) {
        var id = req.params.id;
        db.collection('player', function(err, collection) {
            collection.findOne({'_id': new BSON.ObjectID(id)}, function(err, item) {
                res.send(item);
            });
        });
    });
};

exports.createplayer = function(req, res){
    mongo_connect(function(db) {
        db.collection('player', function(err, collection) {
            var player = req.body;
            collection.insert(player, {safe:true}, function(err, result) {
                if (err) {
                    res.send({'error':'An error has occurred'});
                } else {
                    res.send(result[0]);
                }
            });
        });
    });
};