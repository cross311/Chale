# CoffeeScript

tournamentSvc = ($http, Get) ->

    getCollection = (callback) ->
        Get.tournamentCollection callback
        
    get = (tournamentId, callback) ->
        Get.tournament tournamentId, callback
    create = (createHref, model, callback) ->
        $http.post(createHref, model)
        .success (data, status, headers, config) ->
            callback data
    
    getCollection: getCollection
    get: get
    create: create
        

@chale.factory 'Tournament', ['$http', 'GetRepo', tournamentSvc]