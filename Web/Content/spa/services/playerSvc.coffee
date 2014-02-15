# CoffeeScript

playerSvc = ($http, Get) ->

    getCollection = (tournamentId, callback) ->
        Get.playerCollection tournamentId, callback
        
    get = (tournamentId, playerId, callback) ->
        Get.player tournamentId, playerId, callback
    create = (createHref, model, callback) ->
        $http.post(createHref, model)
        .success (data, status, headers, config) ->
            callback data
    
    getCollection: getCollection
    get: get
    create: create
        

@chale.factory 'Player', ['$http', 'GetRepo', playerSvc]