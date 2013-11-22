
/*
 * GET users listing.
 */

/****
 * User Structure
 *
 *  _id {String}
 *  username {String}
 *  password {String}
 *  email {string}
 */

/**
 *
 * @param req
 * @param res
 */

exports.list = function(req, res){
  res.send("respond with a resource");
};

exports.login = function(req, res) {
    mongo_connect(function(db) {
        var id = req.params.id;
        db.collection('user', function(err, collection) {
            collection.findOne({'username': req.query.username}, function(err, item) {
                if (item.password == req.query.password) {
                    res.send({
                        '_id': item._id,
                        'success': true,
                        'token': '12345678'
                    });
                }
                else {
                    res.send({
                        'success': false
                    })

                }
            });
        });
    });
};

exports.createuser = function(req, res) {
    mongo_connect(function(db) {
        db.collection('user', function(err, collection) {
            var newuser = req.body;
            console.log(newuser.username);
            console.log(newuser.password);
            var insert = {
                username: newuser.username,
                password: newuser.password
            };
            collection.insert(insert, {safe:true}, function(err, result) {
                if (err) {
                    res.send({'error':'An error has occurred'});
                } else {
                    res.send(result[0]);
                }
            });
        });
    });
}