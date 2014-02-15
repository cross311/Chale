# CoffeeScript

gameSvc = ($http, Get) ->

    getCollection = (tournamentId, callback) ->
        Get.gameCollection tournamentId, callback
        
    get = (tournamentId, gameId, callback) ->
        Get.game tournamentId, gameId, callback
    markWinner = (markWinnerHref, model, callback) ->
        $http.post(markWinnerHref, model)
        .success (data, status, headers, config) ->
            callback data
    
    getCollection: getCollection
    get: get
    markWinner: markWinner
        

@chale.factory 'Game', ['$http', 'GetRepo', gameSvc]