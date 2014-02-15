# CoffeeScript
getRepo = ($http, $log) ->
    
    tournamentCollection: (callback) ->
        tournamentVm = 
            tournaments: []
        $http
            method: 'get'
            url: '/tournaments'
        .success (data, status, headers, config) ->
            for t in data.Tournaments
                tournamentVm.tournaments.push t
            tournamentVm.createTournamentHref = data.CreateTournamentHref
            callback tournamentVm if angular.isFunction callback
        .error (data, status, headers, config) ->
            $log.error 
                data: data
                status: status
                headers: headers
                config: config
        tournamentVm
        
    tournament: (tournamentId, callback) ->
        tournamentVm = {}
        $http
            method: 'get'
            url: "/tournaments/#{tournamentId}"
        .success (data, status, headers, config) ->
            tournamentVm = data
            callback tournamentVm if angular.isFunction callback
        .error (data, status, headers, config) ->
            $log.error 
                data: data
                status: status
                headers: headers
                config: config
        tournamentVm
    
    playerCollection: (tournamentId, callback) ->
        playersVm = 
            players: []
        $http
            method: 'get'
            url: "/tournaments/#{tournamentId}/players"
        .success (data, status, headers, config) ->
            for p in data.Players
                playersVm.players.push p
            playersVm.createPlayerHref = data.CreatePlayerHref
            callback playersVm if angular.isFunction callback
        .error (data, status, headers, config) ->
            $log.error 
                data: data
                status: status
                headers: headers
                config: config
        playerVm
        
    player: (tournamentId, playerId, callback) ->
        playerVm = {}
        $http
            method: 'get'
            url: "/tournaments/#{tournamentId}/players/#{playerId}"
        .success (data, status, headers, config) ->
            playerVm = data
            callback playerVm if angular.isFunction callback
        .error (data, status, headers, config) ->
            $log.error 
                data: data
                status: status
                headers: headers
                config: config
        playerVm
    
    gameCollection: (tournamentId, callback) ->
        gamesVm = 
            games: []
        $http
            method: 'get'
            url: "/tournaments/#{tournamentId}/games"
        .success (data, status, headers, config) ->
            for g in data.Games
                gamesVm.games.push g
            gamesVm.markWinnerHref = data.MarkWinnerHref
            callback gamesVm if angular.isFunction callback
        .error (data, status, headers, config) ->
            $log.error 
                data: data
                status: status
                headers: headers
                config: config
        gamesVm
        
    game: (tournamentId, gameId, callback) ->
        gameVm = {}
        $http
            method: 'get'
            url: "/tournaments/#{tournamentId}/games/#{gameId}"
        .success (data, status, headers, config) ->
            gameVm = data
            callback gameVm if angular.isFunction callback
        .error (data, status, headers, config) ->
            $log.error 
                data: data
                status: status
                headers: headers
                config: config
        gameVm
    
@chale.factory 'GetRepo', ['$http', '$log', getRepo]