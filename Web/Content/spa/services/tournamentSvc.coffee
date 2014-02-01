# CoffeeScript

tournamentSvc = ($http, Get) ->

    getCollection = (callback) ->
        Get.tournamentCollection callback
        
    get = (tournamentId, callback) ->
        tournament = {}
        $http.get("/tournaments/#{tournamentId}")
        .success (data, status, headers, config) ->
            callback data
        tournament
    
    getCollection: getCollection
    get: get
        

@chale.factory 'Tournament', ['$http', 'GetRepo', tournamentSvc]