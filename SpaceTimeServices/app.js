
/**
 * Module dependencies.
 */

    //Libraries
var express = require('express'),
    routes = require('./routes'),
    http = require('http'),
    path = require('path'),
    db = require('./utilities/database.js'),

    //Resources
    ship = require('./routes/ship'),
    player = require('./routes/player'),
    user = require('./routes/user')
    ;

var app = express();

// all environments
app.set('port', process.env.PORT || 3000);
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'jade');
app.use(express.favicon());
app.use(express.logger('dev'));
app.use(express.json());
app.use(express.urlencoded());
app.use(express.methodOverride());
app.use(express.cookieParser('your secret here'));
app.use(express.session());
app.use(app.router);
app.use(express.static(path.join(__dirname, 'public')));

// development only
if ('development' == app.get('env')) {
  app.use(express.errorHandler());
}

app.get('/', routes.index);

app.get('/user', user.login);
app.post('/user', user.createuser);

app.get('/player', player.getplayers);
app.get('/player/:id', player.getplayer);
app.post('/player', player.createplayer);

app.get('/ship', ship.getships);
app.get('/ship/:id', ship.getship);
app.put('/ship/:id', ship.updateship);
app.post('/ship', ship.newship);

http.createServer(app).listen(app.get('port'), function(){
  console.log('Express server listening on port ' + app.get('port'));
});
